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
    public class GlitterGutter : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 52;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;

            Item.shoot = ModContent.ProjectileType<GlitterGutterProj>();
            Item.shootSpeed = 6f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = InfernalEclipseCompatibility.IsEnabled ? 19 : 20;
            Item.UseSound = SoundID.Item101;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<CryonicBar> (), 10);
            recipe.AddIngredient(ItemID.CrystalShard, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}
