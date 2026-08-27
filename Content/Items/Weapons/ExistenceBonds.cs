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
    public class ExistenceBonds : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 185;
            Item.knockBack = 3;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;

            Item.shoot = ModContent.ProjectileType<ExistenceBondsProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item116;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PrimalBonds>()
                .AddIngredient(ItemID.BrokenHeroSword)
                .AddIngredient(ModContent.ItemType<AshesofCalamity>(), 8)
                .AddIngredient(ModContent.ItemType<InfectedArmorPlating>(), 8)
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