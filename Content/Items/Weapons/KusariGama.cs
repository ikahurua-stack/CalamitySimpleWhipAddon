using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items;
using CalamitySimpleWhipAddon.Content.Common.Players;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class KusariGama : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 40;
            Item.knockBack = 4;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;

            Item.shoot = ModContent.ProjectileType<KusariGamaProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item152;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override void HoldItem(Player player)
        {
            player.GetModPlayer<WhipHoldEffectPlayer>().holdingNoKnockbackWhip = true;
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}