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
using CalamityMod.Items.Weapons.Ranged;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class ShieldConduitMkIV : ModItem, IHoldShiftTooltipItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 260;
            Item.knockBack = 2;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;

            Item.shoot = ModContent.ProjectileType<ShieldConduitMkIVProj>();
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
                AddIngredient<ShieldConduitMkIII>().
                AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 15).
                AddIngredient(ModContent.ItemType<DubiousPlating>(), 15).
                AddIngredient(ModContent.ItemType<UelibloomBar>(), 8).
                AddIngredient(ItemID.LunarBar, 4).
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