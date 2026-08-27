using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalamityMod.Tiles.Furniture.CraftingStations;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class Milkyway : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 395;
            Item.knockBack = 2;
            Item.rare = ModContent.RarityType<ExoticRainbow>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;

            Item.shoot = ModContent.ProjectileType<MilkywayProj>();
            Item.shootSpeed = 4f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.UseSound = Exoblade.SwingSound with { Volume = 0.50f };
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;


        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient<Necropsia>();
            recipe.AddIngredient<Satellite>();
            recipe.AddIngredient<GlitterGutter>();
            recipe.AddIngredient<EtaCarinae>();
            recipe.AddIngredient(ModContent.ItemType<MiracleMatter>(), 1);

            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}