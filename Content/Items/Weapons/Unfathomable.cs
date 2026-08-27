using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class Unfathomable : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 300;
            Item.knockBack = 15;
            Item.rare = ItemRarityID.Lime;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;

            Item.shoot = ModContent.ProjectileType<UnfathomableProj>();
            Item.shootSpeed = 2f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 57;
            Item.useAnimation = 57;
            Item.UseSound = SoundID.Item167;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient<Droptide>();
            recipe.AddIngredient(ModContent.ItemType<MolluskHusk>(), 6);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 12);
            recipe.AddIngredient(ItemID.BlackPearl, 1);
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