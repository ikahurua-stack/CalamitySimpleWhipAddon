using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent;
using System.Collections.Generic;
using System;
using CalamityMod.NPCs;
using CalamitySimpleWhipAddon;
using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Systems;
using CalamitySimpleWhipAddon.Content.Items.Weapons;
using CalamitySimpleWhipAddon.Content.Projectiles;

namespace CalamitySimpleWhipAddon.Content.Systems
{
    public class LightningSystem : ModSystem
    {
        public class LightningSegment
        {
            public List<Vector2> ThickPoints = new();
            public List<Vector2> ThinPoints = new();
            public List<Vector2> ThinPoints2 = new();

            public Vector2 Direction;

            public int Timer;
        }

        public static List<LightningSegment> ActiveLightnings = new();

        public override void PostUpdateEverything()
        {
            for (int i = ActiveLightnings.Count - 1; i >= 0; i--)
            {
                ActiveLightnings[i].Timer--;

                if (ActiveLightnings[i].Timer <= 0)
                    ActiveLightnings.RemoveAt(i);
            }
        }

        public static void AddLightning(Vector2 start, Vector2 end)
        {
            var lightning = new LightningSegment();

            GenerateLightningSegment3(start, end, lightning.ThickPoints);
            GenerateLightningSegment2(start, end, lightning.ThinPoints2);
            GenerateLightningSegment(start, end, lightning.ThinPoints);

            lightning.Direction =
                (end - start).SafeNormalize(Vector2.UnitX);

            foreach (Vector2 point in lightning.ThinPoints)
            {
                if (Main.rand.NextBool(4))
                {
                    int dust = Dust.NewDust(
                        point - new Vector2(2),
                        4,
                        4,
                        DustID.PortalBoltTrail);

                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].color = Color.Pink;

                    Main.dust[dust].scale =
                        Main.rand.NextFloat(
                            0.3f,
                            0.5f);

                    Vector2 outward = -lightning.Direction;

                    Vector2 side =
                        outward.RotatedBy(MathHelper.PiOver2);

                    Main.dust[dust].velocity =
                        outward * Main.rand.NextFloat(3f, 5f)
                        + side * Main.rand.NextFloat(-2f, 2f);

                    Main.dust[dust].fadeIn = 0.5f;
                }
            }



            lightning.Timer = 5;

            ActiveLightnings.Add(lightning);
        }

        public static void GenerateLightningSegment(
            Vector2 beamStart,
            Vector2 beamEnd,
            List<Vector2> points)
        {
            points.Clear();

            points.Add(beamStart);

            Vector2 dir = beamEnd - beamStart;

            Vector2 normal =
                dir.SafeNormalize(Vector2.UnitX)
                .RotatedBy(MathHelper.PiOver2);

            float distance = Vector2.Distance(
                beamStart,
                beamEnd);

            int segments =
                (int)(distance / 28f)
                + Main.rand.Next(-2, 3);

            segments = Utils.Clamp(
                segments,
                2,
                40);

            for (int i = 1; i < segments; i++)
            {
                float t = i / (float)segments;

                Vector2 pos =
                    Vector2.Lerp(
                        beamStart,
                        beamEnd,
                        t);

                float maxOffset =
                    distance * 0.08f;

                maxOffset =
                    MathHelper.Clamp(
                        maxOffset,
                        25f,
                        50f);

                float offset =
                    maxOffset *
                    (1f - Math.Abs(t - 0.5f));

                pos += normal *
                    Main.rand.NextFloat(
                        -offset,
                        offset);

                points.Add(pos);
            }

            points.Add(beamEnd);
        }

        public static void GenerateLightningSegment2(
            Vector2 beamStart,
            Vector2 beamEnd,
            List<Vector2> points)
        {
            points.Clear();

            points.Add(beamStart);

            Vector2 dir = beamEnd - beamStart;

            Vector2 normal =
                dir.SafeNormalize(Vector2.UnitX)
                .RotatedBy(MathHelper.PiOver2);

            float distance =
                Vector2.Distance(
                    beamStart,
                    beamEnd);

            int segments =
                (int)(distance / 28f)
                + Main.rand.Next(-2, 3);

            segments = Utils.Clamp(
                segments,
                1,
                50);

            for (int i = 1; i < segments; i++)
            {
                float t = i / (float)segments;

                Vector2 pos =
                    Vector2.Lerp(
                        beamStart,
                        beamEnd,
                        t);

                float maxOffset =
                    distance * 0.08f;

                maxOffset =
                    MathHelper.Clamp(
                        maxOffset,
                        20f,
                        40f);

                float offset =
                    maxOffset *
                    (1f - Math.Abs(t - 0.5f));

                pos += normal *
                    Main.rand.NextFloat(
                        -offset,
                        offset);

                points.Add(pos);
            }

            points.Add(beamEnd);
        }

