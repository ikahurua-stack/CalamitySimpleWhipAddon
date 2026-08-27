using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using CalamitySimpleWhipAddon.Content.Systems;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class ButterflyEffectParticle : ModProjectile, IAdditiveProjectile
    {
        private Color particleColor;
        private bool colorInitialized = false;

        private int frameSpeed;
        private bool frameSpeedInitialized = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.ignoreWater = true;

            Projectile.scale *= Main.rand.NextFloat(0.9f, 1.4f);
        }

        public override void AI()
        {
            if (!frameSpeedInitialized)
            {
                frameSpeedInitialized = true;
                frameSpeed = Main.rand.Next(4, 12); // 少しばらける
            }

            if (!colorInitialized)
            {
                colorInitialized = true;

                float t = Main.rand.NextFloat();

                Color cyan = new Color(0, 255, 255);
                Color magenta = new Color(255, 0, 140);

                particleColor = Color.Lerp(cyan, magenta, t);
            }
            // 少しランダムに羽ばたく感じ
            float flutter = (float)Math.Sin(Projectile.timeLeft * 0.4f) * 0.1f;
            Projectile.velocity = Projectile.velocity.RotatedBy(flutter);

            // 徐々に減速
            Projectile.velocity *= 0.97f;

            Projectile.velocity.Y -= 0.01f;

            Projectile.velocity += new Vector2(
                Main.rand.NextFloat(-0.02f, 0.02f),
                Main.rand.NextFloat(-0.02f, 0.02f)
            );

            if (Projectile.velocity.Length() > 0.1f)
                Projectile.rotation = 0f;

            // アニメーション
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= 4)
                    Projectile.frame = 0;
            }

            // 後半フェードアウト
            if (Projectile.timeLeft < 15)
                Projectile.alpha += 12;

            Vector3 lightColor = particleColor.ToVector3();
            Lighting.AddLight(Projectile.Center, lightColor * 0.4f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public void DrawAdditive(SpriteBatch spriteBatch)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;

            int frameHeight = tex.Height / 4;
            Rectangle frame = new Rectangle(
                0,
                frameHeight * Projectile.frame,
                tex.Width,
                frameHeight
            );

            float alpha = 1f - Projectile.alpha / 255f;

            Color color = particleColor;
            color *= alpha;

            SpriteEffects effects = Projectile.ai[1] == 1f ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(
                tex,
                Projectile.Center - Main.screenPosition,
                frame,
                color,
                Projectile.rotation,
                frame.Size() / 2,
                Projectile.scale,
                effects,
                0
            );
        }
    }
}