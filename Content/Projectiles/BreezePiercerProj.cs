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
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using System;
using CalamityMod.Graphics.Primitives;
using static CalamityMod.CalamityUtils;


namespace CalamitySimpleWhipAddon.Content.Projectiles
{


    public class BreezePiercerProj : ModProjectile
    {
        private readonly List<Vector2> tipHistory = new();
        private const int MaxTrailPoints = 20;
        private float SwingProgress;

        private float WhipTrailWidth(float t, Vector2 position)
        {
            // 先端だけ太く、根元はかなり細い
            float tipBias = MathF.Pow(t, 1.5f); // ← ここが肝
            return Projectile.scale * MathHelper.Lerp(1f, 20f, tipBias);
        }

        private static readonly Color[] LuminiteColors =
        {
            new Color(200, 200, 200),
            new Color(255, 255, 255)
        };

        private Color WhipTrailBloomColor(float t, Vector2 position)
        {
            Color baseColor = GetLuminiteGradient(t, position);

            return baseColor
                * Projectile.Opacity
                * 0.5f; // ← 薄く
        }

        private float WhipTrailBloomWidth(float t, Vector2 position)
        {
            return WhipTrailWidth(t, position) * 2f;
        }

        private Color GetLuminiteGradient(float t, Vector2 position)
        {
            t = MathHelper.Clamp(t, 0.1f, 1f);

            float scaled = t * (LuminiteColors.Length - 1);
            int index = (int)scaled;
            int nextIndex = Math.Min(index + 1, LuminiteColors.Length - 1);

            float lerp = scaled - index;
            return Color.Lerp(LuminiteColors[index], LuminiteColors[nextIndex], lerp);
        }

        private Color WhipTrailColor(float t, Vector2 position)
        {
            float historyFactor = MathHelper.Clamp(
                tipHistory.Count / (float)MaxTrailPoints,
                0f,
                1f
            );

            float tipEmphasis = MathHelper.Lerp(0.8f, 1f, t);

            // ★ 振り終わりフェード
            float endFade = Utils.GetLerpValue(
                0.50f, // フェード開始
                1.0f,  // 完全消失
                SwingProgress,
                true
            );
            endFade = 1f - endFade; // 終盤ほど 0 に近づく

            Color baseColor = GetLuminiteGradient(t, position);

            return baseColor
                * Projectile.Opacity
                * historyFactor
                * tipEmphasis
                * endFade;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();

            Projectile.WhipSettings.Segments = 15;
            Projectile.WhipSettings.RangeMultiplier = 1.15f;
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
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuff0D09C>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.45f);
        }

        
        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> points = new();
            Projectile.FillWhipControlPoints(Projectile, points);

            if (points.Count < 2)
                return false;

            Vector2 tip = points[^1];
            Vector2 prev = points[^2];

            // ムチ先端方向
            Vector2 whipDir = tip - prev;
            if (whipDir.LengthSquared() <= 0f)
                return false;

            whipDir.Normalize();

            // 向きを固定（ここが重要）
            Vector2 slashDir = whipDir.RotatedBy(
                Main.player[Projectile.owner].direction * -MathHelper.PiOver2
            );

            // ---- ここからが「板状になる核心」 ----

            List<Vector2> trailPoints = new();

            int trailLength = 12;
            float spacing = 12f;

            for (int i = 0; i < trailLength; i++)
            {
                float t = i / (float)(trailLength - 1);

                Vector2 pos =
                    tip
                    + slashDir * spacing * i
                    + whipDir * 4f * t;

                trailPoints.Add(pos);
            }



            if (tipHistory.Count >= 2)
            {
                Main.spriteBatch.EnterShaderRegion();


                PrimitiveRenderer.RenderTrail(
                    tipHistory,
                    new PrimitiveSettings(
                        WhipTrailBloomWidth,
                        WhipTrailBloomColor,
                        (t, pos) => Vector2.Zero,
                        shader: GameShaders.Misc["CalamityMod:TrailStreak"]
                    ),
                    24
                );

                PrimitiveRenderer.RenderTrail(
                    tipHistory,
                    new PrimitiveSettings(
                        WhipTrailWidth,
                        WhipTrailColor,
                        (t, pos) => Vector2.Zero,
                        shader: GameShaders.Misc["CalamityMod:TrailStreak"]
                    ),
                    24
                );

                Main.spriteBatch.ExitShaderRegion();
            }


            RapierWhipDrawer1.DrawLine(points, (pos) => new Color(101, 149, 154));

            RapierWhipDrawer1.DrawSegments(Projectile, points, Timer);
            return false;
        }



        public override void AI()
        {

            base.AI();

            Player owner = Main.player[Projectile.owner];



            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            SwingProgress = Projectile.ai[0] / swingTime;

            List<Vector2> whipPoints = Projectile.WhipPointsForCollision;

            // ★ ここが最重要：振り抜きに入るまでトレイルを出さない
            bool canTrail = SwingProgress > 0.5f;

            if (!canTrail)
            {
                // 溜め中は履歴をリセット
                tipHistory.Clear();
            }
            else if (whipPoints.Count > 0)
            {
                tipHistory.Add(whipPoints[^1]);

                if (tipHistory.Count > MaxTrailPoints)
                    tipHistory.RemoveAt(0);
            }


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
                        DustID.Enchanted_Gold, 
                        Main.rand.NextFloat(-3f, 3f),
                        Main.rand.NextFloat(-3f, 3f),
                        50,
                        Color.White,
                        Main.rand.NextFloat(0.6f, 1.2f)
                    );
                    Main.dust[dustIndex].noGravity = true;
                }


                foreach (Vector2 p in whipPoints)
                {
                    Lighting.AddLight(p, 0.05f, 0.05f, 0.05f);
                }
            }
        }

    }

}


