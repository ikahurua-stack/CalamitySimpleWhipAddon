using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using CalamityMod.Particles;
using CalamityMod.Dusts;
using CalamitySimpleWhipAddon.Content.Projectiles;

namespace CalamitySimpleWhipAddon.Content.Systems
{
    public static class CalamityGemBurstFX
    {
        public static void Play(
            Vector2 position,
            Color color1,
            Color color2,
            Color color3,
            int dustType)
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
                        color1
                    )
                );

                GeneralParticleHandler.SpawnParticle(
                    new PointParticle(
                        position,
                        Main.rand.NextVector2Circular(4f, 4f),
                        false,
                        10,
                        Main.rand.NextFloat(0.5f, 1.5f),
                        color2
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
                        color3
                    )
                );
            }

            for (int i = 0; i < 4; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    position,
                    dustType,
                    Main.rand.NextVector2Circular(2f, 2f)
                );

                d.noGravity = true;
                d.scale = 1.1f;
                d.fadeIn = 0.6f;
            }

        }

    }
}