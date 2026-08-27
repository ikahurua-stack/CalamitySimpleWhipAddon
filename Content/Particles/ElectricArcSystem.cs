using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CalamitySimpleWhipAddon.Content.Particles
{
    public static class ElectricArcSystem
    {
        public static readonly List<ElectricArcParticle> Arcs = new();

        public static void Spawn(
            Vector2 start,
            Vector2 end,
            float size,
            Color color,
            Color color2,
            Color color3)
        {
            Arcs.Add(
                new ElectricArcParticle(
                    start,
                    end,
                    size,
                    color,
                    color2,
                    color3));
        }

        public static void UpdateAndDraw(SpriteBatch spriteBatch)
        {
            for (int i = Arcs.Count - 1; i >= 0; i--)
            {
                if (Arcs[i].Update())
                {
                    Arcs.RemoveAt(i);
                    continue;
                }

                Arcs[i].Draw(spriteBatch);
            }
        }
    }
}