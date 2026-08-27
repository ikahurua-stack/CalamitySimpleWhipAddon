using CalamityMod.Items;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;
using CalamityMod;

namespace CalamitySimpleWhipAddon.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class BleachedStatisCurse : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 3;
            player.GetKnockback<SummonDamageClass>() += 2.75f;
            player.GetDamage<SummonDamageClass>() += 0.17f;
            player.buffImmune[ModContent.BuffType<Shadowflame>()] = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<StatisBlessing>());
            recipe.AddIngredient(ModContent.ItemType<BleachedTheFirstShadowflame>());
            recipe.AddIngredient(ItemID.FragmentStardust, 10);
            recipe.AddTile(TileID.LunarCraftingStation);

            recipe.Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StatisCurse>())
                .Register();

            Recipe.Create(ModContent.ItemType<StatisCurse>())
                .AddIngredient(Type)
                .Register();
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            CalamityUtils.DrawInventoryCustomScale(
                spriteBatch,
                texture: TextureAssets.Item[Type].Value,
                position,
                frame,
                drawColor,
                itemColor,
                origin,
                scale,
                wantedScale: 1f,
                drawOffset: new(0f, 0f)
            );
            return false;
        }
    }
}