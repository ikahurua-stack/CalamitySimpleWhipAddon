using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using System.Collections.Generic;
using CalamityMod.Items;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class Gelxyribose : ModItem
    {

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 62;
            Item.knockBack = 5;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;

            Item.shoot = ModContent.ProjectileType<GelxyriboseProj>();
            Item.shootSpeed = 2f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 44;
            Item.useAnimation = 44;
            Item.UseSound = SoundID.Item95;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 15);
            recipe.AddIngredient(ModContent.ItemType<BlightedGel>(), 15);
            recipe.AddTile(TileID.Solidifier);
            recipe.Register();
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}