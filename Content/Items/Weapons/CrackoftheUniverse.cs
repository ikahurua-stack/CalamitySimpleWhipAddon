using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Rarities;
using CalamityMod.Items;

namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class CrackoftheUniverse : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 340;
            Item.knockBack = 9;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;

            Item.shoot = ModContent.ProjectileType<CrackoftheUniverseProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = DeadSunsWind.ShootSound;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<EmeraldSplash>());
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 12);
            recipe.AddIngredient(ItemID.Sapphire, 6);
            recipe.AddIngredient(ItemID.Amethyst, 6);
            recipe.AddTile(ModContent.TileType<CosmicAnvil>());
            recipe.Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}