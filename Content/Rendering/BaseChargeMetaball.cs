using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using CalamityMod.Graphics.Metaballs;
using CalamityMod.Enums;
using CalamityMod;
using CalamitySimpleWhipAddon.Content.Systems;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamitySimpleWhipAddon.Content.Items.Weapons;
using Steamworks;

namespace CalamitySimpleWhipAddon.Content.Rendering
{
    public abstract class BaseChargeMetaball : Metaball
    {
        protected abstract ChargeTheme Theme { get; }

        public static Asset<Texture2D> LayerAsset;

        public static Asset<Texture2D> CurrentLayer;

        public override bool AnythingToDraw
        {
            get
            {
                if (ChargeMetaballData.Bodies.Count > 0)
                    return true;

                if (ChargeMetaballData.Particles.Count > 0)
                    return true;

                if (ChargeMetaballData.TrailParticles.Count > 0)
                    return true;

                if (ChargeMetaballData.ExplosionParticles.Count > 0)
                    return true;

                if (ChargeMetaballData.ExplosionBodies.Count > 0)
                    return true;

                foreach (Projectile p in Main.ActiveProjectiles)
                {
                    if (p.ModProjectile is ChargeCoreProjectile core &&
                        core.Theme == Theme)
                        return true;

                    if (p.ModProjectile is ChargeBallProjectile ball &&
                        ball.Theme == Theme)
                        return true;

                    if (p.ModProjectile is WhipChargeProjectile whip &&
                        whip.Theme == Theme)
                        return true;
                }

                return false;
            }
        }

        public abstract override IEnumerable<Texture2D> Layers
        {
            get;
        }

        public override GeneralDrawLayer DrawLayer =>
            GeneralDrawLayer.BeforeProjectiles;

        public abstract override Color EdgeColor
        {
            get;
        }

        public static Asset<Texture2D> LayerWailing;
        public static Asset<Texture2D>[] LayerExecration;


        public abstract override Vector2 CalculateManualOffsetForLayer(int layerIndex);

        public override void ClearInstances()
        {
            
        }

        public override void Update()
        {
            for (int i = ChargeMetaballData.Particles.Count - 1; i >= 0; i--)
            {

                ChargeParticle p = ChargeMetaballData.Particles[i];


                // =====================================================
                // 慣性運動
                // =====================================================
                p.Center += p.Velocity;
                p.Velocity *= Main.rand.NextFloat(0.94f, 0.98f);
                p.Velocity.Y -= Main.rand.NextFloat(0.5f, 1.5f);

                // =====================================================
                // 消滅は「縮小のみ」
                // =====================================================
                if (p.Life < p.MaxLife * 0.9f)
                {
                    p.Size *= Main.rand.NextFloat(0.5f, 0.85f);
                }

                p.Life--;

                if (p.Size < 0.01f)
                {
                    ChargeMetaballData.Particles.RemoveAt(i);
                    continue;
                }


                ChargeMetaballData.Particles[i] = p;
            }

            for (int i = ChargeMetaballData.Bodies.Count - 1; i >= 0; i--)
            {
                ChargeBody b = ChargeMetaballData.Bodies[i];

                b.Size *= 0.9f;
                b.Life--;

                if (b.Life <= 0 || b.Size < 0.5f)
                {
                    ChargeMetaballData.Bodies.RemoveAt(i);
                    continue;
                }

                ChargeMetaballData.Bodies[i] = b;
            }

            for (int i = ChargeMetaballData.TrailParticles.Count - 1; i >= 0; i--)
            {
                TrailParticle p = ChargeMetaballData.TrailParticles[i];

                p.Size *= 0.9f;
                p.Life--;

                if (p.Life <= 0 || p.Size < 0.5f)
                {
                    ChargeMetaballData.TrailParticles.RemoveAt(i);
                    continue;
                }

                ChargeMetaballData.TrailParticles[i] = p;
            }

            for (int i = ChargeMetaballData.ExplosionBodies.Count - 1; i >= 0; i--)
            {
                ExplosionBody p = ChargeMetaballData.ExplosionBodies[i];

                p.Size *= 0.93f;
                p.Life--;

                if (p.Life <= 0 || p.Size < 0.5f)
                {
                    ChargeMetaballData.ExplosionBodies.RemoveAt(i);
                    continue;
                }

                ChargeMetaballData.ExplosionBodies[i] = p;
            }

            for (int i = ChargeMetaballData.ExplosionParticles.Count - 1; i >= 0; i--)
            {
                ExplosionParticle p = ChargeMetaballData.ExplosionParticles[i];


                p.Center += p.Velocity;
                p.Velocity *= Main.rand.NextFloat(0.94f, 0.98f);
                p.Velocity.Y += Main.rand.NextFloat(0.1f, 0.5f);
                p.Size *= 0.95f;
                p.Life--;

                if (p.Life <= 0 || p.Size < 0.5f)
                {
                    ChargeMetaballData.ExplosionParticles.RemoveAt(i);
                    continue;
                }

                ChargeMetaballData.ExplosionParticles[i] = p;
            }

        }


