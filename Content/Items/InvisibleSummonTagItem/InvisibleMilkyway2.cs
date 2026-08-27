using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CalamitySimpleWhipAddon.Content.Items.InvisibleSummonTagItem
{
	public class InvisibleMilkyway2 : ModItem
	{
        public override void SetDefaults()
        {
            Item.width = 2;
            Item.height = 2;
            Item.rare = ItemRarityID.Gray;
            Item.value = 0;
        }

        public override bool PreDrawInInventory(
            SpriteBatch spriteBatch,
            Vector2 position,
            Rectangle frame,
            Color drawColor,
            Color itemColor,
            Vector2 origin,
            float scale)
        {
            return false; // インベントリにも描画しない
        }
    }
}
