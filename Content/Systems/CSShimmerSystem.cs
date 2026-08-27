using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Items.Weapons;

namespace CalamitySimpleWhipAddon.Content.Systems
{
    public class CSShimmerSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            ItemID.Sets.ShimmerTransformToItem[
                ModContent.ItemType<ChorusofExecration>()
            ] = ModContent.ItemType<MassofWailing>();
        }
    }
}