using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamitySimpleWhipAddon.Content.Common.Players;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class GildedReliquary : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 316;
            Item.knockBack = 3;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;

            Item.shoot = ModContent.ProjectileType<GildedReliquaryProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GoldRush>()
                .AddRecipeGroup("AnyGoldBar", 30)
                .AddIngredient(ItemID.GoldDust, 100)
                .AddIngredient(ModContent.ItemType<ArmoredShell>(), 2)
                .AddIngredient(ModContent.ItemType<TwistingNether>(), 2)
                .AddIngredient(ModContent.ItemType<DarkPlasma>(), 2)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}