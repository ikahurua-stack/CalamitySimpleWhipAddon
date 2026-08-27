using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common;

namespace CalamitySimpleWhipAddon.Content.Systems
{
    public class ExternalProjectileSetupSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            // Calamity Entropy
            BulletMinionParentRegistry.TryAdd("CalamityEntropy", "PlanetKiller");

            BulletMinionParentRegistry.TryAdd("Clamity", "PlanterrorStaffTentacle");
            BulletMinionParentRegistry.TryAdd("Clamity", "HellstoneShellfishStaffMinion");
            // 追加はここに1行ずつ
            // BulletMinionParentRegistry.TryAdd("ThoriumMod", "SomeMinion");
        }
    }
}
