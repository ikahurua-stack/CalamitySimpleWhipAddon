using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Rendering;
using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Systems;
using CalamityMod.NPCs;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class WhipChargeProjectile : ModProjectile
    {

        public ChargeTheme Theme =>
            (ChargeTheme)(int)Projectile.ai[1];

        private Vector2 previousCenter;
        private bool initialized;

        public int OrbitIndex;

        private Vector2 orbitOffset;
        private Vector2 orbitVelocity;
        private bool orbitInitialized;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Summon;

            Projectile.penetrate = 1;

            Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.timeLeft = 300;

            Projectile.ArmorPenetration = 9999;

        }

        public override void AI()
        {
            if (Projectile.ai[2] == 0f)
            {
                OrbitAI();
            }
            else
            {
                ReleaseAI();
            }

            if (++Projectile.localAI[0] >= 2)
            {
                Projectile.localAI[0] = 0;
                Projectile.netUpdate = true;
            }
        }

        private void OrbitAI()
        {
            if (Projectile.ai[2] != 0f)
                return;

            Projectile coreProjectile = FindCore();

            if (coreProjectile == null ||
                coreProjectile.ModProjectile is not ChargeCoreProjectile core)
            {
                Projectile.Kill();
                return;
            }

            float coreRadius =
                ChargeVisual.MassToRadius(core.Mass);

            float maxDistance =
                coreRadius * 0.9f;

            // 初回だけ適当な位置に配置
            if (!orbitInitialized)
            {
                orbitInitialized = true;

                orbitOffset =
                    Main.rand.NextVector2CircularEdge(1f, 1f) *
                    Main.rand.NextFloat(coreRadius * 0.2f, maxDistance);

                orbitVelocity =
                    Main.rand.NextVector2Circular(2f, 2f);
            }

            // コアへ引っ張る
            orbitVelocity += -orbitOffset * 0.025f;

            // ランダムに蠢く
            orbitVelocity +=
                Main.rand.NextVector2Circular(0.25f, 0.25f);

            // 他の頭と軽く反発
            foreach (Projectile other in Main.ActiveProjectiles)
            {
                if (other.whoAmI == Projectile.whoAmI ||
                    other.owner != Projectile.owner ||
                    other.ModProjectile is not WhipChargeProjectile ||
                    other.ai[2] != 0f)
                {
                    continue;
                }

                Vector2 diff =
                    Projectile.Center - other.Center;

                float dist = diff.Length();

                if (dist > 0f && dist < coreRadius * 0.7f)
                {
                    orbitVelocity +=
                        diff / dist *
                        (coreRadius * 0.35f - dist) *
                        0.008f;
                }

                float preferred = coreRadius * 0.28f;

                if (dist > 1f)
                {
                    orbitVelocity +=
                        diff / dist *
                        (preferred - dist) *
                        0.004f;
                }
            }

            // 摩擦
            orbitVelocity *= 0.92f;

            orbitOffset += orbitVelocity;

            // コアから離れすぎない
            float len = orbitOffset.Length();

            if (len > maxDistance)
            {
                orbitOffset =
                    orbitOffset / len * maxDistance;

                orbitVelocity *= 0.4f;
            }

            Projectile.Center =
                coreProjectile.Center +
                orbitOffset;

            Projectile.rotation =
                orbitOffset.ToRotation();

            Projectile.velocity = Vector2.Zero;

            Projectile.timeLeft = 2;

            Projectile.ai[0] =
                core.StoredDamage /
                Math.Max(CountOrbitProjectiles(), 1);

            WhipProjectileSync.Register(Projectile);

            UpdateFrames();
        }


        private void ReleaseAI()
        {
            NPC target = WhipChargeSystem.FindTarget(Main.player[Projectile.owner]);

            Projectile.timeLeft = 300;

            Projectile.friendly = false;

            if (!initialized)
            {
                previousCenter = Projectile.Center;
                initialized = true;
            }

            float radius =
                ChargeVisual.DamageToRadius(Projectile.ai[0]);

            float distance =
                Vector2.Distance(previousCenter, Projectile.Center);

            int steps =
                Math.Max(1, (int)(distance / (radius * 0.1f)));

            for (int i = 0; i <= steps; i++)
            {
                Vector2 pos =
                    Vector2.Lerp(previousCenter,
                                 Projectile.Center,
                                 i / (float)steps);

                ChargeMetaballData.TrailParticles.Add(new TrailParticle
                {
                    Theme = Theme,
                    Center = pos,
                    Size = radius,
                    Life = 20
                });
            }

            previousCenter = Projectile.Center;

            if (target != null)
            {
                Vector2 toTarget = target.Center - Projectile.Center;

                float speed = 24f;

                toTarget.Normalize();

                Projectile.velocity =
                    Vector2.Lerp(
                        Projectile.velocity,
                        toTarget * speed,
                        0.1f); // ←追尾強さ

                if (Vector2.Distance(Projectile.Center, target.Center) <= 10f)
                {
                    Projectile.friendly = true;
                }
            }

            if (Projectile.velocity.LengthSquared() > 0.01f)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }


            // メタボール同期用データ
            WhipProjectileSync.Register(Projectile);

            UpdateFrames();
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

        private int CountOrbitProjectiles()
        {
            int count = 0;

            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.owner == Projectile.owner &&
                    projectile.ModProjectile is WhipChargeProjectile &&
                    projectile.ai[2] == 0f)
                {
                    count++;
                }
            }

            return count;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(orbitOffset);
            writer.WriteVector2(orbitVelocity);
            writer.Write(orbitInitialized);
            writer.Write(Projectile.rotation);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            orbitOffset = reader.ReadVector2();
            orbitVelocity = reader.ReadVector2();
            orbitInitialized = reader.ReadBoolean();
            Projectile.rotation = reader.ReadSingle();
        }

        public void UpdateFrames()
        {
            int maxFrame = 6;
            Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter / 5 % maxFrame;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            switch (Theme)
            {
                case ChargeTheme.massofWailing:
                    {
                        break;
                    }

                case ChargeTheme.chorusofExecration:
                    {
                        Texture2D tex =
                            ModContent.Request<Texture2D>(
                                "CalamityMod/Projectiles/Magic/SpiritCongregation").Value;

                        ChargeHeadDrawer.DrawHead(
                            tex,
                            Projectile.Center,
                            Projectile.rotation,
                            ChargeVisual.DamageToRadius(Projectile.ai[0]),
                            Projectile.frame);
                        break;
                    }
            }

            return false;
        }


        public void DrawHeadForMetaball()
        {
            switch (Theme)
            {
                case ChargeTheme.massofWailing:

                    break;

                case ChargeTheme.chorusofExecration:

                    ChargeHeadDrawer.DrawHead(ModContent.Request<Texture2D>("CalamityMod/Projectiles/Magic/SpiritCongregationBack").Value, 
                            Projectile.Center,
                            Projectile.rotation,
                            ChargeVisual.DamageToRadius(Projectile.ai[0]),
                            Projectile.frame);

                    break;
            }
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(
                        SoundID.NPCHit20 with { Pitch = Main.rand.NextFloat(0.8f, 1.1f), Volume = 4.5f },
                        Projectile.Center
                    );

            float mass = Projectile.ai[0];

            BaseChargeMetaball.SpawnExplosionMetaballs(
                Projectile.Center,
                10,
                mass, Theme);

            Projectile.Kill();
        }
    }
}
