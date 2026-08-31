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


    public class LoadoutProj : ModProjectile
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
            Projectile.WhipSettings.RangeMultiplier = 1.7f;
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
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuff7D07C2>(), 240);
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuffEx4>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.8f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);

            SimpleWhipDrawerLo.DrawSegments(Projectile, points, Timer);

            return false;
        }

        public override void AI()
        {
            base.AI();

            Color darkVermilion = new Color(240, 40, 30);

            Player owner = Main.player[Projectile.owner];

            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            SwingProgress = Projectile.ai[0] / swingTime;

            List<Vector2> whipPoints = Projectile.WhipPointsForCollision;

            Vector2 tip = whipPoints[^1];

            if (Utils.GetLerpValue(0.1f, 0.7f, SwingProgress, true) *
                Utils.GetLerpValue(0.9f, 0.7f, SwingProgress, true) > 0.5f &&
                !Main.rand.NextBool(3))
            {

                // ★ 再宣言しない
                int pointIndex = Main.rand.Next(whipPoints.Count - 10, whipPoints.Count);
                Rectangle spawnArea = Utils.CenteredRectangle(
                    whipPoints[pointIndex],
                    new Vector2(30f, 30f)
                );


                for (int i = 0; i < whipPoints.Count - 1; i++)
                {
                    Vector2 pos = whipPoints[i];

                    // 小さな光
                    Lighting.AddLight(pos, darkVermilion.ToVector3() * 0.6f);

                }


                // 強い光
                Lighting.AddLight(tip, darkVermilion.ToVector3() * 1f);

            }



        }

    }
}