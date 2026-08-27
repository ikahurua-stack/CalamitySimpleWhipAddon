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
using CalamityMod.CustomRecipes;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class ShieldConduitMkII : ModItem, IHoldShiftTooltipItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 61;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;

            Item.shoot = ModContent.ProjectileType<ShieldConduitMkIIProj>();
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
                AddIngredient<ShieldConduit>().
                AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 10).
                AddIngredient(ModContent.ItemType<DubiousPlating>(), 10).
                AddRecipeGroup("AnyMythrilBar", 8).
                AddIngredient(ItemID.SoulofSight, 20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}