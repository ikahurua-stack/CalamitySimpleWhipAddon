using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Common;
using CalamityMod.Items;
using CalamityMod.Items.Materials;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
	public class CommanderGrip : ModItem
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
			player.GetModPlayer<WhipAccessoryPlayer>().commanderGrip = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
                .AddIngredient(ModContent.ItemType<MagneticGrip>())
                .AddIngredient(ItemID.SoulofNight, 7)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}