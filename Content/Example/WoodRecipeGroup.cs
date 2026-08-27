using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.FurnitureMonolith;
using CalamityMod.Items.Placeables.FurnitureAcidwood;

namespace CalamitySimpleWhipAddon
{
    public class WoodRecipeGroup : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup woodGroup = new RecipeGroup(
                () => Language.GetTextValue("Mods.CalamitySimpleWhipAddon.RecipeGroups.AnyWood"),
                new int[]
                {
                    ItemID.Wood,
                    ItemID.BorealWood,
                    ItemID.PalmWood,
                    ItemID.Ebonwood,
                    ItemID.Shadewood,
                    ItemID.RichMahogany,
                    ItemID.Pearlwood,

                    ModContent.ItemType<AstralMonolith>(),
                    ModContent.ItemType<Acidwood>(),
                }
            );

            RecipeGroup.RegisterGroup(
                "CalamitySimpleWhipAddon:AnyWood",
                woodGroup
            );
        }
    }
}

