using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
    public class BleachedVoltaicJelly : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.maxMinions += 1;
			player.GetDamage<SummonDamageClass>() += 0.05f;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<VoltaicJelly>())
				.Register();

			Recipe.Create(ModContent.ItemType<VoltaicJelly>())
				.AddIngredient(Type)
				.Register();
		}
	}
}