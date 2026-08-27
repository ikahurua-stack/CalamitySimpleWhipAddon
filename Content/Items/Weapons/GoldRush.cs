using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items;
using CalamitySimpleWhipAddon.Content.Common.Players;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class GoldRush : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 171;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;

            Item.shoot = ModContent.ProjectileType<GoldRushProj>();
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
                .AddRecipeGroup("AnyGoldBar", 20)
                .AddIngredient(ItemID.GoldDust, 50)
                .AddIngredient(ModContent.ItemType<LifeAlloy>(), 4)
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