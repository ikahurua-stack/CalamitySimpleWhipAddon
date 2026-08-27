using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace CalamitySimpleWhipAddon.Content.Rendering
{
    public static class WhipProjectileSync
    {
        private static readonly Dictionary<int, Vector2> Positions = new();

        public static void Register(Projectile p)
        {
            Positions[p.whoAmI] = p.Center;
        }

        public static bool TryGet(int id, out Vector2 pos)
        {
            return Positions.TryGetValue(id, out pos);
        }

        public static void ClearDead()
        {
            Positions.Clear();
        }

        public static IEnumerable<Vector2> AllPositions => Positions.Values;
    }
}