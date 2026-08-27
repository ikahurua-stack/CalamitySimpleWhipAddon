using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamityMod.Tiles.Furniture.CraftingStations;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class AurelianSanctum : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 798;
            Item.knockBack = 8;
            Item.rare = ModContent.RarityType<HotPink>();
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;

            Item.shoot = ModContent.ProjectileType<AurelianSanctumProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GildedReliquary>()
                .AddRecipeGroup("AnyGoldBar", 40)
                .AddIngredient(ItemID.GoldDust, 200)
                .AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5)
                .AddTile(ModContent.TileType<DraedonsForge>())
                .Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}