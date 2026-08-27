using Terraria;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Items.Weapons;

namespace CalamitySimpleWhipAddon.Content.Systems
{
    public class CalamityEnchantmentRegisterSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            if (!ModLoader.TryGetMod("CalamityMod", out Mod calamity))
                return;

            calamity.Call(
                "MakeItemExhumable",
                ModContent.ItemType<MassofWailing>(),
                ModContent.ItemType<ChorusofExecration>()
            );
        }
    }
}