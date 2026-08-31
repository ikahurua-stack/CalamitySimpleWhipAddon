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
using CalamityMod.Items.Placeables.Ores;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class EtaCarinae : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 332;
            Item.knockBack = 4;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;

            Item.shoot = ModContent.ProjectileType<EtaCarinaeProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.UseSound = SoundID.Item7;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient<ButterflyEffect>();
            recipe.AddIngredient(ModContent.ItemType<Lumenyl>(), 10);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 3);
            recipe.AddIngredient(ModContent.ItemType<ExodiumCluster>(), 12);
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