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


	public class ButterflyEffectMoth : ModProjectile, IAdditiveProjectile
    {
        private Color mothColor;
        private bool colorInitialized = false;

        private int frameSpeed;
        private bool frameSpeedInitialized = false;

        public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 240;

			Projectile.scale *= Main.rand.NextFloat(0.9f, 1.4f);
		}

		private Vector2 Offset => new Vector2(Projectile.ai[1], Projectile.localAI[0]);

		public override void AI()
		{
            if (Projectile.ai[0] < 0 || Projectile.ai[0] >= Main.maxNPCs)
            {
                Projectile.Kill();
                return;
            }

            NPC target = Main.npc[(int)Projectile.ai[0]];

            if (!target.active)
            {
                Projectile.Kill();
                return;
            }

            if (!frameSpeedInitialized)
            {
                frameSpeedInitialized = true;
                frameSpeed = Main.rand.Next(4, 7); // 4～6くらい（元は5）
            }

            if (!colorInitialized)
            {
                colorInitialized = true;

                float t = Main.rand.NextFloat();

                Color cyan = new Color(0, 255, 255);
                Color magenta = new Color(255, 0, 140);

                mothColor = Color.Lerp(cyan, magenta, t);
            }

            if (Projectile.localAI[1] == 0f)
			{
				if (Main.rand.NextBool())
					Projectile.localAI[1] = 1f; 
				else
					Projectile.localAI[1] = 2f; 
			}
			// ===== サイズ調整 =====
			float baseScale = 0.45f; 

			float sizeFactor = (target.width + target.height) * 0.5f;

			// 基準サイズ（ゾンビくらい）
			float referenceSize = 50f;

			float scaleMultiplier = sizeFactor / referenceSize;

			// 最小最大制限
			scaleMultiplier = MathHelper.Clamp(scaleMultiplier, 0.9f, 2.6f);

			Projectile.scale = baseScale * scaleMultiplier;

			if (!target.active)
			{
				Projectile.Kill();
				return;
			}

            if (Projectile.ai[1] == 0f && Projectile.localAI[0] == 0f)
            {
                // 敵のサイズに合わせてランダムな位置をセット
                float range = (target.width + target.height) * 0.25f;
                Projectile.ai[1] = Main.rand.NextFloat(-range, range);     // Xオフセット
                Projectile.localAI[0] = Main.rand.NextFloat(-range, range); // Yオフセット
                Projectile.netUpdate = true; // マルチプレイ同期用
            }

            Vector2 offset = new Vector2(Projectile.ai[1], Projectile.localAI[0]);


			// 位置固定
			Projectile.Center = target.Center + offset;

			// ===== 敵中心を向く =====
			Vector2 toCenter = target.Center - Projectile.Center;
			Projectile.rotation = toCenter.ToRotation() + MathHelper.Pi;

			// ===== アニメーション =====
			Projectile.frameCounter++;
            if (Projectile.frameCounter >= frameSpeed)
            {
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= 8)
					Projectile.frame = 0;
			}

			// ===== フェードアウト =====
			int fadeStart = 60;
			if (Projectile.timeLeft < fadeStart)
			{
				float progress = 1f - Projectile.timeLeft / (float)fadeStart;
				Projectile.alpha = (int)(255f * progress);
			}

            Vector3 lightColor = mothColor.ToVector3();
            Lighting.AddLight(Projectile.Center, lightColor * 0.6f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public void DrawAdditive(SpriteBatch spriteBatch)
        {
            NPC target = Main.npc[(int)Projectile.ai[0]];

            Texture2D tex = TextureAssets.Projectile[Type].Value;

			int frameHeight = tex.Height / 8;
			Rectangle frame = new Rectangle(
				0,
				frameHeight * Projectile.frame,
				tex.Width,
				frameHeight
			);

			SpriteEffects effects = SpriteEffects.None;

			if (Projectile.Center.X < target.Center.X)
				effects = SpriteEffects.FlipVertically;

            // ===== 色決定 =====
            Color drawColor = mothColor;
            drawColor *= (1f - Projectile.alpha / 255f);
            drawColor *= 1.3f;

            

			Main.EntitySpriteDraw(
				tex,
				Projectile.Center - Main.screenPosition,
				frame,
                drawColor,
                Projectile.rotation,
				frame.Size() / 2,
				Projectile.scale,
				effects,
				0
			);
		}
	}
}