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
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Placeables.SunkenSea;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class ShieldConduit : ModItem, IHoldShiftTooltipItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 28;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Orange;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;

            Item.shoot = ModContent.ProjectileType<ShieldConduitProj>();
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
                AddIngredient(ModContent.ItemType<RoverDrive>()).
                AddIngredient(ModContent.ItemType<MysteriousCircuitry>(),5).
                AddIngredient(ModContent.ItemType<DubiousPlating>(),5).
                AddIngredient(ModContent.ItemType<AerialiteBar>(),4).
                AddIngredient(ModContent.ItemType<SeaPrism>(),7).
                AddTile(TileID.Anvils).
                Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}