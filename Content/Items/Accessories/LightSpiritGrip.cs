using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Common;
using CalamityMod.Items;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
	public class LightSpiritGrip : ModItem
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
			player.GetModPlayer<WhipAccessoryPlayer>().lightSpiritGrip = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
                .AddIngredient(ModContent.ItemType<AirflowGrip>())
                .AddIngredient(ItemID.SoulofLight, 7)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}