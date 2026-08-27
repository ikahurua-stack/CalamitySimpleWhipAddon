using System;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Cooldowns;
using CalamitySimpleWhipAddon.Content.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using static CalamityMod.CalamityUtils;
using static Terraria.ModLoader.ModContent;

namespace CalamitySimpleWhipAddon.Content.Cooldowns
{
	public class ShieldConduitDurability : CooldownHandler
	{
		private static Color ringColorLerpStart = new Color(49, 120, 221);
		private static Color ringColorLerpEnd = new Color(20, 99, 150);

		private float AdjustedCompletion => instance.timeLeft / (float)instance.player.GetModPlayer<WhipShieldPlayer>().MaxShield;

		public static new string ID => "ShieldConduitDurability";
		public override bool CanTickDown => instance.player.GetModPlayer<WhipShieldPlayer>().shieldLife <= 0 || instance.timeLeft <= 0;
		public override bool ShouldDisplay => instance.player.GetModPlayer<WhipShieldPlayer>().shieldLife > 0;
		public override LocalizedText DisplayName => GetText($"UI.Cooldowns.{ID}");
		public override string Texture => "CalamitySimpleWhipAddon/Content/Cooldowns/ShieldConduitActive";
		public override string OutlineTexture => "CalamitySimpleWhipAddon/Content/Cooldowns/ShieldConduitOutline";
		public override string OverlayTexture => "CalamitySimpleWhipAddon/Content/Cooldowns/ShieldConduitOverlay";
		public override Color OutlineColor => new Color(112, 244, 244);
		public override Color CooldownStartColor => Color.Lerp(ringColorLerpStart, ringColorLerpEnd, instance.Completion);
		public override Color CooldownEndColor => Color.Lerp(ringColorLerpStart, ringColorLerpEnd, instance.Completion);
		public override bool SavedWithPlayer => false;
		public override bool PersistsThroughDeath => false;


		public override void ApplyBarShaders(float opacity)
		{
			GameShaders.Misc["CalamityMod:CircularBarShader"].UseOpacity(opacity);
			GameShaders.Misc["CalamityMod:CircularBarShader"].UseSaturation(AdjustedCompletion);
			GameShaders.Misc["CalamityMod:CircularBarShader"].UseColor(CooldownStartColor);
			GameShaders.Misc["CalamityMod:CircularBarShader"].UseSecondaryColor(CooldownEndColor);
			GameShaders.Misc["CalamityMod:CircularBarShader"].Apply();
		}

