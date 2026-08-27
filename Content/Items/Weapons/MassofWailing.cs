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
    public class MassofWailing : ModItem
    {

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 126;
            Item.knockBack = 4;
            Item.rare = ItemRarityID.Lime;
            Item.value = CalamityGlobalItem.RarityLimeBuyPrice;

            Item.shoot = ModContent.ProjectileType<MassofWailingProj>();
            Item.shootSpeed = 2f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.UseSound = SoundID.Item171;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<AshesofCalamity>(), 13);
            recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 10);
            recipe.AddIngredient(ItemID.SoulofNight, 8);
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