        public static void SpawnExplosionMetaballs(Vector2 center, int count, float mass, ChargeTheme theme)
        {
            int spawnCount =
                (int)MathHelper.Clamp(count / 5f, 8f, 25f);

            for (int i = 0; i < spawnCount; i++)
            {
                float radius =
                    ChargeVisual.DamageToRadius(
                        mass);

                Vector2 velocity =
                    Main.rand.NextVector2Circular(7f, 7f);


                ChargeMetaballData.ExplosionBodies.Add(new ExplosionBody
                {
                    Theme = theme,

                    Center = center + Main.rand.NextVector2Circular(6f, 6f),
                    Size = radius * 1.8f,
                    Life = 30
                });

                ChargeMetaballData.ExplosionParticles.Add(new ExplosionParticle
                {
                    Theme = theme,

                    Center = center,
                    Velocity = velocity,
                    Size = radius * 0.85f,
                    Life = 30
                });
            }
        }

        public override void DrawInstances()
        {
            Texture2D tex =
                ModContent.Request<Texture2D>(
                    "CalamityMod/ExtraTextures/BasicCircle").Value;

            foreach (var p in ChargeMetaballData.Bodies)
            {
                if (p.Theme != Theme)
                    continue;

                Main.spriteBatch.Draw(
                    tex,
                    p.Center - Main.screenPosition,
                    null,
                    Color.White,
                    0f,
                    tex.Size() * 0.5f,
                    p.Size / tex.Width,
                    SpriteEffects.None,
                    0f
                );
            }

            foreach (var p in ChargeMetaballData.Particles)
            {
                if (p.Theme != Theme)
                    continue;

                float scale = p.Size / tex.Width;

                Main.spriteBatch.Draw(
                    tex,
                    p.Center - Main.screenPosition,
                    null,
                    Color.White,
                    0f,
                    tex.Size() * 0.5f,
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }

            foreach (var p in ChargeMetaballData.TrailParticles)
            {
                if (p.Theme != Theme)
                    continue;

                Main.spriteBatch.Draw(
                    tex,
                    p.Center - Main.screenPosition,
                    null,
                    Color.White,
                    p.Rotation,
                    tex.Size() * 0.5f,
                    p.Size / tex.Width,
                    SpriteEffects.None,
                    0f);
            }

            foreach (var p in ChargeMetaballData.ExplosionBodies)
            {
                if (p.Theme != Theme)
                    continue;

                Main.spriteBatch.Draw(
                    tex,
                    p.Center - Main.screenPosition,
                    null,
                    Color.White,
                    0f,
                    tex.Size() * 0.5f,
                    p.Size / tex.Width,
                    SpriteEffects.None,
                    0f);
            }

            foreach (var p in ChargeMetaballData.ExplosionParticles)
            {
                if (p.Theme != Theme)
                    continue;

                Main.spriteBatch.Draw(
                    tex,
                    p.Center - Main.screenPosition,
                    null,
                    Color.White,
                    p.Rotation,
                    tex.Size() * 0.5f,
                    p.Size / tex.Width,
                    SpriteEffects.None,
                    0f);
            }


            foreach (Projectile proj in Main.projectile)
            {
                if (!proj.active)
                    continue;

                if (proj.ModProjectile is ChargeCoreProjectile core)
                {
                    if (core.Theme != Theme)
                        continue;

                    switch (core.Theme)
                    {
                        case ChargeTheme.massofWailing:
                            DrawMassOfWailingCore(proj, core);
                            break;

                        case ChargeTheme.chorusofExecration:
                            DrawChorusCore(proj, core);
                            break;
                    }

                    continue;
                }

                if (proj.ModProjectile is ChargeBallProjectile ball)
                {
                    if (ball.Theme != Theme)
                        continue;

                    switch (ball.Theme)
                    {
                        case ChargeTheme.massofWailing:
                            DrawWailingBall(proj, ball);
                            break;

                        case ChargeTheme.chorusofExecration:
                            DrawExecrationBall(proj, ball);
                            break;
                    }

                    continue;
                }

                if (proj.ModProjectile is not WhipChargeProjectile whip)
                    continue;

                if (whip.Theme != Theme)
                    continue;

                switch (whip.Theme)
                {
                    case ChargeTheme.massofWailing:

                        DrawWailingProjectile(proj);

                        break;

                    case ChargeTheme.chorusofExecration:

                        DrawExecrationProjectile(proj);

                        break;
                }
            }

            int eminenceType = ModContent.ProjectileType<WhipChargeProjectile>();
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.type != eminenceType)
                    continue;

                var whip = p.ModProjectile as WhipChargeProjectile;

                if (whip == null)
                    continue;

                if (whip.Theme != Theme)
                    continue;

                whip.DrawHeadForMetaball();
            }

        }


