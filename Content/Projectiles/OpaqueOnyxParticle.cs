using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using CalamitySimpleWhipAddon.Content.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class OpaqueOnyxParticle : ModProjectile, IAdditiveProjectile
    {
        private Color particleColor;
        private bool initialized = false;

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 40;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (!initialized)
            {
                initialized = true;

                // ===== 色ランダム =====
                float t = Main.rand.NextFloat();
                Color color1 = new Color(175, 0, 200);
                Color color2 = new Color(200, 100, 255);
                particleColor = Color.Lerp(color1, color2, t);

                // ===== 回転ランダム =====
                Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);

                // ===== プレイヤーから離れる方向 =====
                Player player = Main.player[Projectile.owner];

                Vector2 dir = Projectile.Center - player.Center;
                if (dir.Length() < 0.1f)
                    dir = Main.rand.NextVector2Unit();

                dir.Normalize();

                // 少し拡散
                dir = dir.RotatedBy(Main.rand.NextFloat(-0.6f, 0.6f));

                float speed = Main.rand.NextFloat(5f, 9f);
                Projectile.velocity = dir * speed;

                Projectile.scale = Main.rand.NextFloat(0.8f, 1.3f);
            }

            // ===== 減速 =====
            Projectile.velocity *= 0.96f;


            // ===== 回転 =====
            Projectile.rotation += 0.08f;

            // ===== フェードアウト =====
            if (Projectile.timeLeft < 20)
            {
                float progress = 1f - Projectile.timeLeft / 20f;
                Projectile.alpha = (int)(255f * progress);
            }

            // ===== 発光 =====
            Lighting.AddLight(Projectile.Center, particleColor.ToVector3() * 0.3f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        // 一括描画システムから呼ばれる専用メソッド
        public void DrawAdditive(SpriteBatch spriteBatch)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            float alpha = 1f - Projectile.alpha / 255f;
            Color drawColor = particleColor * alpha * 2f;

            // EntitySpriteDraw ではなく spriteBatch.Draw を使用（一括描画用）
            spriteBatch.Draw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                Projectile.rotation,
                tex.Size() / 2f,
                Projectile.scale * 0.65f,
                SpriteEffects.None,
                0
            );
        }
    }
}