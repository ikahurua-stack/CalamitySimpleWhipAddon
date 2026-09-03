using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class OpaqueOnyx : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = InfernalEclipseCompatibility.IsEnabled ? 24 : 32;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;

            Item.shoot = ModContent.ProjectileType<OpaqueOnyxProj>();
            Item.shootSpeed = 4f;

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

            recipe.AddIngredient(ModContent.ItemType<KingsMajesty> ());
            recipe.AddIngredient(ItemID.ThornWhip);
            recipe.AddIngredient(ItemID.BoneWhip);
            recipe.AddIngredient(ItemID.Amethyst, 6);
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 8);
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
