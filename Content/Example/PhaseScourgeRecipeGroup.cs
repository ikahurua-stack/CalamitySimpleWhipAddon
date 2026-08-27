using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using CalamitySimpleWhipAddon.Content.Items.Weapons;

namespace CalamitySimpleWhipAddon
{
    public class PhaseScourgeGroup : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup phaseScourgeGroup = new RecipeGroup(
                () => Language.GetTextValue("Mods.CalamitySimpleWhipAddon.RecipeGroups.AnyPhaseScourge"),
                new int[]
                {
                    ModContent.ItemType<GreenPhaseScourge>(),
                    ModContent.ItemType<BluePhaseScourge>(),
                    ModContent.ItemType<OrangePhaseScourge>(),
                    ModContent.ItemType<PurplePhaseScourge>(),
                    ModContent.ItemType<RedPhaseScourge>(),
                    ModContent.ItemType<WhitePhaseScourge>(),
                    ModContent.ItemType<YellowPhaseScourge>()
                }
            );

            RecipeGroup.RegisterGroup(
                "CalamitySimpleWhipAddon:AnyPhaseScourge",
                phaseScourgeGroup
            );
        }
    }
}
