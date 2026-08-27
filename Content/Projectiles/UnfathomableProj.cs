using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{


    public class UnfathomableProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 80;
            Projectile.WhipSettings.RangeMultiplier = 2.1f;
        }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private float ChargeTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }



        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuff25>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.97f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);

            SimpleWhipDrawer4.DrawSegments(Projectile, points, Timer);

            return false;
        }

        public override void AI()
        {
            base.AI();

            Player owner = Main.player[Projectile.owner];

            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            float swingProgress = Projectile.ai[0] / swingTime;

            if (Utils.GetLerpValue(0.1f, 0.7f, swingProgress, true) *
                Utils.GetLerpValue(0.9f, 0.7f, swingProgress, true) > 0.5f &&
                !Main.rand.NextBool(30))
            {
                List<Vector2> points = Projectile.WhipPointsForCollision;

                int pointIndex = Main.rand.Next(points.Count - 20, points.Count);
                Rectangle spawnArea = Utils.CenteredRectangle(points[pointIndex], new Vector2(30f, 30f));

                int dustType = DustID.Water;
                if (Main.rand.NextBool(10))
                    dustType = DustID.Water;

                Dust dust = Dust.NewDustDirect(spawnArea.TopLeft(), spawnArea.Width, spawnArea.Height,
                    dustType, 0f, 0f, 100, Color.White);

                dust.position = points[pointIndex];
                dust.fadeIn = 0.3f;
                dust.scale = Main.rand.NextFloat(0.5f, 1.8f);
                dust.noGravity = false;

                Vector2 spinningPoint = points[pointIndex] - points[pointIndex - 1];
                dust.velocity *= 1f;
                dust.velocity += spinningPoint.RotatedBy(owner.direction * ((float)Math.PI / 2f));
                dust.velocity *= 1f;

                foreach (var p in points)
                {
                    Lighting.AddLight(p, 0.05f, 0.3f, 0.3f);
                }
            }


        }

    }
}