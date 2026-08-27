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


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class ShieldConduitMkIII : ModItem, IHoldShiftTooltipItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 170;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;

            Item.shoot = ModContent.ProjectileType<ShieldConduitMkIIIProj>();
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
                AddIngredient<ShieldConduitMkII>().
                AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 15).
                AddIngredient(ModContent.ItemType<DubiousPlating>(), 15).
                AddIngredient(ModContent.ItemType<LifeAlloy>(), 5).
                AddIngredient(ModContent.ItemType<InfectedArmorPlating>(), 10).
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