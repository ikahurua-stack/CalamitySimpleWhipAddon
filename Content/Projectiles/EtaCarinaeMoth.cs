using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using System;
using System.IO;
using CalamityMod.Graphics.Primitives;
using static CalamityMod.CalamityUtils;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class EtaCarinaeMoth : ModProjectile
    {
        private bool randomInitialized;
        private int Seed => (int)Projectile.ai[2];

        float depth;
        float depthFactor;

        bool visible = true;

        float orbitAngle;
        float orbitSpeed;
        float tiltAngle;
        float radiusX;
        float radiusY;

        float wobbleStrength;
        float wobbleSpeed;
        float wobbleOffset;

        float brightness = 1f;

        private Color mothColor;

        float spinSpeed;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 25;
            ProjectileID.Sets.TrailingMode[Type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;

            Projectile.hide = true;

            Projectile.scale *= Main.rand.NextFloat(0.9f, 1.4f);
        }

        // ===== Milkywayトレイル =====

        private float TrailWidth(float t, Vector2 pos)
        {
            float width = MathF.Pow(1f - t, 1.8f);
            return Projectile.scale * 10f * width;
        }

        private Color TrailColor(float t, Vector2 pos)
        {
            float alpha = (1f - Projectile.alpha / 255f);

            Color c = Color.Lerp(mothColor, Color.Cyan, t * 0.8f + depthFactor * 0.4f);

            c *= brightness;
            c *= depthFactor;
            c *= alpha;
            c *= (1f - t);

            return c;
        }

        private Vector2 TrailOffset(float t, Vector2 pos)
        {
            return Vector2.Zero;
        }

        private void DrawMilkywayTrail()
        {
            List<Vector2> points = new();

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;

                points.Add(Projectile.oldPos[i] + Projectile.Size / 2);
            }

            if (points.Count < 2)
                return;

            var shader = GameShaders.Misc["CalamityMod:SideStreakTrail"];
            shader.SetShaderTexture(TextureAssets.Extra[197]);

            PrimitiveRenderer.RenderTrail(
                points,
                new PrimitiveSettings(
                    TrailWidth,
                    TrailColor,
                    TrailOffset,
                    shader: shader
                ),
                20
            );
        }


        private void InitializeRandomValues()
        {
            if (randomInitialized)
                return;

            randomInitialized = true;

            UnifiedRandom orbitRand = new UnifiedRandom(Seed ^ 13579);
            UnifiedRandom radiusRand = new UnifiedRandom(Seed ^ 24680);
            UnifiedRandom colorRand = new UnifiedRandom(Seed ^ 31415);
            UnifiedRandom spinRand = new UnifiedRandom(Seed ^ 92653);
            UnifiedRandom miscRand = new UnifiedRandom(Seed ^ 58979);

            orbitAngle = orbitRand.NextFloat(MathHelper.TwoPi);
            orbitSpeed = orbitRand.NextFloat(0.05f, 0.1f);
            tiltAngle = orbitRand.NextFloat(-MathHelper.Pi, MathHelper.Pi);

            spinSpeed = spinRand.NextFloat(-0.20f, 0.20f);

            float t = colorRand.NextFloat();

            Color cyan = new Color(30, 120, 255);
            Color white = new Color(50, 175, 255);

            mothColor = Color.Lerp(cyan, white, t);

            Projectile.localAI[1] = miscRand.NextBool() ? 1f : 2f;
        }

        public override void AI()
        {
            if (Projectile.ai[0] < 0 || Projectile.ai[0] >= Main.maxNPCs)
            {
                Projectile.Kill();
                return;
            }

            NPC target = Main.npc[(int)Projectile.ai[0]];

            if (!target.active)
            {
                Projectile.Kill();
                return;
            }


            visible = true;

            Projectile.rotation += 0.0001f;

            InitializeRandomValues();

            if (radiusX == 0f)
            {
                UnifiedRandom radiusRand = new UnifiedRandom(Seed ^ 24680);

                float size = (target.width + target.height) * 0.5f;
                float sizeFactor2 = MathF.Sqrt(size) * 10f;
                sizeFactor2 = MathHelper.Clamp(sizeFactor2, 40f, 120f);

                radiusX = sizeFactor2 * radiusRand.NextFloat(0.9f, 1.3f);
                radiusY = sizeFactor2 * radiusRand.NextFloat(0.25f, 0.45f);

                wobbleStrength = radiusRand.NextFloat(4f, 18f);
                wobbleSpeed = radiusRand.NextFloat(0.8f, 1.8f);
                wobbleOffset = radiusRand.NextFloat(MathHelper.TwoPi);
            }

            if (Projectile.localAI[1] == 0f)
            {
                if (Main.rand.NextBool())
                    Projectile.localAI[1] = 1f;
                else
                    Projectile.localAI[1] = 2f;
            }
            // ===== サイズ調整 =====
            float baseScale = 0.45f;

            float sizeFactor = (target.width + target.height) * 0.5f;

            // 基準サイズ（ゾンビくらい）
            float referenceSize = 50f;

            float scaleMultiplier = sizeFactor / referenceSize;

            // 最小最大制限
            scaleMultiplier = MathHelper.Clamp(scaleMultiplier, 0.9f, 2.0f);


            if (!target.active)
            {
                Projectile.Kill();
                return;
            }

            Vector2 offset = new Vector2(Projectile.ai[1], Projectile.localAI[0]);

            orbitAngle += orbitSpeed;

            if (!visible)
            {
                Projectile.Center = target.Center;
                return;
            }

            float x = MathF.Cos(orbitAngle) * radiusX;
            float y = MathF.Sin(orbitAngle) * radiusY;

            depth = MathF.Sin(orbitAngle);

            float cosTilt = MathF.Cos(tiltAngle);
            float sinTilt = MathF.Sin(tiltAngle);

            float rotX = x * cosTilt - y * sinTilt;
            float rotY = x * sinTilt + y * cosTilt;

            Vector2 orbitOffset = new Vector2(rotX, rotY);

            Projectile.Center = target.Center + orbitOffset;

            depthFactor = (depth + 1f) * 0.5f;

            float depthScale = 0.65f + depthFactor * 0.65f;
            Projectile.scale = baseScale * scaleMultiplier * depthScale;

            brightness = 0.4f + depthFactor * 0.9f;

            

            int fadeStart = 60;

            if (Projectile.timeLeft < fadeStart)
            {
                float fade = 1f - Projectile.timeLeft / (float)fadeStart;
                Projectile.alpha = (int)MathHelper.Lerp(Projectile.alpha, 255f, fade);
            }

            Projectile.rotation += spinSpeed;

            if (visible)
            {
                Vector3 lightColor = mothColor.ToVector3();
                Lighting.AddLight(Projectile.Center, lightColor * 0.6f);
            }

            if (visible && Main.rand.NextBool(6))
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.GemSapphire,
                    Main.rand.NextVector2Circular(0.4f, 0.4f)
                );

                d.scale = Main.rand.NextFloat(0.3f, 0.6f);
                d.noGravity = true;
                d.color = mothColor;
                d.velocity *= 0.3f;
            }
        }


        public override bool PreDraw(ref Color lightColor)
        {
            if (!visible)
                return false;

            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Rectangle frame = tex.Bounds;

            Color drawColor = mothColor * brightness;
            drawColor *= (1f - Projectile.alpha / 255f);
            drawColor *= 3.5f;


            if (visible)
            {
                Main.spriteBatch.EnterShaderRegion();
                DrawMilkywayTrail();
                Main.spriteBatch.ExitShaderRegion();
            }

            Main.spriteBatch.End();

            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.Additive,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix
            );

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                frame,
                drawColor,
                Projectile.rotation,
                frame.Size() / 2,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            Main.spriteBatch.End();

            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix
            );


            return false;
        }

        public override void DrawBehind(
            int index,
            List<int> behindNPCsAndTiles,
            List<int> behindNPCs,
            List<int> behindProjectiles,
            List<int> overPlayers,
            List<int> overWiresUI)
        {
            if (depth < 0f)
            {
                // 奥にいる時：NPCの背後レイヤーに追加
                behindNPCs.Add(index);
            }
            else
            {
                behindProjectiles.Add(index);
            }
        }
    }
}