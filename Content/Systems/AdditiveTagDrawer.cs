using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using System;
using ReLogic.Content; // ★追加

namespace CalamitySimpleWhipAddon.Content.Systems
{
    public static class AdditiveTagDrawer
    {
        // ★テクスチャへのリクエストを事前に保持して高速化
        private static Asset<Texture2D> milkywayTex;
        private static Asset<Texture2D> skybreakerTex;

        // ★外部から「今描画するものがあるか」を判定するためのプロパティ（前回のシステム側で使用）
        public static bool IsEmpty => tags.Count == 0 && skybreakers.Count == 0;

        private struct Skybreaker
        {
            public Vector2 Position;
            public float Rotation;
            public float Scale;
            public int Frame;
            public int MaxFrames;
        }

        private static readonly List<Skybreaker> skybreakers = new();

        public static void RegisterSkybreaker(Vector2 pos, float rot, float scale, int maxFrames)
        {
            // サーバー側なら登録を無視して無駄なメモリ消費を防ぐ
            if (Main.dedServ) return;

            skybreakers.Add(new Skybreaker
            {
                Position = pos,
                Rotation = rot,
                Scale = scale,
                Frame = 0,
                MaxFrames = maxFrames
            });
        }

        private struct TagDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public float Alpha;
        }

        private static readonly List<TagDrawData> tags = new();

        public static void Register(Vector2 pos, float rot, float alpha)
        {
            if (Main.dedServ) return;

            tags.Add(new TagDrawData
            {
                Position = pos,
                Rotation = rot,
                Alpha = alpha
            });
        }

        public static void Update()
        {
            // 【重要】描画が呼ばれない環境（サーバー等）でも、ここで必ずクリアしてメモリリークを防ぐ
            if (Main.dedServ)
            {
                tags.Clear();
                skybreakers.Clear();
                return;
            }

            for (int i = skybreakers.Count - 1; i >= 0; i--)
            {
                var s = skybreakers[i];
                s.Frame++;

                if (s.Frame >= s.MaxFrames)
                    skybreakers.RemoveAt(i);
                else
                    skybreakers[i] = s;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (IsEmpty) return;

            // ---- Milkyway ----
            if (tags.Count > 0)
            {
                // 初回だけ読み込む（超高速化）
                milkywayTex ??= ModContent.Request<Texture2D>("CalamitySimpleWhipAddon/Content/Textures/Particles/MilkywayFire");
                Texture2D tex = milkywayTex.Value;
                Vector2 origin = tex.Size() * 0.5f;

                foreach (var t in tags)
                {
                    spriteBatch.Draw(
                        tex,
                        t.Position - Main.screenPosition,
                        null,
                        Color.White * t.Alpha,
                        t.Rotation,
                        origin,
                        0.8f,
                        SpriteEffects.None,
                        0f
                    );
                }

                tags.Clear(); // 描画後にクリア
            }

            // ---- Skybreaker ----
            if (skybreakers.Count > 0)
            {
                skybreakerTex ??= ModContent.Request<Texture2D>("CalamitySimpleWhipAddon/Content/Textures/Particles/SkybreakerCoilFire");
                Texture2D tex = skybreakerTex.Value;

                const int textureFrames = 20;

                foreach (var s in skybreakers)
                {
                    int frame = Math.Min(
                        (int)(s.Frame * 0.95f * textureFrames / s.MaxFrames),
                        textureFrames - 1
                    );

                    float alpha = MathF.Pow(1.2f - s.Frame / (float)s.MaxFrames, 2.5f);

                    Rectangle rect = tex.Frame(1, textureFrames, 0, frame);
                    Vector2 origin = rect.Size() * 0.5f;

                    spriteBatch.Draw(
                        tex,
                        s.Position - Main.screenPosition,
                        rect,
                        Color.White * alpha,
                        s.Rotation,
                        origin,
                        s.Scale * 1.35f,
                        SpriteEffects.None,
                        0f
                    );
                }
            }
        }
    }
}
