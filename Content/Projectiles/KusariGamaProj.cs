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


    public class KusariGamaProj : ModProjectile
    {

        private float SwingProgress;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 40;
            Projectile.WhipSettings.RangeMultiplier = 1.5f;
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
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuff13>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.8f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);

            SimpleWhipDrawer10.DrawSegments(Projectile, points, Timer);

            return false;
        }

        public override void AI()
        {
            base.AI();

            Player owner = Main.player[Projectile.owner];

            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            SwingProgress = Projectile.ai[0] / swingTime;

            List<Vector2> whipPoints = Projectile.WhipPointsForCollision;

            if (Utils.GetLerpValue(0.1f, 0.7f, SwingProgress, true) *
                Utils.GetLerpValue(0.9f, 0.7f, SwingProgress, true) > 0.5f &&
                !Main.rand.NextBool(3))
            {
                Vector2 tip = whipPoints[^1];

                // ★ 再宣言しない
                int pointIndex = Main.rand.Next(whipPoints.Count - 10, whipPoints.Count);
                Rectangle spawnArea = Utils.CenteredRectangle(
                    whipPoints[pointIndex],
                    new Vector2(30f, 30f)
                );

                for (int i = 0; i < 1; i++)
                {
                    int dustIndex = Dust.NewDust(
                        tip - new Vector2(4, 4),
                        8, 8,
                        54,
                        Main.rand.NextFloat(-3f, 3f),
                        Main.rand.NextFloat(-3f, 3f),
                        50,
                        Color.Black,
                        Main.rand.NextFloat(0.6f, 1.2f)
                    );
                    Main.dust[dustIndex].noGravity = true;
                }


            }



        }

    }
}