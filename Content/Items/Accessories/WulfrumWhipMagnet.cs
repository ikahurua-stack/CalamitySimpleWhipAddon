using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Common;
using CalamityMod.Items;
using CalamityMod.Items.Materials;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
	public class WulfrumWhipMagnet : ModItem
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
			player.whipRangeMultiplier += 0.10f;

			player.GetModPlayer<WhipAccessoryPlayer>().whipMagnet = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();

			recipe.AddIngredient(ModContent.ItemType<WulfrumMetalScrap>(), 10);
			recipe.AddIngredient(ModContent.ItemType<EnergyCore>());
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
