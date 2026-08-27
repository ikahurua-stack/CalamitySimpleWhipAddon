using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles; // ← 追加
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class WoodenWhip : ModItem
    {

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 6;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.White;
            Item.value = Item.buyPrice(0, 0, 0, 50);

            Item.shoot = ModContent.ProjectileType<WoodenWhipProj>();
            Item.shootSpeed = 3f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 38;
            Item.useAnimation = 38;
            Item.UseSound = SoundID.Item152;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Rope, 3)
                .AddRecipeGroup("CalamitySimpleWhipAddon:AnyWood", 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}