        public static void GenerateLightningSegment3(
            Vector2 beamStart,
            Vector2 beamEnd,
            List<Vector2> points)
        {
            points.Clear();

            points.Add(beamStart);

            Vector2 dir = beamEnd - beamStart;

            Vector2 normal =
                dir.SafeNormalize(Vector2.UnitX)
                .RotatedBy(MathHelper.PiOver2);

            float distance =
                Vector2.Distance(
                    beamStart,
                    beamEnd);

            int segments =
                (int)(distance / 45f)
                + Main.rand.Next(-1, 2);

            segments = Utils.Clamp(
                segments,
                1,
                25);

            for (int i = 1; i < segments; i++)
            {
                float t = i / (float)segments;

                Vector2 pos =
                    Vector2.Lerp(
                        beamStart,
                        beamEnd,
                        t);

                float maxOffset =
                    distance * 0.08f;

                maxOffset =
                    MathHelper.Clamp(
                        maxOffset,
                        3f,
                        14f);

                float offset =
                    maxOffset *
                    (1f - Math.Abs(t - 0.5f));

                pos += normal *
                    Main.rand.NextFloat(
                        -offset,
                        offset);

                points.Add(pos);
            }

            points.Add(beamEnd);
        }

        public override void PostDrawTiles()
        {
            if (ActiveLightnings == null || ActiveLightnings.Count == 0)
                return;

            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                Main.GameViewMatrix.TransformationMatrix);

            DrawLightnings();

            Main.spriteBatch.End();
        }

        

        private static void DrawSegment(
            Texture2D pixel,
            Vector2 start,
            Vector2 end,
            Color color,
            float width)
        {
            Vector2 diff = end - start;

            Main.EntitySpriteDraw(
                pixel,
                start - Main.screenPosition,
                new Rectangle(0, 0, 1, 1),
                color,
                diff.ToRotation(),
                new Vector2(0f, 0.5f),
                new Vector2(diff.Length(), width),
                SpriteEffects.None,
                0);
        }

        public static void DrawLightnings()
        {
            Texture2D pixel = TextureAssets.MagicPixel.Value;

            foreach (var lightning in ActiveLightnings)
            {
                float alpha = lightning.Timer / 5f;

                float pulse =
                    0.9f +
                    0.25f *
                    MathF.Sin(Main.GlobalTimeWrappedHourly * 80f);

                float pulse2 =
                    0.85f +
                    0.35f *
                    MathF.Sin(Main.GlobalTimeWrappedHourly * 110f + 1.7f);

                var thick = lightning.ThickPoints;
                var thin1 = lightning.ThinPoints;
                var thin2 = lightning.ThinPoints2;

                float thickScale =
                    (alpha > 0.5f ? 1f : alpha * 2f) * pulse;

                float thinScale =
                    (alpha > 0.5f ? 1f : alpha * 2f) * pulse2;

                // 太い電撃
                for (int i = 0; i < thick.Count - 1; i++)
                {
                    Vector2 start = thick[i];
                    Vector2 end = thick[i + 1];

                    DrawSegment(pixel, start, end,
                        Color.Red * 0.15f * alpha,
                        20f * thickScale);

                    DrawSegment(pixel, start, end,
                        Color.Red * 0.2f * alpha,
                        16f * thickScale);

                    DrawSegment(pixel, start, end,
                        Color.DeepPink * 0.6f * alpha,
                        9f * thickScale);
                }

                // 細い1
                for (int i = 0; i < thin1.Count - 1; i++)
                {
                    Vector2 start = thin1[i];
                    Vector2 end = thin1[i + 1];

                    DrawSegment(pixel, start, end,
                        Color.Red * 0.2f * alpha,
                        8f * thinScale);

                    DrawSegment(pixel, start, end,
                        Color.DeepPink * 0.6f * alpha,
                        4f * thinScale);
                }

                // 細い2
                for (int i = 0; i < thin2.Count - 1; i++)
                {
                    Vector2 start = thin2[i];
                    Vector2 end = thin2[i + 1];

                    DrawSegment(pixel, start, end,
                        Color.Red * 0.2f * alpha,
                        8f * thinScale);

                    DrawSegment(pixel, start, end,
                        Color.DeepPink * 0.6f * alpha,
                        4f * thinScale);
                }

                // 白いコア
                for (int i = 0; i < thick.Count - 1; i++)
                {
                    Vector2 start = thick[i];
                    Vector2 end = thick[i + 1];

                    float whitePulse =
                        0.9f +
                        0.15f *
                        MathF.Sin(Main.GlobalTimeWrappedHourly * 150f);

                    DrawSegment(pixel, start, end,
                        Color.White * alpha,
                        6f * thickScale * whitePulse);
                }

                for (int i = 0; i < thin1.Count - 1; i++)
                {
                    Vector2 start = thin1[i];
                    Vector2 end = thin1[i + 1];

                    float whitePulse =
                        0.85f +
                        0.2f *
                        MathF.Sin(Main.GlobalTimeWrappedHourly * 180f + 0.8f);

                    DrawSegment(pixel, start, end,
                        Color.White * alpha,
                        2f * thinScale * whitePulse);
                }

                for (int i = 0; i < thin2.Count - 1; i++)
                {
                    Vector2 start = thin2[i];
                    Vector2 end = thin2[i + 1];

                    float whitePulse =
                        0.85f +
                        0.2f *
                        MathF.Sin(Main.GlobalTimeWrappedHourly * 180f + 0.8f);

                    DrawSegment(pixel, start, end,
                        Color.White * alpha,
                        2f * thinScale * whitePulse);
                }
            }
        }
    }
}