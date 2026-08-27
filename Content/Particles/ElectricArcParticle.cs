using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using System.Collections.Generic;
using System;

namespace CalamitySimpleWhipAddon.Content.Particles
{
    public class ElectricArcParticle
    {
        public readonly List<Vector2> Points = new();

        public int TimeLeft = 12;

        public float Width = 2f;

        public float SizeFactor;

        public Color OuterColor;
        public Color MiddleColor;
        public Color InnerColor;

        public int Age;

        public Vector2 Start;
        public Vector2 End;

        public readonly List<Vector2> BasePoints = new();

        public readonly List<float> Phase = new();

        public readonly List<float> Amplitude = new();

        public readonly List<float> Speed = new();

        public ElectricArcParticle(
            Vector2 start,
            Vector2 end,
            float sizeFactor,
            Color color,
            Color color2,
            Color color3
            )
        {
            Start = start;
            End = end;

            SizeFactor = sizeFactor;

            OuterColor = color;
            MiddleColor = color2;
            InnerColor = color3;

            TimeLeft = 12;

            Width = MathHelper.Lerp(2f, 5f, sizeFactor);

            Generate(Start, End);
        }

        void Generate(Vector2 start, Vector2 end)
        {
            Points.Clear();
            BasePoints.Clear();
            Phase.Clear();
            Amplitude.Clear();
            Speed.Clear();

            int segments = 8;

            float jitter = MathHelper.Lerp(5f, 18f, SizeFactor);

            Vector2 normal =
                (end - start).SafeNormalize(Vector2.UnitX);

            normal = new Vector2(-normal.Y, normal.X);

            for (int i = 0; i <= segments; i++)
            {
                float t = i / (float)segments;

                Vector2 p = Vector2.Lerp(start, end, t);

                BasePoints.Add(p);

                Points.Add(p);

                Phase.Add(Main.rand.NextFloat(MathHelper.TwoPi));

                Speed.Add(Main.rand.NextFloat(18f, 45f));

                if (i == 0 || i == segments)
                    Amplitude.Add(0f);
                else
                    Amplitude.Add(
                        Main.rand.NextFloat(
                            jitter * 0.5f,
                            jitter));
            }
        }

        public bool Update()
        {
            Age++;
            TimeLeft--;

            Vector2 start = BasePoints[0];
            Vector2 end = BasePoints[^1];

            Vector2 normal =
                (end - start).SafeNormalize(Vector2.UnitX);

            normal = new Vector2(-normal.Y, normal.X);

            for (int i = 0; i < Points.Count; i++)
            {
                float offset =
                    (float)Math.Sin(
                        Main.GlobalTimeWrappedHourly * Speed[i]
                        + Phase[i]);

                Points[i] =
                    BasePoints[i] +
                    normal *
                    offset *
                    Amplitude[i];
            }

            return TimeLeft <= 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D tex = TextureAssets.MagicPixel.Value;

            float alpha = TimeLeft / 12f;

            for (int i = 0; i < Points.Count - 1; i++)
            {
                float progress =
                    Age * 1.3f;

                if (i > progress)
                    break;

                Vector2 start = Points[i];
                Vector2 end = Points[i + 1];

                Vector2 diff = end - start;

                float outerWidth = Width * (1.8f + SizeFactor);
                float middleWidth = Width * (1f + SizeFactor * 0.5f);
                float innerWidth = Width * 0.6f;

                Rectangle rect = new Rectangle(0, 0, 1, 1);

                float localAlpha =
                    MathHelper.Clamp(progress - i, 0f, 1f);

                localAlpha *= alpha;

                // 外側
                spriteBatch.Draw(
                    tex,
                    start - Main.screenPosition,
                    rect,
                    OuterColor * localAlpha * 0.25f,
                    diff.ToRotation(),
                    new Vector2(0f, 0.5f),
                    new Vector2(diff.Length(), outerWidth),
                    SpriteEffects.None,
                    0);

                // 中
                spriteBatch.Draw(
                    tex,
                    start - Main.screenPosition,
                    rect,
                    MiddleColor * localAlpha * 0.6f,
                    diff.ToRotation(),
                    new Vector2(0f, 0.5f),
                    new Vector2(diff.Length(), middleWidth),
                    SpriteEffects.None,
                    0);

                // 芯
                spriteBatch.Draw(
                    tex,
                    start - Main.screenPosition,
                    rect,
                    InnerColor * localAlpha,
                    diff.ToRotation(),
                    new Vector2(0f, 0.5f),
                    new Vector2(diff.Length(), innerWidth),
                    SpriteEffects.None,
                    0);
            }
        }
    }
}