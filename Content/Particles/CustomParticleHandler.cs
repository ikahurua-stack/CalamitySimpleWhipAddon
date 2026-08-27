using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;

namespace CalamitySimpleWhipAddon.Content.Particles
{
    public static class CustomParticleHandler
    {
        private static readonly List<CustomParticle> particles = new();

        // ★空かどうかを判定するプロパティを追加
        public static bool IsEmpty => particles.Count == 0;

        public static void Spawn(CustomParticle particle)
        {
            if (Main.dedServ) return; // サーバーなら生成しない

            if (particle != null)
                particles.Add(particle);
        }

        public static void UpdateAll()
        {
            if (Main.dedServ)
            {
                particles.Clear();
                return;
            }

            for (int i = particles.Count - 1; i >= 0; i--)
            {
                if (particles[i].Update())
                    particles.RemoveAt(i);
            }
        }

        public static void DrawAll(SpriteBatch spriteBatch)
        {
            if (IsEmpty) return;

            foreach (var p in particles)
            {
                p.Draw(spriteBatch);
            }
        }

        public static void ClearAll()
        {
            particles.Clear();
        }
    }
}