        private void DrawMassOfWailingCore(Projectile projectile, ChargeCoreProjectile core)
        {
            Texture2D tex =
                ModContent.Request<Texture2D>(
                    "CalamityMod/ExtraTextures/BasicCircle").Value;

            float pulse =
                1f +
                0.05f *
                (float)Math.Sin(core.Noise * 2f);

            float radius =
                ChargeVisual.MassToRadius(core.Mass) * pulse;

            Vector2 center =
                projectile.Center;

            Main.spriteBatch.Draw(
                tex,
                center - Main.screenPosition,
                null,
                Color.White,
                0f,
                tex.Size() * 0.5f,
                radius / tex.Width,
                SpriteEffects.None,
                0f);

            for (int i = 0; i < 5; i++)
            {
                float angle =
                    MathHelper.TwoPi / 5f * i +
                    core.Noise;

                float radius2 =
                    radius *
                    (0.3f +
                     0.08f *
                     (float)Math.Sin(core.Noise * 4f + i));

                Vector2 offset =
                    angle.ToRotationVector2() *
                    radius2;

                float childSize =
                    radius *
                    Main.rand.NextFloat(0.45f, 0.65f);

                Main.spriteBatch.Draw(
                    tex,
                    center + offset - Main.screenPosition,
                    null,
                    Color.White,
                    0f,
                    tex.Size() * 0.5f,
                    childSize / tex.Width,
                    SpriteEffects.None,
                    0f);
            }
        }

        private void DrawChorusCore(Projectile projectile, ChargeCoreProjectile core)
        {
            DrawMassOfWailingCore(projectile, core);
        }

        private void DrawWailingBall(Projectile projectile, ChargeBallProjectile ball)
        {
            Texture2D tex =
                ModContent.Request<Texture2D>(
                    "CalamityMod/ExtraTextures/BasicCircle").Value;

            float radius =
                ChargeVisual.MassToRadius(ball.Mass);

            Main.spriteBatch.Draw(
                tex,
                projectile.Center - Main.screenPosition,
                null,
                Color.White,
                0f,
                tex.Size() * 0.5f,
                radius / tex.Width,
                SpriteEffects.None,
                0f);
        }

        private void DrawExecrationBall(Projectile projectile, ChargeBallProjectile ball)
        {
            DrawWailingBall(projectile, ball);
        }

        private void DrawWailingProjectile(Projectile proj)
        {
            Texture2D tex =
                ModContent.Request<Texture2D>(
                    "CalamityMod/ExtraTextures/BasicCircle").Value;

            float radius =
                    ChargeVisual.DamageToRadius(proj.ai[0]);

            Main.spriteBatch.Draw(
                tex,
                proj.Center - Main.screenPosition,
                null,
                Color.White,
                proj.rotation,
                tex.Size() * 0.5f,
                radius / tex.Width,
                SpriteEffects.None,
                0f);
        }

        private void DrawExecrationProjectile(Projectile proj)
        {
            DrawWailingProjectile(proj);
        }


    }
}
