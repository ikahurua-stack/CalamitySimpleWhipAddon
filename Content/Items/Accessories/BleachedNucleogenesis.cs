using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
    public class BleachedNucleogenesis : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 10));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 52;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<CosmicPurple>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 4;
            player.GetKnockback<SummonDamageClass>() += 3f;
            player.GetDamage<SummonDamageClass>() += 0.22f;
            player.buffImmune[ModContent.BuffType<Shadowflame>()] = true;
            player.buffImmune[ModContent.BuffType<Irradiated>()] = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<BleachedStarTaintedGenerator>());
            recipe.AddIngredient(ModContent.ItemType<BleachedStatisCurse>());
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4);
            recipe.AddTile(ModContent.TileType<CosmicAnvil>());

            recipe.Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Nucleogenesis>())
                .Register();

            Recipe.Create(ModContent.ItemType<Nucleogenesis>())
                .AddIngredient(Type)
                .Register();
        }
    }
}