using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Common;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Items;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
	public class EmperorsGrip : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.accessory = true;
			Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
        }

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.maxMinions += 2;

            // ムチの範囲 +10%
            player.whipRangeMultiplier += 0.3f;

			player.GetModPlayer<WhipAccessoryPlayer>().emperorsGrip = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
                .AddIngredient(ModContent.ItemType<NecromanticGrip>())
                .AddIngredient(ItemID.LunarBar, 8)
                .AddIngredient(ItemID.FragmentStardust, 7)
                .AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4)
                .AddTile(ModContent.TileType<CosmicAnvil>())
				.Register();
		}
	}
}