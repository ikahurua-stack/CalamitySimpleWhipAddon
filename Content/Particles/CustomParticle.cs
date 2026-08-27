using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace CalamitySimpleWhipAddon.Content.Particles
{
    public class CustomParticle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Scale;
        public float MaxScale;
        public float LifeTime;
        public float Age;
        public Texture2D Texture;
        public Color Color;

        // 回転用プロパティ
        public float Rotation;
        public float AngularVelocity;

        // ★ 高速化のためのキャッシュ領域
        private Vector2 _origin;
        private float _timeRatio; // 毎フレームの進行度 (0.0 〜 1.0) を保持

        public CustomParticle(Vector2 pos, Vector2 vel, float maxScale, float lifeTime, Texture2D tex, Color color)
        {
            Position = pos;
            Velocity = vel;
            Scale = 0f;
            MaxScale = maxScale;
            LifeTime = lifeTime;
            Texture = tex;
            Color = color;
            Age = 0f;
            _timeRatio = 0f;

            // ★ 中心点（Origin）を生成時に1度だけ計算して使い回す（毎フレームのメモリ消費を完全にゼロにする）
            if (Texture != null)
            {
                _origin = new Vector2(Texture.Width * 0.5f, Texture.Height * 0.5f);
            }

            // ★ 安全な乱数生成（もし可能なら呼び出し元から Main.rand.NextFloat を引数で渡すのがベストですが、現状維持でもエフェクトなら動作自体はします）
            Rotation = Main.rand.NextFloat(0f, MathHelper.TwoPi);
            AngularVelocity = Main.rand.NextFloat(-0.1f, 0.1f);
        }

        public bool Update()
        {
            Age++;
            Position += Velocity;
            Rotation += AngularVelocity;

            // ★ 割り算の回数を1回にまとめ、値をキャッシュする
            _timeRatio = Age / LifeTime;

            // 線形補間（Lerp）
            Scale = MathHelper.Lerp(MaxScale, 0f, _timeRatio);

            return Age >= LifeTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture == null) return;

            // ★ キャッシュされた _timeRatio と _origin を使うことで、CPUとメモリの負荷を極限まで減らす
            spriteBatch.Draw(
                Texture,
                Position - Main.screenPosition,
                null,
                Color * (1f - _timeRatio),
                Rotation,
                _origin,
                Scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
