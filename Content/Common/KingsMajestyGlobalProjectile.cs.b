using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamitySimpleWhipAddon.Content.Common.GlobalNPCs;
using CalamityMod.Particles;
using CalamityMod.Dusts;

namespace CalamitySimpleWhipAddon.Content.Common
{
    public class KingsMajestyGlobalProjectile : GlobalProjectile
    {
        public override void OnHitNPC(
            Projectile projectile,
            NPC target,
            NPC.HitInfo hit,
            int damageDone)
        {
            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

            var tag = target.GetGlobalNPC<WhipGemNPC>();

            if (!tag.GemMarked)
                return;

            Projectile gem = null;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];

                if (p.active &&
                    p.owner == projectile.owner &&
                    p.type == ModContent.ProjectileType<KingsMajestyGem>())
                {
                    gem = p;
                    break;
                }
            }

            if (gem == null)
                return;

            // ★ ここが重要：クールダウン中は何もしない
            if (tag.cooldownTimer > 0)
                return;

            // ★ 溜め開始
            tag.chargeTimer++;

            // ★ 20F溜めたら発射
            if (tag.chargeTimer < 20)
                return;

            tag.chargeTimer = 0;
            tag.cooldownTimer = 30;

            SoundEngine.PlaySound(
                new SoundStyle("CalamityMod/Sounds/Custom/RedJewelFire"),
                gem.Center);

            for (int i = 0; i < 6; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(6f, 6f);

                Dust d = Dust.NewDustPerfect(
                    gem.Center,
                    DustID.GemRuby,
                    vel);

                d.noGravity = true;
                d.scale = 1.3f;
                d.fadeIn = 0.6f;
            }

            CalamityGemBurstFX.Play(gem.Center);

            if (gem.active && gem.type == ModContent.ProjectileType<KingsMajestyGem>())
            {
                ((KingsMajestyGem)gem.ModProjectile).TriggerFlash();
            }

            Vector2 velocity =
                (target.Center - gem.Center).SafeNormalize(Vector2.UnitY) * 12f;

            Projectile.NewProjectile(
                projectile.GetSource_OnHit(target),
                gem.Center,
                velocity,
                ModContent.ProjectileType<KingsMajestyShot>(),
                damageDone / 2,
                0f,
                projectile.owner);
        }

        
    }

    public static class CalamityGemBurstFX
    {
        public static void Play(Vector2 position)
        {
            for (int i = 0; i < 6; i++)
            {
                GeneralParticleHandler.SpawnParticle(
                    new PointParticle(
                        position,
                        Main.rand.NextVector2Circular(6f, 6f),
                        false,
                        10,
                        Main.rand.NextFloat(0.5f, 1.5f),
                        Color.Red
                    )
                );

                GeneralParticleHandler.SpawnParticle(
                    new PointParticle(
                        position,
                        Main.rand.NextVector2Circular(4f, 4f),
                        false,
                        10,
                        Main.rand.NextFloat(0.5f, 1.5f),
                        Color.Pink
                    )
                );
            }

            for (int i = 0; i < 8; i++)
            {
                GeneralParticleHandler.SpawnParticle(
                    new PointParticle(
                        position,
                        Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) *
                        Main.rand.NextFloat(3f, 8f),
                        false,
                        12,
                        Main.rand.NextFloat(0.8f, 1.3f),
                        Color.HotPink
                    )
                );
            }

            for (int i = 0; i < 4; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    position,
                    DustID.GemRuby,
                    Main.rand.NextVector2Circular(2f, 2f)
                );

                d.noGravity = true;
                d.scale = 1.1f;
                d.fadeIn = 0.6f;
            }

            SoundEngine.PlaySound(
                new SoundStyle("CalamityMod/Sounds/Custom/RedJewelFire"),
                position);
        }
    }
}