using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
	public class BreezePiercer : ModItem
	{
		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.SummonMeleeSpeed;
			Item.damage = InfernalEclipseCompatibility.IsEnabled ? 21 : 19;
			Item.knockBack = 1;
			Item.rare = ItemRarityID.Orange;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;

            Item.shoot = ModContent.ProjectileType<BreezePiercerProj>();
			Item.shootSpeed = 8f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.UseSound = SoundID.Item7;
            Item.autoReuse = true;
            Item.noMelee = true;
			Item.noUseGraphic = true;
		}


		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();

			recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 7);
			recipe.AddIngredient(ItemID.SunplateBlock, 4);

			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		public override bool MeleePrefix()
		{
			return true;
		}
	}
}
