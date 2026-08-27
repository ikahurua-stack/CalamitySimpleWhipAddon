using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using CalamityMod.Items;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class Necropsia : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 175;
            Item.knockBack = 0;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;

            Item.shoot = ModContent.ProjectileType<NecropsiaProj>();
            Item.shootSpeed = 8f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.UseSound = SoundID.Item103;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;


        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient<Ectopia>();
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Necroplasm>(), 20);

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}