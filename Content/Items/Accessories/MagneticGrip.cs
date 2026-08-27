using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Common;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
	public class MagneticGrip : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.accessory = true;
			Item.rare = ItemRarityID.Green;
			Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.whipRangeMultiplier -= 0.15f;

            player.GetModPlayer<WhipAccessoryPlayer>().magneticGrip = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 4)
                .AddIngredient(ModContent.ItemType<DubiousPlating>(), 5)
                .AddTile(TileID.Anvils)
				.Register();
		}
	}
}