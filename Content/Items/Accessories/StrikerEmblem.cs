using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamityMod.Items.Materials;
using CalamitySimpleWhipAddon.Content.Items.Weapons;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
    public class StrikerEmblem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Expert;

            Item.expert = true;
        }

        public override void UpdateAccessory(
            Player player,
            bool hideVisual)
        {
            player.whipRangeMultiplier += 0.10f;

            player
                .GetModPlayer<EchoWhipPlayer>()
                .echoAccessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<WoodenWhip>())
                .AddRecipeGroup("CalamitySimpleWhipAddon:AnyWood", 5)
                .AddIngredient(ItemID.Daybloom, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}