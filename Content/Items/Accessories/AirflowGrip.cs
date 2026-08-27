using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Common;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Items;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
	public class AirflowGrip : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.accessory = true;
			Item.rare = ItemRarityID.Orange;
			Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.whipRangeMultiplier -= 0.15f;

            player.GetModPlayer<WhipAccessoryPlayer>().airflowGrip = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
                .AddIngredient(ModContent.ItemType<AerialiteBar>(), 4)
                .AddIngredient(ItemID.SunplateBlock, 5)
                .AddIngredient(ItemID.Feather, 2)
                .AddTile(TileID.Anvils)
				.Register();
		}
	}
}