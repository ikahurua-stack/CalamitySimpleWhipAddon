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
    public class Rubellus : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 263;
            Item.knockBack = 4;
            Item.rare = ItemRarityID.Purple;
            Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;

            Item.shoot = ModContent.ProjectileType<RubellusProj>();
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
            recipe.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 12);
            recipe.AddIngredient(ItemID.Ruby, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}