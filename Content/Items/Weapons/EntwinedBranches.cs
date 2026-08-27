using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Rarities;
using CalamityMod.Items;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class EntwinedBranches : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 410;
            Item.knockBack = 20;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;

            Item.shoot = ModContent.ProjectileType<EntwinedBranchesProj>();
            Item.shootSpeed = 2f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.UseSound = SoundID.Grass;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ItemID.ThornWhip);
            recipe.AddIngredient(ModContent.ItemType<UelibloomBar>(), 16);
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