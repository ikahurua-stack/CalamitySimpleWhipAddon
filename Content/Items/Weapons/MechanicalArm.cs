using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamitySimpleWhipAddon.Content.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class MechanicalArm : ModItem
    {

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 60;
            Item.knockBack = 4;
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;

            Item.shoot = ModContent.ProjectileType<MechanicalArmProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.UseSound = SoundID.Item108 with { Pitch = Main.rand.NextFloat(1.2f, 1.4f)};
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<WulfrumArm>());
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemID.SoulofFright, 2);
            recipe.AddIngredient(ItemID.SoulofMight, 2);
            recipe.AddIngredient(ItemID.SoulofSight, 2);
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