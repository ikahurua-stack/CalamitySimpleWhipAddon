using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Common;
using CalamityMod.Items;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
	public class LeatherGrip : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.accessory = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			// ムチの範囲 +10%
			player.whipRangeMultiplier += 0.10f;

			player.GetModPlayer<WhipAccessoryPlayer>().leatherGrip = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Leather, 5)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}