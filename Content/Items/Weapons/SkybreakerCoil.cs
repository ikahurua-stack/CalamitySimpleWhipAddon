using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Sounds;
using CalamityMod.Items;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class SkybreakerCoil : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 410;
            Item.knockBack = 0;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;

            Item.shoot = ModContent.ProjectileType<SkybreakerCoilProj>();
            Item.shootSpeed = 4f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.UseSound = DeadSunsWind.ShootSound;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;


        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 7);
            recipe.AddIngredient(ModContent.ItemType<Teslastaff>(), 1);
            recipe.AddIngredient(ItemID.ElectrosphereLauncher);

            recipe.AddTile(ModContent.TileType<CosmicAnvil>());
            recipe.Register();
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}