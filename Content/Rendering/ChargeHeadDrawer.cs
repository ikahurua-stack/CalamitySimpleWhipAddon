using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalamitySimpleWhipAddon.Content.Rendering
{
    public static class ChargeHeadDrawer
    {
        public static void DrawHead(
            Texture2D texture,
            Vector2 center,
            float rotation,
            float radius,
            int frame,
            float scaleFactor = 1f,
            bool offsetForward = true)
        {
            float drawScale = (radius / 45f) * 0.6f * scaleFactor;

            SpriteEffects direction =
                MathF.Cos(rotation) > 0f
                ? SpriteEffects.None
                : SpriteEffects.FlipVertically;

            Rectangle source = texture.Frame(1, 6, 0, frame);
            Vector2 origin = source.Size() * 0.5f;

            Vector2 drawPosition = center - Main.screenPosition;

            if (offsetForward)
                drawPosition += rotation.ToRotationVector2() * radius * 0.5f;

            Main.EntitySpriteDraw(
                texture,
                drawPosition,
                source,
                Color.White,
                rotation,
                origin,
                drawScale,
                direction,
                0);
        }
    }
}