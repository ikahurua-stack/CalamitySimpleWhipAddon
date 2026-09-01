using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Common;
using CalamityMod.Items;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
	public class BuddyEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.accessory = true;
			Item.rare = ItemRarityID.Expert;

            Item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
            modPlayer.buddyEmblem = true;

            player.AddBuff(
                ModContent.BuffType<Content.Buffs.BuddyEmblemBuff>(),
                2
            );
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BabyBirdStaff, 1)
                .AddRecipeGroup("CalamitySimpleWhipAddon:AnyWood", 5)
                .AddIngredient(ItemID.Daybloom, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
