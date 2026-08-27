using CalamityMod.Items;
using CalamityMod.Buffs.StatDebuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
    public class BleachedNuclearFuelRod : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 1;
            player.GetDamage<SummonDamageClass>() += 0.07f;
            player.buffImmune[ModContent.BuffType<Irradiated>()] = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<NuclearFuelRod>())
                .Register();

            Recipe.Create(ModContent.ItemType<NuclearFuelRod>())
                .AddIngredient(Type)
                .Register();
        }
    }
}