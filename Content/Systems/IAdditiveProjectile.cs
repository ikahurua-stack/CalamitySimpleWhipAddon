using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Systems
{
	// これを持っているプロジェクタイルは一括描画の対象になる、という約束事
	public interface IAdditiveProjectile
	{
        void DrawAdditive(SpriteBatch spriteBatch);
	}
}
