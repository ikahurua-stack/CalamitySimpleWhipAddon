using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class ShieldConduitMkV : ModItem, IHoldShiftTooltipItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 340;
            Item.knockBack = 2;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;

            Item.shoot = ModContent.ProjectileType<ShieldConduitMkVProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = DeadSunsWind.ShootSound;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ShieldConduitMkIV>().
                AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 20).
                AddIngredient(ModContent.ItemType<DubiousPlating>(), 20).
                AddIngredient(ModContent.ItemType<CosmiliteBar>(), 8).
                AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 2).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}