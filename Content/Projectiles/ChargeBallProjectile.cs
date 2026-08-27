using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Rendering;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class ChargeBallProjectile : ModProjectile
    {
        public override string Texture => "CalamitySimpleWhipAddon/Content/Projectiles/WhipChargeProjectile";

        public float PendingDamage => Projectile.ai[0];

        public ChargeTheme Theme => (ChargeTheme)(int)Projectile.ai[1];

        public float PendingCharge => Projectile.ai[2];

        public float Mass => ChargeVisual.DamageToMass(PendingDamage);

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            Projectile coreProjectile = FindCore();

            if (coreProjectile == null ||
                coreProjectile.ModProjectile is not ChargeCoreProjectile core)
            {
                Projectile.Kill();
                return;
            }

            Vector2 toCenter = coreProjectile.Center - Projectile.Center;
            float dist = toCenter.Length();

            if (dist < 10f)
            {
                if (Projectile.owner == Main.myPlayer)
                    core.Absorb(this);

                SoundEngine.PlaySound(
                    SoundID.Item154 with { Pitch = Main.rand.NextFloat(0.8f, 1.1f), Volume = 1.5f },
                    coreProjectile.Center);

                Projectile.Kill();
                return;
            }

            Vector2 direction = toCenter.SafeNormalize(Vector2.Zero);

            Projectile.velocity = Vector2.Lerp(
                Projectile.velocity,
                direction * 24f,
                0.08f);

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (++Projectile.localAI[0] >= 2)
            {
                Projectile.localAI[0] = 0;
                Projectile.netUpdate = true;
            }
        }

        private Projectile FindCore()
        {
            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.owner == Projectile.owner &&
                    projectile.ModProjectile is ChargeCoreProjectile)
                {
                    return projectile;
                }
            }

            return null;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.rotation);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.rotation = reader.ReadSingle();
        }

        public override bool PreDraw(ref Color lightColor) => false;
    }
}
