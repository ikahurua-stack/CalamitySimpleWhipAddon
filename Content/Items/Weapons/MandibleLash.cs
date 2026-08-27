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
    public class MandibleLash : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 12;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Green;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;

            Item.shoot = ModContent.ProjectileType<MandibleLashProj>();
            Item.shootSpeed = 2f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item7;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ItemID.SandstoneBrick, 15);
            recipe.AddIngredient(ItemID.AntlionMandible, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}