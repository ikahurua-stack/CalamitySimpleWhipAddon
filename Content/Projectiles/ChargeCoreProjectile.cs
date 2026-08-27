using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Rendering;
using CalamitySimpleWhipAddon.Content.Systems;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class ChargeCoreProjectile : ModProjectile
    {
        private const int MaxOrbitProjectiles = 10;

        public override string Texture => "CalamitySimpleWhipAddon/Content/Projectiles/WhipChargeProjectile";

        public float StoredDamage
        {
            get => Projectile.ai[0];
            private set => Projectile.ai[0] = value;
        }

        public ChargeTheme Theme
        {
            get => (ChargeTheme)(int)Projectile.ai[1];
            private set => Projectile.ai[1] = (float)value;
        }

        public float StoredCharge
        {
            get => Projectile.ai[2];
            private set => Projectile.ai[2] = value;
        }

        public float Mass;
        public float Noise;

        private Vector2 coreVelocity;
        private int releaseCooldown;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            ChargeTheme heldTheme = WhipChargeSystem.GetHeldTheme(player);

            if (heldTheme == ChargeTheme.None)
            {
                Projectile.Kill();
                return;
            }

            if (Theme == ChargeTheme.None)
                Theme = heldTheme;

            Vector2 targetPos = player.Center + new Vector2(0f, -120f);

            coreVelocity += (targetPos - Projectile.Center) * 0.07f;
            coreVelocity *= 0.90f;
            Projectile.Center += coreVelocity;
            Projectile.velocity = Vector2.Zero;

            Mass = MathHelper.Lerp(Mass, ChargeVisual.DamageToMass(StoredDamage), 0.08f);
            Noise += 0.08f;

            Lighting.AddLight(
                Projectile.Center,
                0.04f * Mass,
                0.008f * Mass,
                0.008f * Mass);

            if (releaseCooldown > 0)
                releaseCooldown--;

            if (Projectile.owner == Main.myPlayer)
            {
                UpdateOrbitProjectiles(player);

                if (StoredCharge >= WhipChargeSystem.ReleaseThreshold &&
                    releaseCooldown <= 0 &&
                    CountOrbitProjectiles() > 0)
                {
                    Release(player);
                }
            }

            Projectile.timeLeft = 2;

            if (++Projectile.localAI[0] >= 2)
            {
                Projectile.localAI[0] = 0;
                Projectile.netUpdate = true;
            }
        }

        public void Absorb(ChargeBallProjectile ball)
        {
            if (ball.Theme != Theme)
            {
                Theme = ball.Theme;
                StoredDamage = 0f;
                StoredCharge = 0f;
                Mass = 0f;
                KillOrbitProjectiles();
            }

            StoredDamage += ball.PendingDamage;
            StoredCharge += ball.PendingCharge;
            Projectile.netUpdate = true;
        }

        private void UpdateOrbitProjectiles(Player player)
        {
            int desired = Math.Min((int)(StoredCharge / 10f), MaxOrbitProjectiles);
            int activeCount = CountOrbitProjectiles();

            while (activeCount < desired)
            {
                int id = Projectile.NewProjectile(
                    player.GetSource_Misc("Orbit"),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<WhipChargeProjectile>(),
                    0,
                    0,
                    player.whoAmI,
                    0f,
                    (float)Theme,
                    0f);

                if (id >= 0 && id < Main.maxProjectiles)
                    Main.projectile[id].netUpdate = true;

                activeCount++;
            }

            while (activeCount > desired)
            {
                Projectile orbit = FindLastOrbitProjectile();

                if (orbit == null)
                    break;

                orbit.Kill();
                activeCount--;
            }
        }

        private void Release(Player player)
        {
            float totalDamage = StoredDamage;
            float damagePerShot = totalDamage / MaxOrbitProjectiles;
            float massPerShot = totalDamage / MaxOrbitProjectiles;

            BaseChargeMetaball.SpawnExplosionMetaballs(Projectile.Center, CountOrbitProjectiles(), totalDamage, Theme);

            SoundEngine.PlaySound(
                SoundID.Item176 with { Pitch = Main.rand.NextFloat(0.8f, 1.1f), Volume = 2.5f },
                Projectile.Center);

            foreach (Projectile orbit in Main.ActiveProjectiles)
            {
                if (!IsOrbitProjectile(orbit))
                    continue;

                NPC releaseTarget = WhipChargeSystem.FindTarget(player);

                Vector2 dir = releaseTarget != null
                    ? (releaseTarget.Center - orbit.Center).SafeNormalize(Vector2.UnitX)
                    : Main.rand.NextVector2Unit();

                dir = WhipChargeSystem.GetPerpendicularBothSides(
                    -dir,
                    Main.rand.NextFloat(16f, 50f),
                    1.05f);

                orbit.velocity = dir;
                orbit.damage = (int)damagePerShot;
                orbit.ai[0] = massPerShot;
                orbit.ai[1] = (float)Theme;
                orbit.ai[2] = 1f;
                orbit.netUpdate = true;
            }

            StoredDamage = 0f;
            StoredCharge = 0f;
            Mass = 0f;
            releaseCooldown = 30;
            Projectile.netUpdate = true;
        }

        private int CountOrbitProjectiles()
        {
            int count = 0;

            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (IsOrbitProjectile(projectile))
                    count++;
            }

            return count;
        }

        private Projectile FindLastOrbitProjectile()
        {
            for (int i = Main.maxProjectiles - 1; i >= 0; i--)
            {
                Projectile projectile = Main.projectile[i];

                if (IsOrbitProjectile(projectile))
                    return projectile;
            }

            return null;
        }

        private void KillOrbitProjectiles()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];

                if (IsOrbitProjectile(projectile))
                    projectile.Kill();
            }
        }

        private bool IsOrbitProjectile(Projectile projectile)
        {
            return projectile.active &&
                projectile.owner == Projectile.owner &&
                projectile.ModProjectile is WhipChargeProjectile &&
                projectile.ai[2] == 0f;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(coreVelocity);
            writer.Write(Mass);
            writer.Write(Noise);
            writer.Write(releaseCooldown);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            coreVelocity = reader.ReadVector2();
            Mass = reader.ReadSingle();
            Noise = reader.ReadSingle();
            releaseCooldown = reader.ReadInt32();
        }

        public override bool PreDraw(ref Microsoft.Xna.Framework.Color lightColor) => false;
    }
}