		public override void DrawExpanded(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
		{
			base.DrawExpanded(spriteBatch, position, opacity, scale);

			float Xoffset = instance.timeLeft > 9 ? -10f : -5;

			float lifeRatio = AdjustedCompletion;

			Color textColor = lifeRatio > 0.5f
				? Color.Lerp(Color.Yellow, Color.Cyan, (lifeRatio - 0.5f) * 2f)
				: Color.Lerp(Color.Red, Color.Yellow, lifeRatio * 2f);

			DrawBorderStringEightWay(spriteBatch, FontAssets.MouseText.Value, instance.timeLeft.ToString(), position + new Vector2(Xoffset, 4) * scale, textColor, Color.Black, scale);
		}

		public override void DrawCompact(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
		{
			Texture2D sprite = Request<Texture2D>(Texture).Value;
			Texture2D outline = Request<Texture2D>(OutlineTexture).Value;
			Texture2D overlay = Request<Texture2D>(OverlayTexture).Value;

			spriteBatch.Draw(outline, position, null, OutlineColor * opacity, 0, outline.Size() * 0.5f, scale, SpriteEffects.None, 0f);

			spriteBatch.Draw(sprite, position, null, Color.White * opacity, 0, sprite.Size() * 0.5f, scale, SpriteEffects.None, 0f);

			int lostHeight = (int)Math.Ceiling(overlay.Height * AdjustedCompletion);
			Rectangle crop = new Rectangle(0, lostHeight, overlay.Width, overlay.Height - lostHeight);
			spriteBatch.Draw(overlay, position + Vector2.UnitY * lostHeight * scale, crop, OutlineColor * opacity * 0.9f, 0, sprite.Size() * 0.5f, scale, SpriteEffects.None, 0f);

			float Xoffset = instance.timeLeft > 9 ? -10f : -5;

			float lifeRatio = AdjustedCompletion;

			Color textColor = lifeRatio > 0.5f
				? Color.Lerp(Color.Yellow, Color.Cyan, (lifeRatio - 0.5f) * 2f)
				: Color.Lerp(Color.Red, Color.Yellow, lifeRatio * 2f);

			DrawBorderStringEightWay(spriteBatch, FontAssets.MouseText.Value, instance.timeLeft.ToString(), position + new Vector2(Xoffset, 4) * scale, textColor, Color.Black, scale);
		}
	}

	public class ShieldConduitRecharge : CooldownHandler
	{
		private static Color ringColorLerpStart = new Color(120, 255, 120);
		private static Color ringColorLerpEnd = new Color(92, 187, 150);

		public static new string ID => "ShieldConduitRecharge";
		public float CompletionPercentage => MathHelper.Clamp(instance.timeLeft / (float)instance.player.GetModPlayer<WhipShieldPlayer>().MaxCharge, 0f, 1f);
		private bool IsEmpty => CompletionPercentage == 100;
		public override bool CanTickDown => false;
		public override bool ShouldDisplay => instance.player.GetModPlayer<WhipShieldPlayer>().charge > 0;
		public override LocalizedText DisplayName => GetText($"UI.Cooldowns.{ID}");
		public override string Texture => "CalamitySimpleWhipAddon/Content/Cooldowns/ShieldConduit";
		public override string OutlineTexture => "CalamitySimpleWhipAddon/Content/Cooldowns/ShieldConduitOutline";
		public override string OverlayTexture => "CalamitySimpleWhipAddon/Content/Cooldowns/ShieldConduitOverlay";
		public override bool SavedWithPlayer => false;
		public override bool PersistsThroughDeath => false;
		public override Color OutlineColor => new Color(194, 255, 67);
		public override Color CooldownStartColor => Color.Lerp(ringColorLerpStart, ringColorLerpEnd, CompletionPercentage);
		public override Color CooldownEndColor => Color.Lerp(ringColorLerpStart, ringColorLerpEnd, CompletionPercentage);

		public override void ApplyBarShaders(float opacity)
		{
			// CircularBarShader に対して、標準の instance.Completion ではなく自前の計算値を渡す
			Terraria.Graphics.Shaders.GameShaders.Misc["CalamityMod:CircularBarShader"].UseOpacity(opacity);
			Terraria.Graphics.Shaders.GameShaders.Misc["CalamityMod:CircularBarShader"].UseSaturation(CompletionPercentage);
			Terraria.Graphics.Shaders.GameShaders.Misc["CalamityMod:CircularBarShader"].UseColor(CooldownStartColor);
			Terraria.Graphics.Shaders.GameShaders.Misc["CalamityMod:CircularBarShader"].UseSecondaryColor(CooldownEndColor);
			Terraria.Graphics.Shaders.GameShaders.Misc["CalamityMod:CircularBarShader"].Apply();
		}

		public override void DrawExpanded(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
		{
			base.DrawExpanded(spriteBatch, position, opacity, scale);

			// チャージ量を％表示にしたい場合は instance.timeLeft.ToString() + "%" などに書き換え可能
			string text = instance.timeLeft.ToString();
			float xOffset = instance.timeLeft > 99 ? -12f : (instance.timeLeft > 9 ? -8f : -4f);

			CalamityUtils.DrawBorderStringEightWay(spriteBatch, FontAssets.MouseText.Value, text, position + new Vector2(xOffset, 4) * scale, Color.White * opacity, Color.Black * opacity, scale);
		}

		public override void Tick()
		{
		}

		// Rover Drive shields are recharged as a side effect of the cooldown completing client side
		public override void OnCompleted()
		{
		}
		
	}

    public class ShieldConduitRecovery : CooldownHandler
    {
        private static Color ringColorLerpStart = new Color(255, 180, 80);
        private static Color ringColorLerpEnd = new Color(255, 80, 80);

        public static new string ID => "ShieldConduitRecovery";

        public float Completion
        {
            get
            {
                var mp = instance.player.GetModPlayer<WhipShieldPlayer>();

                if (mp.rechargeCooldownMax <= 0)
                    return 0f;

                return 1f - (instance.timeLeft / (float)mp.rechargeCooldownMax);
            }
        }

        public override bool ShouldDisplay =>
            instance.player.GetModPlayer<WhipShieldPlayer>().rechargeCooldown > 0;

        public override bool CanTickDown => true;

        public override LocalizedText DisplayName => GetText($"UI.Cooldowns.{ID}");

        public override string Texture =>
            "CalamitySimpleWhipAddon/Content/Cooldowns/ShieldConduit";

        public override string OutlineTexture =>
            "CalamitySimpleWhipAddon/Content/Cooldowns/ShieldConduitOutline";

        public override string OverlayTexture =>
            "CalamitySimpleWhipAddon/Content/Cooldowns/ShieldConduitOverlay";

        public override bool SavedWithPlayer => false;
        public override bool PersistsThroughDeath => false;

        public override Color OutlineColor => new Color(255, 120, 80);

        public override Color CooldownStartColor =>
            Color.Lerp(ringColorLerpStart, ringColorLerpEnd, instance.Completion);

        public override Color CooldownEndColor =>
            Color.Lerp(ringColorLerpStart, ringColorLerpEnd, instance.Completion);

        public override void ApplyBarShaders(float opacity)
        {
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseOpacity(opacity);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseSaturation(Completion);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseColor(CooldownStartColor);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseSecondaryColor(CooldownEndColor);
            GameShaders.Misc["CalamityMod:CircularBarShader"].Apply();
        }

        public override void DrawCompact(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
        {
            Texture2D sprite = Request<Texture2D>(Texture).Value;
            Texture2D outline = Request<Texture2D>(OutlineTexture).Value;
            Texture2D overlay = Request<Texture2D>(OverlayTexture).Value;

            spriteBatch.Draw(outline, position, null, OutlineColor * opacity, 0,
                outline.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            spriteBatch.Draw(sprite, position, null, Color.White * opacity, 0,
                sprite.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            int lostHeight = (int)Math.Ceiling(overlay.Height * (1f - Completion));
            Rectangle crop = new Rectangle(0, lostHeight, overlay.Width, overlay.Height - lostHeight);

            spriteBatch.Draw(overlay,
                position + Vector2.UnitY * lostHeight * scale,
                crop,
                OutlineColor * opacity * 0.9f,
                0,
                sprite.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f);

            string text = instance.timeLeft.ToString();
            float xOffset = instance.timeLeft > 9 ? -10f : -5f;

            DrawBorderStringEightWay(spriteBatch,
                FontAssets.MouseText.Value,
                text,
                position + new Vector2(xOffset, 4) * scale,
                Color.White,
                Color.Black,
                scale);
        }
    }

}