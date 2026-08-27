using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles; // ← 追加
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class ChorusofExecration : ModItem
    {

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 666;
            Item.knockBack = 4;
            Item.rare = ModContent.RarityType<CalamityRed>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;

            Item.shoot = ModContent.ProjectileType<ChorusofExecrationProj>();
            Item.shootSpeed = 2f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.UseSound = SoundID.Item171;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}