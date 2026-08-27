using CalamityMod;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic; // ← これを追加：List用
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent;
using CalamitySimpleWhipAddon.Content.Common.Players;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class WhipShieldProjectile : ModProjectile
    {
        private static Texture2D horizontalGradient;

        private float spawnTime = -1f;

        private bool hasPlayedSound = false;

        private static void EnsureGradient()
        {
            if (horizontalGradient != null && !horizontalGradient.IsDisposed) return;

            horizontalGradient = new Texture2D(Main.graphics.GraphicsDevice, 256, 1);
            Color[] data = new Color[256];
            for (int i = 0; i < 256; i++)
            {
                float t = Math.Abs(i / 255f - 0.5f) * 2f;
                float intensity = (float)Math.Pow(t, 1.5);
                data[i] = new Color(255, 255, 255) * intensity;
            }
            horizontalGradient.SetData(data);
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.netImportant = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // ⭐ オーナーが死ぬ、またはシールド耐久値が0以下なら消滅
            var modPlayer = player.GetModPlayer<WhipShieldPlayer>();
            if (!player.active || player.dead || modPlayer.shieldLife <= 0)
            {
                Projectile.Kill();
                return;
            }

            if (!hasPlayedSound)
            {
                SoundEngine.PlaySound(RoverDrive.ActivationSound, Projectile.Center);
                hasPlayedSound = true;
            }

            // 位置の同期と、寿命の毎フレーム維持
            Projectile.Center = player.MountedCenter + player.gfxOffY * Vector2.UnitY;
            Projectile.timeLeft = 60; // ⭐ 常に60フレーム（1秒）の余裕を持たせる


            // 残量割合
            Projectile.ai[0] = modPlayer.shieldLife / (float)modPlayer.MaxShield;

            Lighting.AddLight(Projectile.Center, 0f, 0f, 0f); // 一応初期化（任意）

            float baseSize = 70f;
            float halfSize = baseSize * 0.5f;

            float lifeRatio = Projectile.ai[0];

            Color displayColor;
            if (lifeRatio > 0.5f)
                displayColor = Color.Lerp(Color.Yellow, Color.Cyan, (lifeRatio - 0.5f) * 2f);
            else
                displayColor = Color.Lerp(Color.Red, Color.Yellow, lifeRatio * 2f);

            // Lighting用に正規化
            float hitBoost = 1f + (modPlayer.shieldHitTimer / 10f) * 0.5f;
            Vector3 lightColor = displayColor.ToVector3() * 0.25f * hitBoost;


            int pointsPerSide = 5; // 増やすほど滑らか

            for (int i = 0; i < pointsPerSide; i++)
            {
                float t = i / (float)(pointsPerSide - 1);

                float x = MathHelper.Lerp(-halfSize, halfSize, t);
                float y = MathHelper.Lerp(-halfSize, halfSize, t);

                // 上
                Lighting.AddLight(Projectile.Center + new Vector2(x, -halfSize), lightColor);

                // 下
                Lighting.AddLight(Projectile.Center + new Vector2(x, halfSize), lightColor);

                // 左
                Lighting.AddLight(Projectile.Center + new Vector2(-halfSize, y), lightColor);

                // 右
                Lighting.AddLight(Projectile.Center + new Vector2(halfSize, y), lightColor);
            }


        }



        // 描画順序を「UIやワイヤーの手前」に変更する
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCsEachPlayerNighttimeSigns, List<int> ItemGridBox, List<int> overWiresUI, List<int> overWalls)
        {
            overWiresUI.Add(index);
        }

        // 通常のプロジェクタイル描画（プレイヤーの背後）は行わない
        public override bool PreDraw(ref Color lightColor) => false;

        // 手前レイヤーでの描画処理
        public override void PostDraw(Color lightColor)
        {
            EnsureGradient();
            SpriteBatch sb = Main.spriteBatch;
            Player player = Main.player[Projectile.owner];
            if (!player.active) return;

            var modPlayer = player.GetModPlayer<WhipShieldPlayer>();
            if (modPlayer.shieldLife <= 0) return;

            // --- 1. 出現時間の記録 (初回のみ) ---
            if (spawnTime < 0)
            {
                spawnTime = (float)Main.GlobalTimeWrappedHourly;
            }

            // --- 2. 色の計算 ---
            float lifeRatio = Projectile.ai[0];
            Color displayColor;
            if (lifeRatio > 0.5f)
                displayColor = Color.Lerp(Color.Yellow, Color.Cyan, (lifeRatio - 0.5f) * 2f);
            else
                displayColor = Color.Lerp(Color.Red, Color.Yellow, lifeRatio * 2f);

            // --- 3. 展開アニメーションの計算 (オーバーシュート付き) ---
            float currentTime = (float)Main.GlobalTimeWrappedHourly;
            float elapsed = currentTime - spawnTime;
            if (elapsed < 0)
                elapsed += 3600f;

            const float duration = 0.5f; // 少しだけ時間を伸ばすと「戻る」動きが見えやすくなります
            float scaleX = 1f;
            float scaleY = 1f;

            if (elapsed < duration)
            {
                float p = elapsed / duration;

                // オーバーシュート関数の定義
                // 1.0を通り越して、最後に1.0に収束する
                float Overshoot(float t)
                {
                    float s = 1.70158f; // この数値を大きくすると「勢い余る量」が増えます
                    t = t - 1;
                    return t * t * ((s + 1) * t + s) + 1;
                }

                // 横の動き (0.0～0.6の区間で展開)
                float pX = MathHelper.Clamp(p / 0.6f, 0f, 1f);
                scaleX = Overshoot(pX);

                // 縦の動き (0.5～1.0の区間で展開)
                float pY = MathHelper.Clamp((p - 0.5f) / 0.6f, 0f, 1f);
                if (p < 0.5f) scaleY = 0.1f;
                else scaleY = Overshoot(pY);
            }

            float hitBoost = 1f + (modPlayer.shieldHitTimer / 10f) * 1.5f;

            // --- 4. 描画処理 ---
            float baseSize = 70f;
            float width = baseSize * scaleX;
            float height = baseSize * scaleY;

            Texture2D noise = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/TechyNoise").Value;
            Texture2D pixel = TextureAssets.MagicPixel.Value;

            Vector2 center = Projectile.Center - Main.screenPosition;

            // 被弾時に震える
            if (modPlayer.shieldHitTimer > 0)
            {
                float intensity = modPlayer.shieldHitTimer / 10f; // 0〜1
                float shakeAmount = 4f * intensity; // 揺れ幅

                center += Main.rand.NextVector2Circular(shakeAmount, shakeAmount);
            }

            Rectangle rect = new Rectangle((int)(center.X - width / 2), (int)(center.Y - height / 2), (int)width, (int)height);
            float scroll = Main.GlobalTimeWrappedHourly * 0.5f;

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // 描画
            Rectangle sourceRect = new Rectangle((int)(scroll * noise.Width) % noise.Width, 0, noise.Width, noise.Height);
            sb.Draw(noise, rect, sourceRect, displayColor * (0.6f * hitBoost));
            sb.Draw(pixel, rect, displayColor * 0.04f);
            sb.Draw(horizontalGradient, rect, displayColor * (0.4f * hitBoost));

            // --- 枠の描画 (1ドットずつ厚みと透明度を変える) ---

            // 1. 外側の1ドット (外枠)
            Rectangle outTop = new Rectangle(rect.X, rect.Y, rect.Width, 1);
            Rectangle outBottom = new Rectangle(rect.X, rect.Bottom - 1, rect.Width, 1);
            Rectangle outLeft = new Rectangle(rect.X, rect.Y, 1, rect.Height);
            Rectangle outRight = new Rectangle(rect.Right - 1, rect.Y, 1, rect.Height);

            // 2. 内側の1ドット (内枠: 1ドット分内側にずらす)
            Rectangle inTop = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, 1);
            Rectangle inBottom = new Rectangle(rect.X + 1, rect.Bottom - 2, rect.Width - 2, 1);
            Rectangle inLeft = new Rectangle(rect.X + 1, rect.Y + 1, 1, rect.Height - 2);
            Rectangle inRight = new Rectangle(rect.Right - 2, rect.Y + 1, 1, rect.Height - 2);

            // --- 描画実行 ---

            // 外枠 (はっきり)
            sb.Draw(pixel, outTop, displayColor * 0.03f);
            sb.Draw(pixel, outBottom, displayColor * 0.03f);
            sb.Draw(pixel, outLeft, displayColor * 0.85f);
            sb.Draw(pixel, outRight, displayColor * 0.85f);

            // 内枠 (薄く)
            sb.Draw(pixel, inTop, displayColor * 0.01f);
            sb.Draw(pixel, inBottom, displayColor * 0.01f);
            sb.Draw(pixel, inLeft, displayColor * 0.4f);
            sb.Draw(pixel, inRight, displayColor * 0.4f);

            
            sb.Draw(horizontalGradient, outTop, displayColor * 0.85f);
            sb.Draw(horizontalGradient, outBottom, displayColor * 0.85f);
            sb.Draw(horizontalGradient, inTop, displayColor * 0.4f);
            sb.Draw(horizontalGradient, inBottom, displayColor * 0.4f);

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

    }
}