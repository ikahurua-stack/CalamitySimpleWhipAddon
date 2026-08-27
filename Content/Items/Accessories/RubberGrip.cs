using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Common;
using CalamityMod.Items.Materials;
using CalamityMod.Items;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
	public class RubberGrip : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.accessory = true;
			Item.rare = ItemRarityID.LightRed;
			Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
        }

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			// ムチの範囲 +10%
			player.whipRangeMultiplier += 0.15f;

			player.GetModPlayer<WhipAccessoryPlayer>().rubberGrip = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
                .AddRecipeGroup("AnyCobaltBar", 5)
                .AddIngredient(ModContent.ItemType<LeatherGrip>())
                .AddIngredient(ModContent.ItemType<SilkGrip>())
                .AddTile(TileID.Anvils)
				.Register();
		}
	}
}