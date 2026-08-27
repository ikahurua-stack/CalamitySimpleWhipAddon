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


    public class ExistenceBondsProj : ModProjectile
    {

        private float SwingProgress;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 46;
            Projectile.WhipSettings.RangeMultiplier = 2.05f;
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
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuffEx7>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.8f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);

            SimpleWhipDrawer11.DrawSegments(Projectile, points, Timer);

            return false;
        }

        public override void AI()
        {
            base.AI();

            Color darkVermilion = new Color(240, 20, 240);

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

                    // 既存の光
                    Lighting.AddLight(pos, darkVermilion.ToVector3() * 0.6f);

                    // ★ ダスト追加
                    if (Main.rand.NextBool(6))
                    {
                        Vector2 dir = (whipPoints[i + 1] - whipPoints[i]).SafeNormalize(Vector2.Zero);

                        Dust dust = Dust.NewDustDirect(
                            pos,
                            0,
                            0,
                            DustID.Shadowflame,
                            0f,
                            0f,
                            150,
                            default,
                            1.5f
                        );

                        dust.velocity = dir.RotatedByRandom(0.4f) * Main.rand.NextFloat(1.5f, 3f);
                        dust.noGravity = true;
                    }
                }

                if (Main.rand.NextBool(2))
                {
                    Dust tipDust = Dust.NewDustDirect(
                        tip,
                        0,
                        0,
                        DustID.Shadowflame,
                        0f,
                        0f,
                        100,
                        default,
                        1.9f
                    );

                    tipDust.velocity =
                        (tip - whipPoints[^2]).SafeNormalize(Vector2.UnitY)
                        * Main.rand.NextFloat(3f, 5f);

                    tipDust.noGravity = true;
                }

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