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
using Terraria.Utilities;
using System;


namespace CalamitySimpleWhipAddon.Content.Projectiles
{


	public class ActiasAlienaMoth : ModProjectile, IAdditiveProjectile
    {
        private int frameSpeed;

        private bool initialized = false;

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

			Projectile.scale *= Main.rand.NextFloat(0.9f, 1.3f);
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

            if (!initialized)
            {
                initialized = true;

                UnifiedRandom rand = new UnifiedRandom(Projectile.identity);

                // 羽ばたき速度
                frameSpeed = rand.Next(4, 7);

                // 色
                Projectile.localAI[1] = rand.NextBool() ? 1f : 2f;

                // 初期位置
                float range = (target.width + target.height) * 0.25f;

                Projectile.ai[1] = rand.NextFloat(-range, range);
                Projectile.localAI[0] = rand.NextFloat(-range, range);
            }
            // ===== サイズ調整 =====
            float baseScale = 0.4f; 

			float sizeFactor = (target.width + target.height) * 0.5f;

			// 基準サイズ（ゾンビくらい）
			float referenceSize = 50f;

			float scaleMultiplier = sizeFactor / referenceSize;

			// 最小最大制限
			scaleMultiplier = MathHelper.Clamp(scaleMultiplier, 0.8f, 2.4f);

			Projectile.scale = baseScale * scaleMultiplier;

			if (!target.active)
			{
				Projectile.Kill();
				return;
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

			Lighting.AddLight(Projectile.Center, 0.1f, 0.6f, 0.1f);
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
			Color mothColor;

			if (Projectile.localAI[1] == 1f)
				mothColor = new Color(70, 255, 70);
			else
				mothColor = new Color(65, 255, 85);

			mothColor *= (1f - Projectile.alpha / 255f);

			

			Main.EntitySpriteDraw(
				tex,
				Projectile.Center - Main.screenPosition,
				frame,
				mothColor *= 1.3f,
				Projectile.rotation,
				frame.Size() / 2,
				Projectile.scale,
				effects,
				0
			);

			
		}
	}
}