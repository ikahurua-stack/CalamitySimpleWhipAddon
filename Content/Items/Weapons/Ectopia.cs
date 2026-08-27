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
    public class Ectopia : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 69;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;

            Item.shoot = ModContent.ProjectileType<EctopiaProj>();
            Item.shootSpeed = 8f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.UseSound = SoundID.Item103;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;


        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ItemID.SpectreBar, 12);
            recipe.AddIngredient(ItemID.Ectoplasm, 10);

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}