using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
    public class BleachedStarbusterCore : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 1;
            player.GetDamage<SummonDamageClass>() += 0.10f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StarbusterCore>())
                .Register();

            Recipe.Create(ModContent.ItemType<StarbusterCore>())
                .AddIngredient(Type)
                .Register();
        }
    }
}