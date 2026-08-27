using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Common;
using CalamityMod.Items.Materials;
using CalamityMod.Items;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
	public class NecromanticGrip : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.accessory = true;
			Item.rare = ItemRarityID.Yellow;
			Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
        }

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.maxMinions += 1;

            // ムチの範囲 +10%
            player.whipRangeMultiplier += 0.15f;

			player.GetModPlayer<WhipAccessoryPlayer>().necromanticGrip = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
                .AddIngredient(ItemID.NecromanticScroll)
                .AddIngredient(ModContent.ItemType<RubberGrip>())
                .AddIngredient(ModContent.ItemType<LightSpiritGrip>())
                .AddIngredient(ItemID.Ectoplasm, 10)
                .AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}