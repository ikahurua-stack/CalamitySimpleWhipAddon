using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using System;
using CalamityMod.Graphics.Primitives;
using static CalamityMod.CalamityUtils;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class SkybreakerCoilProj : ModProjectile
    {
        // 雷サウンドの再生間隔（フレーム）
        private const int LightningSoundInterval = 6;

        // 内部タイマー
        private int lightningSoundTimer = 0;


        // ================= トレイル履歴 =================

        private const int MaxTrailPoints = 60;
        private readonly List<Vector2> tipHistory = new();

        // ================= 雷トレイル設定 =================

        private float LightningWidth(float t, Vector2 position)
        {
            float sharp = MathF.Pow(t, 1.35f);
            return Projectile.scale * MathHelper.Lerp(1.2f, 14f, sharp);
        }

        private float LightningBloomWidth(float t, Vector2 position)
            => LightningWidth(t, position) * 2.2f;

        private Color LightningColor(float t, Vector2 position)
        {
            float flicker =
                0.85f +
                MathF.Sin(Main.GlobalTimeWrappedHourly * 32f + t * 10f) * 0.25f;

            Color core = Color.Lerp(
                new Color(20, 150, 255),
                new Color(0, 110, 255),
                t
            );

            return core * flicker * Projectile.Opacity;
        }

        private Color LightningBloomColor(float t, Vector2 position)
        {
            return new Color(0, 170, 255) * 0.35f * Projectile.Opacity;
        }

        // ================= 基本設定 =================

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 66;
            Projectile.WhipSettings.RangeMultiplier = 3.85f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuffEx3>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.99f);
        }

        // ================= 描画 =================

        public override bool PreDraw(ref Color lightColor)
        {
            // ムチ本体
            List<Vector2> points = new();
            Projectile.FillWhipControlPoints(Projectile, points);
            SimpleWhipDrawer9.DrawSegments(Projectile, points, Projectile.ai[0]);

            if (tipHistory.Count < 2)
                return false;

            List<Vector2> lightningPoints = new();

            for (int i = 0; i < tipHistory.Count; i++)
            {
                float t = i / (float)(tipHistory.Count - 1);

                Vector2 p = tipHistory[i];

                // === 進行方向を取得 ===
                Vector2 dir;
                if (i == 0)
                    dir = tipHistory[1] - tipHistory[0];
                else
                    dir = tipHistory[i] - tipHistory[i - 1];

                if (dir.LengthSquared() < 0.0001f)
                    dir = Vector2.UnitX;

                dir.Normalize();

                // === 法線（進行方向に対して垂直）===
                Vector2 normal = dir.RotatedBy(MathHelper.PiOver2);

                float time = Main.GlobalTimeWrappedHourly;

                float noise =
                    MathF.Sin(time * 30f + t * 22f) +
                    MathF.Sin(time * 51f + t * 11f) * 0.6f;

                float strength = MathHelper.Lerp(3f, 18f, t);
                Vector2 offset = normal * noise * strength;

                if (Main.rand.NextBool(2))
                    offset *= Main.rand.NextFloat(1.2f, 3.2f);

                lightningPoints.Add(p + offset);
            }

            Main.spriteBatch.EnterShaderRegion();

            PrimitiveRenderer.RenderTrail(
                lightningPoints,
                new PrimitiveSettings(
                    LightningBloomWidth,
                    LightningBloomColor,
                    (t, pos) => Vector2.Zero,
                    shader: GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"]
                ),
                35
            );

            PrimitiveRenderer.RenderTrail(
                lightningPoints,
                new PrimitiveSettings(
                    LightningWidth,
                    LightningColor,
                    (t, pos) => Vector2.Zero,
                    shader: GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"]
                ),
                35
            );

            Main.spriteBatch.ExitShaderRegion();

            return false;
        }

        // ================= AI =================

        public override void AI()
        {
            base.AI();

            Player owner = Main.player[Projectile.owner];
            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            float swingProgress = Projectile.ai[0] / swingTime;

            List<Vector2> whipPoints = Projectile.WhipPointsForCollision;
            bool canTrail = swingProgress > 0.5f;

            // ================= 雷サウンド =================
            if (canTrail)
            {
                lightningSoundTimer++;

                if (lightningSoundTimer >= LightningSoundInterval)
                {
                    SoundEngine.PlaySound(
                        SoundID.DD2_LightningBugZap with { Pitch = Main.rand.NextFloat(0.8f, 1.2f) , Volume = 2.5f },
                        Projectile.Center
                    );

                    lightningSoundTimer = 0;
                }
            }
            else
            {
                // トレイルが止まったらリセット
                lightningSoundTimer = 0;
            }


            if (!canTrail)
            {
                tipHistory.Clear();
            }
            else if (whipPoints.Count > 0)
            {
                tipHistory.Add(whipPoints[^1]);

                if (whipPoints.Count > 4)
                    tipHistory.Add(whipPoints[^3]);

                if (tipHistory.Count > MaxTrailPoints)
                    tipHistory.RemoveRange(0, tipHistory.Count - MaxTrailPoints);
            }

            if (canTrail && Main.rand.NextBool(1))
            {
                Vector2 tip = whipPoints[^1];

                int d = Dust.NewDust(
                    tip - Vector2.One * 4,
                    8, 8,
                    DustID.Electric,
                    Main.rand.NextFloat(-5f, 5f),
                    Main.rand.NextFloat(-5f, 5f),
                    100,
                    Color.White,
                    Main.rand.NextFloat(1f, 1.5f)
                );

                Main.dust[d].noGravity = true;
            }
        }
    }
}
