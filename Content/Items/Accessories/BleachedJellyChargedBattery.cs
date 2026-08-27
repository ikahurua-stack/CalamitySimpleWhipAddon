using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
    public class BleachedJellyChargedBattery : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 1;
            player.GetDamage<SummonDamageClass>() += 0.12f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<WulfrumBattery>());
            recipe.AddIngredient(ModContent.ItemType<BleachedVoltaicJelly>());
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(),10);
            recipe.AddIngredient(ModContent.ItemType<StormlionMandible>(),2);
            recipe.AddTile(TileID.Anvils);

            recipe.Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<JellyChargedBattery>())
                .Register();

            Recipe.Create(ModContent.ItemType<JellyChargedBattery>())
                .AddIngredient(Type)
                .Register();
        }
    }
}