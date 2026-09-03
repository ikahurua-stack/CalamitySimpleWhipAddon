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
    public class Satellite : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = InfernalEclipseCompatibility.IsEnabled ? 120 : 115;
            Item.knockBack = 4;
            Item.rare = ItemRarityID.Purple;
            Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;

            Item.shoot = ModContent.ProjectileType<SatelliteProj>();
            Item.shootSpeed = 8f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = InfernalEclipseCompatibility.IsEnabled ? 12 : 14;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;


        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ItemID.LunarBar, 15);
            recipe.AddIngredient(ItemID.FragmentStardust, 8);

            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}
