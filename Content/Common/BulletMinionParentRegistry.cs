using Terraria.ModLoader;
using System.Collections.Generic;

namespace CalamitySimpleWhipAddon.Content.Common
{
    public static class BulletMinionParentRegistry
    {
        private static readonly HashSet<int> Parents = new();

        public static bool Contains(int type)
            => Parents.Contains(type);

        public static void TryAdd(string modName, string projName)
        {
            if (!ModLoader.TryGetMod(modName, out Mod mod))
                return;

            if (mod.TryFind(projName, out ModProjectile mp))
                Parents.Add(mp.Type);
        }
    }
}
