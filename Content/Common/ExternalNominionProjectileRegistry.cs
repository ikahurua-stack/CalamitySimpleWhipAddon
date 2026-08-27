using System.Collections.Generic;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Common
{
    public class ExternalNominionProjectileRegistry : ModSystem
    {
        private static readonly HashSet<int> RegisteredNominionProjectiles = new();

        public static IEnumerable<int> GetRegisteredTypes() => RegisteredNominionProjectiles;

        public override void PostSetupContent()
        {
            RegisterSOTSProjectiles();
        }

        public static bool Contains(int projectileType)
        {
            return RegisteredNominionProjectiles.Contains(projectileType);
        }

        
        private static void RegisterSOTSProjectiles()
        {
            if (!ModLoader.TryGetMod("SOTS", out Mod sots))
                return;

            TryAdd(sots, "IlluminationSparkle");
            TryAdd(sots, "BloomingHookMinion");
            TryAdd(sots, "BrassBall");
            TryAdd(sots, "WoeBall");
            TryAdd(sots, "BundleSnakeMinion");

        }

        

        private static void TryAdd(Mod mod, string projectileName)
        {
            if (mod.TryFind(projectileName, out ModProjectile projectile))
                RegisteredNominionProjectiles.Add(projectile.Type);
        }

    }
}
