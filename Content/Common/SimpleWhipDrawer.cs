using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using System;

namespace CalamitySimpleWhipAddon.Content.Common
{
	public static class SimpleWhipDrawer1
	{
		public static void DrawLine(
	List<Vector2> points,
	Func<Vector2, Color> getColor
)
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new Vector2(frame.Width / 2, 2);

			Vector2 pos = points[0];

			for (int i = 0; i < points.Count - 1; i++)
			{
				Vector2 element = points[i];
				Vector2 diff = points[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;

				Color color = getColor(points[i]) * Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

				Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(
					texture,
					pos - Main.screenPosition,
					frame,
					color,
					rotation,
					origin,
					scale,
					SpriteEffects.None,
					0
				);

				pos += diff;
			}
		}

		public static void DrawSegments(
			Projectile proj,
			List<Vector2> points,
			float timer
		)
		{
			SpriteEffects flip = proj.spriteDirection < 0
				? SpriteEffects.None
				: SpriteEffects.FlipHorizontally;

			Texture2D texture = TextureAssets.Projectile[proj.type].Value;

			Vector2 pos = points[0];

			for (int i = 0; i < points.Count - 1; i++)
			{
				Rectangle frame = new Rectangle(0, 0, 10, 26);
				Vector2 origin = new Vector2(5, 5);
				float scale = 1.8f;

				if (i == points.Count - 2)
				{
					frame.Y = 74;
					frame.Height = 18;

					Projectile.GetWhipSettings(proj, out float timeToFlyOut, out int _, out float _);
					float t = timer / timeToFlyOut;

					scale = MathHelper.Lerp(0.5f, 2f,
						Utils.GetLerpValue(0.1f, 0.7f, t, true) *
						Utils.GetLerpValue(0.9f, 0.7f, t, true));
				}
				else if (i > 10)
				{
					frame.Y = 58;
					frame.Height = 16;
                }
				else if (i > 5)
				{
					frame.Y = 42;
					frame.Height = 16;
                }
				else if (i > 0)
				{
					frame.Y = 26;
					frame.Height = 16;
                }

				Vector2 element = points[i];
				Vector2 diff = points[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(element.ToTileCoordinates());

				Main.EntitySpriteDraw(
					texture,
					pos - Main.screenPosition,
					frame,
					color,
					rotation,
					origin,
					scale,
					flip,
					0
				);

				pos += diff;
			}
		}
	}

    public static class SimpleWhipDrawer2
    {
        public static void DrawLine(
    List<Vector2> points,
    Func<Vector2, Color> getColor
)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;

                Color color = getColor(points[i]) * Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );

                pos += diff;
            }
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 20, 26);
                Vector2 origin = new Vector2(10, 8);
                float scale = 1.8f;

                if (i == points.Count - 2)
                {
                    frame.Y = 74;
                    frame.Height = 28;

                    Projectile.GetWhipSettings(proj, out float timeToFlyOut, out int _, out float _);
                    float t = timer / timeToFlyOut;

                    scale = MathHelper.Lerp(0.5f, 1.8f,
                        Utils.GetLerpValue(0.1f, 0.7f, t, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 10)
                {
                    frame.Y = 58;
                    frame.Height = 16;
                }
                else if (i > 5)
                {
                    frame.Y = 42;
                    frame.Height = 16;
                }
                else if (i > 0)
                {
                    frame.Y = 26;
                    frame.Height = 16;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    flip,
                    0
                );

                pos += diff;
            }
        }
    }

    public static class SimpleWhipDrawer3
    {
        public static void DrawLine(
    List<Vector2> points,
    Func<Vector2, Color> getColor
)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;

                Color color = getColor(points[i]) * Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );

                pos += diff;
            }
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 10, 26);
                Vector2 origin = new Vector2(5, 8);
                float scale = 1.5f;

                if (i == points.Count - 2)
                {
                    frame.Y = 74;
                    frame.Height = 18;

                    Projectile.GetWhipSettings(proj, out float timeToFlyOut, out int _, out float _);
                    float t = timer / timeToFlyOut;

                    scale = MathHelper.Lerp(0.5f, 1.5f,
                        Utils.GetLerpValue(0.1f, 0.7f, t, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 10)
                {
                    frame.Y = 58;
                    frame.Height = 16;

                    Projectile.GetWhipSettings(proj, out float timeToFlyOut, out int _, out float _);
                    float t = timer / timeToFlyOut;

                    scale = MathHelper.Lerp(0.5f, 1.5f,
                        Utils.GetLerpValue(0.1f, 0.7f, t, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
            
                else if (i > 5)
                {
                    frame.Y = 42;
                    frame.Height = 16;

                    Projectile.GetWhipSettings(proj, out float timeToFlyOut, out int _, out float _);
                    float t = timer / timeToFlyOut;

                    scale = MathHelper.Lerp(0.5f, 1.5f,
                        Utils.GetLerpValue(0.1f, 0.7f, t, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 0)
                {
                    frame.Y = 26;
                    frame.Height = 16;

                    Projectile.GetWhipSettings(proj, out float timeToFlyOut, out int _, out float _);
                    float t = timer / timeToFlyOut;

                    scale = MathHelper.Lerp(0.5f, 1.5f,
                        Utils.GetLerpValue(0.1f, 0.7f, t, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    flip,
                    0
                );

                pos += diff;
            }
        }
    }

    public static class SimpleWhipDrawer4
    {
        public static void DrawLine(
    List<Vector2> points,
    Func<Vector2, Color> getColor
)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;

                Color color = getColor(points[i]) * Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );

                pos += diff;
            }
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 10, 26);
                Vector2 origin = new Vector2(5, 8);
                float scale = 1.5f;

                if (i == points.Count - 2)
                {
                    frame.Y = 74;
                    frame.Height = 18;

                    Projectile.GetWhipSettings(proj, out float timeToFlyOut, out int _, out float _);
                    float t = timer / timeToFlyOut;

                    scale = MathHelper.Lerp(0.5f, 2.8f,
                        Utils.GetLerpValue(0.1f, 0.7f, t, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 44)
                {
                    frame.Y = 58;
                    frame.Height = 16;
                }
                else if (i > 22)
                {
                    frame.Y = 42;
                    frame.Height = 16;
                }
                else if (i > 0)
                {
                    frame.Y = 26;
                    frame.Height = 16;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    flip,
                    0
                );

                pos += diff;
            }
        }
    }

    public static class SimpleWhipDrawer5
    {
        public static void DrawLine(
    List<Vector2> points,
    Func<Vector2, Color> getColor
)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;

                Color color = getColor(points[i]) * Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );

                pos += diff;
            }
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 24, 26);
                Vector2 origin = new Vector2(12, 8);
                float scale = 1.4f;

                if (i == points.Count - 2)
                {
                    frame.Y = 74;
                    frame.Height = 26;

                    Projectile.GetWhipSettings(proj, out float timeToFlyOut, out int _, out float _);
                    float t = timer / timeToFlyOut;

                    scale = MathHelper.Lerp(0.5f, 2.3f,
                        Utils.GetLerpValue(0.1f, 0.7f, t, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 44)
                {
                    frame.Y = 58;
                    frame.Height = 16;
                }
                else if (i > 22)
                {
                    frame.Y = 42;
                    frame.Height = 16;
                }
                else if (i > 0)
                {
                    frame.Y = 26;
                    frame.Height = 16;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    flip,
                    0
                );

                pos += diff;
            }
        }
    }

    public static class SimpleWhipDrawer6
    {
        public static void DrawLine(
    List<Vector2> points,
    Func<Vector2, Color> getColor
)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;

                Color color = getColor(points[i]) * Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );

                pos += diff;
            }
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 33);
                Vector2 origin = new Vector2(15, 8);
                float scale = 1.4f;

                if (i == points.Count - 2)
                {
                    frame.Y = 90;
                    frame.Height = 45;

                    Player owner = Main.player[proj.owner];
                    float progress = 1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    // 進行度に応じて先端スケールを補間
                    scale = MathHelper.Lerp(0.5f, 1.9f, Utils.GetLerpValue(0.1f, 0.7f, progress, true) *
                                                        Utils.GetLerpValue(0.9f, 0.7f, progress, true));
                }
                else if (i > 44)
                {
                    frame.Y = 71;
                    frame.Height = 19;
                    scale = 1.2f;
                }
                else if (i > 22)
                {
                    frame.Y = 52;
                    frame.Height = 19;
                    scale = 1.1f;
                }
                else if (i > 0)
                {
                    frame.Y = 33;
                    frame.Height = 19;
                    scale = 1f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    flip,
                    0
                );

                pos += diff;
            }
        }
    }

    public static class SimpleWhipDrawer7
    {
        // ★ 中間②専用スケール倍率（ここだけ触れば見た目調整できる）
        private const float Middle2ScaleMultiplier = 1.25f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 33);
                Vector2 origin = new Vector2(15, 8);
                float scale = 1.4f;

                if (i == points.Count - 2)
                {
                    // ===== 先端 =====
                    frame.Y = 90;
                    frame.Height = 45;

                    Player owner = Main.player[proj.owner];
                    float progress =
                        1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    scale = MathHelper.Lerp(
                        0.5f,
                        1.9f,
                        Utils.GetLerpValue(0.1f, 0.7f, progress, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, progress, true)
                    );
                }
                else if (i > 0)
                {
                    // ===== 中間 =====
                    int[] frames = { 33, 52, 71 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 19;
                    scale = 1f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;
                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケールを強調
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // 位置更新（★ ここが分離されているので長さが壊れない）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class SimpleWhipDrawer8
    {
        // ★ 中間②専用スケール倍率（ここだけ触れば見た目調整できる）
        private const float Middle2ScaleMultiplier = 1.25f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 33);
                Vector2 origin = new Vector2(15, 8);
                float scale = 1.4f;

                if (i == points.Count - 2)
                {
                    // ===== 先端 =====
                    frame.Y = 90;
                    frame.Height = 45;

                    Player owner = Main.player[proj.owner];
                    float progress =
                        1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    scale = MathHelper.Lerp(
                        0.5f,
                        1.9f,
                        Utils.GetLerpValue(0.1f, 0.7f, progress, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, progress, true)
                    );
                }
                else if (i > 0)
                {
                    // ===== 中間 =====
                    int[] frames = { 33, 52, 71 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 19;
                    scale = 0.8f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;
                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケールを強調
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // 位置更新（★ ここが分離されているので長さが壊れない）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class SimpleWhipDrawer9
    {
        // ★ 中間②専用スケール倍率（ここだけ触れば見た目調整できる）
        private const float Middle2ScaleMultiplier = 1.40f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 33);
                Vector2 origin = new Vector2(15, 8);
                float scale = 1.5f;

                if (i == points.Count - 2)
                {
                    // ===== 先端 =====
                    frame.Y = 90;
                    frame.Height = 45;

                    Player owner = Main.player[proj.owner];
                    float progress =
                        1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    scale = MathHelper.Lerp(
                        0.8f,
                        2.4f,
                        Utils.GetLerpValue(0.1f, 0.7f, progress, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, progress, true)
                    );
                }
                else if (i > 0)
                {
                    // ===== 中間 =====
                    int[] frames = { 33, 52, 71 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 19;
                    scale = 1.25f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;
                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケールを強調
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // 位置更新（★ ここが分離されているので長さが壊れない）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class SimpleWhipDrawer10
    {
        public static void DrawLine(
    List<Vector2> points,
    Func<Vector2, Color> getColor
)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;

                Color color = getColor(points[i]) * Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );

                pos += diff;
            }
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 24, 26);
                Vector2 origin = new Vector2(12, 8);
                float scale = 1.0f;

                if (i == points.Count - 2)
                {
                    frame.Y = 74;
                    frame.Height = 26;

                    Projectile.GetWhipSettings(proj, out float timeToFlyOut, out int _, out float _);
                    float t = timer / timeToFlyOut;

                    scale = MathHelper.Lerp(0.5f, 2.6f,
                        Utils.GetLerpValue(0.1f, 0.7f, t, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 44)
                {
                    frame.Y = 58;
                    frame.Height = 16;
                }
                else if (i > 22)
                {
                    frame.Y = 42;
                    frame.Height = 16;
                }
                else if (i > 0)
                {
                    frame.Y = 26;
                    frame.Height = 16;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    flip,
                    0
                );

                pos += diff;
            }
        }
    }

    public static class SimpleWhipDrawer11
    {
        // ★ 中間②専用スケール倍率（ここだけ触れば見た目調整できる）
        private const float Middle2ScaleMultiplier = 1.0f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 33);
                Vector2 origin = new Vector2(15, 8);
                float scale = 1.5f;

                if (i == points.Count - 2)
                {
                    // ===== 先端 =====
                    frame.Y = 90;
                    frame.Height = 45;

                    Player owner = Main.player[proj.owner];
                    float progress =
                        1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    scale = MathHelper.Lerp(
                        0.8f,
                        2.4f,
                        Utils.GetLerpValue(0.1f, 0.7f, progress, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, progress, true)
                    );
                }
                else if (i > 0)
                {
                    // ===== 中間 =====
                    int[] frames = { 33, 52, 71 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 19;
                    scale = 1.25f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;
                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケールを強調
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // 位置更新（★ ここが分離されているので長さが壊れない）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class SimpleWhipDrawer12
    {
        // ★ 中間②専用スケール倍率（ここだけ触れば見た目調整できる）
        private const float Middle2ScaleMultiplier = 1.15f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 33);
                Vector2 origin = new Vector2(15, 8);
                float scale = 1.4f;

                if (i == points.Count - 2)
                {
                    // ===== 先端 =====
                    frame.Y = 90;
                    frame.Height = 45;

                    Player owner = Main.player[proj.owner];
                    float progress =
                        1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    scale = MathHelper.Lerp(
                        0.8f,
                        1.9f,
                        Utils.GetLerpValue(0.1f, 0.7f, progress, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, progress, true)
                    );
                }
                else if (i > 0)
                {
                    // ===== 中間 =====
                    int[] frames = { 33, 52, 71 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 19;
                    scale = 1.25f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;
                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケールを強調
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // 位置更新（★ ここが分離されているので長さが壊れない）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class SimpleWhipDrawer13
    {
        // ★ 中間②専用スケール倍率（ここだけ触れば見た目調整できる）
        private const float Middle2ScaleMultiplier = 1f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 24, 26);
                Vector2 origin = new Vector2(12, 8);
                float scale = 1.4f;

                if (i == points.Count - 2)
                {
                    // ===== 先端 =====
                    frame.Y = 74;
                    frame.Height = 18;

                    Player owner = Main.player[proj.owner];
                    float progress =
                        1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    scale = MathHelper.Lerp(
                        0.9f,
                        2.2f,
                        Utils.GetLerpValue(0.1f, 0.7f, progress, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, progress, true)
                    );
                }
                else if (i > 0)
                {
                    // ===== 中間 =====
                    int[] frames = { 26, 42, 58 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 16;
                    scale = 1f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;
                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケールを強調
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // 位置更新（★ ここが分離されているので長さが壊れない）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(12, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(12, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(12, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(12, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class SimpleWhipDrawer14
    {
        // ★ 中間②専用スケール倍率（ここだけ触れば見た目調整できる）
        private const float Middle2ScaleMultiplier = 1.25f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 33);
                Vector2 origin = new Vector2(15, -8);
                float scale = 1.4f;

                if (i == points.Count - 2)
                {
                    // ===== 先端 =====
                    frame.Y = 90;
                    frame.Height = 25;

                    Player owner = Main.player[proj.owner];
                    float progress =
                        1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    scale = MathHelper.Lerp(
                        0.5f,
                        1.9f,
                        Utils.GetLerpValue(0.1f, 0.7f, progress, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, progress, true)
                    );
                }
                else if (i > 0)
                {
                    // ===== 中間 =====
                    int[] frames = { 33, 52, 71 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 19;
                    scale = 1f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;
                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケールを強調
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // 位置更新（★ ここが分離されているので長さが壊れない）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class SimpleWhipDrawer15
    {
        // ★ 中間②専用スケール倍率（ここだけ触れば見た目調整できる）
        private const float Middle2ScaleMultiplier = 1.35f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 33);
                Vector2 origin = new Vector2(15, 8);
                float scale = 1.4f;

                if (i == points.Count - 2)
                {
                    // ===== 先端 =====
                    frame.Y = 90;
                    frame.Height = 25;

                    Player owner = Main.player[proj.owner];
                    float progress =
                        1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    scale = MathHelper.Lerp(
                        0.5f,
                        2.4f,
                        Utils.GetLerpValue(0.1f, 0.7f, progress, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, progress, true)
                    );
                }
                else if (i > 0)
                {
                    // ===== 中間 =====
                    int[] frames = { 33, 52, 71 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 19;
                    scale = 1f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;
                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケールを強調
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // 位置更新（★ ここが分離されているので長さが壊れない）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class SimpleWhipDrawer16
    {
        // ★ 中間②専用スケール倍率（ここだけ触れば見た目調整できる）
        private const float Middle2ScaleMultiplier = 1.25f;

        private const string GlowTexturePath =
            "CalamitySimpleWhipAddon/Content/Projectiles/CrackoftheUniverseProj_Glow";

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            Texture2D glowTexture =
                ModContent.Request<Texture2D>(GlowTexturePath).Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 33);
                Vector2 origin = new Vector2(15, -8);
                float scale = 1.82f;

                if (i == points.Count - 2)
                {
                    // ===== 先端 =====
                    frame.Y = 90;
                    frame.Height = 25;

                    Player owner = Main.player[proj.owner];
                    float progress =
                        1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    scale = MathHelper.Lerp(
                        0.65f,
                        2.47f,
                        Utils.GetLerpValue(0.1f, 0.7f, progress, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, progress, true)
                    );
                }
                else if (i > 0)
                {
                    // ===== 中間 =====
                    int[] frames = { 33, 52, 71 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 19;
                    scale = 1.3f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;
                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケールを強調
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // 位置更新（★ ここが分離されているので長さが壊れない）
                pos += diff;
            }

            DrawLayer(normalSegments, texture, flip);
            DrawGlowLayer(normalSegments, glowTexture, flip);
            DrawLayer(prioritySegments, texture, flip);
            DrawGlowLayer(prioritySegments, glowTexture, flip);
            DrawLayer(handleSegments, texture, flip);
            DrawGlowLayer(handleSegments, glowTexture, flip);
            DrawLayer(tipSegments, texture, flip);
            DrawGlowLayer(tipSegments, glowTexture, flip);
        }

        private static void DrawLayer(
            List<SegmentDrawData> segments,
            Texture2D texture,
            SpriteEffects flip
        )
        {
            foreach (var s in segments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }

        private static void DrawGlowLayer(
            List<SegmentDrawData> segments,
            Texture2D glowTexture,
            SpriteEffects flip
        )
        {
            foreach (var s in segments)
            {
                Main.EntitySpriteDraw(
                    glowTexture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    Color.White, // ★ 発光はライト非依存
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class SimpleWhipDrawer17
    {
        // ★ 中間②専用スケール倍率（ここだけ触れば見た目調整できる）
        private const float Middle2ScaleMultiplier = 1.25f;

        private const string GlowTexturePath =
            "CalamitySimpleWhipAddon/Content/Projectiles/RubellusProj_Glow";

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            Texture2D glowTexture =
                ModContent.Request<Texture2D>(GlowTexturePath).Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 33);
                Vector2 origin = new Vector2(15, -8);
                float scale = 1.6f;

                if (i == points.Count - 2)
                {
                    // ===== 先端 =====
                    frame.Y = 90;
                    frame.Height = 25;

                    Player owner = Main.player[proj.owner];
                    float progress =
                        1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    scale = MathHelper.Lerp(
                        0.55f,
                        2.2f,
                        Utils.GetLerpValue(0.1f, 0.7f, progress, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, progress, true)
                    );
                }
                else if (i > 0)
                {
                    // ===== 中間 =====
                    int[] frames = { 33, 52, 71 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 19;
                    scale = 1.1f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;
                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケールを強調
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // 位置更新（★ ここが分離されているので長さが壊れない）
                pos += diff;
            }

            DrawLayer(normalSegments, texture, flip);
            DrawGlowLayer(normalSegments, glowTexture, flip);
            DrawLayer(prioritySegments, texture, flip);
            DrawGlowLayer(prioritySegments, glowTexture, flip);
            DrawLayer(handleSegments, texture, flip);
            DrawGlowLayer(handleSegments, glowTexture, flip);
            DrawLayer(tipSegments, texture, flip);
            DrawGlowLayer(tipSegments, glowTexture, flip);
        }

        private static void DrawLayer(
            List<SegmentDrawData> segments,
            Texture2D texture,
            SpriteEffects flip
        )
        {
            foreach (var s in segments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }

        private static void DrawGlowLayer(
            List<SegmentDrawData> segments,
            Texture2D glowTexture,
            SpriteEffects flip
        )
        {
            foreach (var s in segments)
            {
                Main.EntitySpriteDraw(
                    glowTexture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    Color.White, // ★ 発光はライト非依存
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class SimpleWhipDrawerLo
    {
        // ★ 中間②専用スケール倍率
        private const float Middle2ScaleMultiplier = 1.1f;

        // ★ 発光テクスチャのパス（ここだけ変更すればOK）
        private const string GlowTexturePath =
            "CalamitySimpleWhipAddon/Content/Projectiles/LoadoutProj_Glow";

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            // 通常テクスチャ
            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            // ★ 発光テクスチャ
            Texture2D glowTexture =
                ModContent.Request<Texture2D>(GlowTexturePath).Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 33);
                float scale = 1f;

                if (i == points.Count - 2)
                {
                    // ===== 先端 =====
                    frame.Y = 90;
                    frame.Height = 45;

                    Player owner = Main.player[proj.owner];
                    float progress =
                        1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    scale = MathHelper.Lerp(
                        0.5f,
                        1.7f,
                        Utils.GetLerpValue(0.1f, 0.7f, progress, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, progress, true)
                    );
                }
                else if (i > 0)
                {
                    // ===== 中間 =====
                    int[] frames = { 33, 52, 71 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 19;
                    scale = 0.8f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;
                float rotation = diff.ToRotation() - MathHelper.PiOver2;

                Color lightColor =
                    Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = i == points.Count - 2;
                bool isHandle = i == 0;

                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                if (isMiddle2)
                    scale *= Middle2ScaleMultiplier;

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = lightColor
                };

                if (isTip)
                    tipSegments.Add(data);
                else if (isHandle)
                    handleSegments.Add(data);
                else if (isMiddle2)
                    prioritySegments.Add(data);
                else
                    normalSegments.Add(data);

                pos += diff;
            }

            // ===== 通常描画 =====
            DrawLayer(normalSegments, texture, flip);
            DrawLayer(prioritySegments, texture, flip);
            DrawLayer(handleSegments, texture, flip);
            DrawLayer(tipSegments, texture, flip);

            // ===== 発光描画（色は固定） =====
            DrawGlowLayer(normalSegments, glowTexture, flip);
            DrawGlowLayer(prioritySegments, glowTexture, flip);
            DrawGlowLayer(handleSegments, glowTexture, flip);
            DrawGlowLayer(tipSegments, glowTexture, flip);
        }

        private static void DrawLayer(
            List<SegmentDrawData> segments,
            Texture2D texture,
            SpriteEffects flip
        )
        {
            foreach (var s in segments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }

        private static void DrawGlowLayer(
            List<SegmentDrawData> segments,
            Texture2D glowTexture,
            SpriteEffects flip
        )
        {
            foreach (var s in segments)
            {
                Main.EntitySpriteDraw(
                    glowTexture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    Color.White, // ★ 発光はライト非依存
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }


    public static class SimpleWhipDrawerShi
    {
        // ★ 中間②専用スケール倍率
        private const float Middle2ScaleMultiplier = 1.1f;

        // ★ 発光テクスチャのパス（ここだけ変更すればOK）
        private const string GlowTexturePath =
            "CalamitySimpleWhipAddon/Content/Projectiles/ShieldConduitProj2";

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            Player player = Main.player[proj.owner];
            var modPlayer = player.GetModPlayer<WhipShieldPlayer>();

            bool attackOrb =
                modPlayer.rechargeCooldown > 0 ||
                modPlayer.shieldLife >= modPlayer.MaxShield;


            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            // 通常テクスチャ
            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            // ★ 発光テクスチャ
            Texture2D glowTexture =
                ModContent.Request<Texture2D>(GlowTexturePath).Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 33);
                float scale = 1f;

                if (i == points.Count - 2)
                {
                    // ===== 先端 =====
                    frame.Y = 90;
                    frame.Height = 45;

                    Player owner = Main.player[proj.owner];
                    float progress =
                        1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    scale = MathHelper.Lerp(
                        0.5f,
                        1.7f,
                        Utils.GetLerpValue(0.1f, 0.7f, progress, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, progress, true)
                    );
                }
                else if (i > 0)
                {
                    // ===== 中間 =====
                    int[] frames = { 33, 52, 71 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 19;
                    scale = 0.8f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;
                float rotation = diff.ToRotation() - MathHelper.PiOver2;

                Color lightColor =
                    Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = i == points.Count - 2;
                bool isHandle = i == 0;

                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                if (isMiddle2)
                    scale *= Middle2ScaleMultiplier;

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = lightColor
                };

                if (isTip)
                    tipSegments.Add(data);
                else if (isHandle)
                    handleSegments.Add(data);
                else if (isMiddle2)
                    prioritySegments.Add(data);
                else
                    normalSegments.Add(data);

                pos += diff;
            }

            Color glowColor = attackOrb
                ? Color.Red
                : Color.Cyan;

            // ===== Additive 開始 =====
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.Additive,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix
            );

            // ===== 発光描画（Additive） =====
            DrawGlowLayer(normalSegments, glowTexture, flip, glowColor);
            DrawGlowLayer(prioritySegments, glowTexture, flip, glowColor);
            DrawGlowLayer(handleSegments, glowTexture, flip, glowColor);
            DrawGlowLayer(tipSegments, glowTexture, flip, glowColor);


            // ===== 通常に戻す =====
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix
                );

            // ===== 通常描画 =====
            DrawLayer(normalSegments, texture, flip);
            DrawLayer(prioritySegments, texture, flip);
            DrawLayer(handleSegments, texture, flip);
            DrawLayer(tipSegments, texture, flip);

            
        }

        private static void DrawLayer(
            List<SegmentDrawData> segments,
            Texture2D texture,
            SpriteEffects flip
        )
        {
            foreach (var s in segments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }

        private static void DrawGlowLayer(
            List<SegmentDrawData> segments,
            Texture2D glowTexture,
            SpriteEffects flip,
            Color glowColor
        )
        {
            foreach (var s in segments)
            {
                Main.EntitySpriteDraw(
                    glowTexture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    glowColor,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }

    }


    public static class RapierWhipDrawer1
	{
		public static void DrawLine(
	List<Vector2> points,
	Func<Vector2, Color> getColor
)
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new Vector2(frame.Width / 2, 2);

			Vector2 pos = points[0];

			for (int i = 0; i < points.Count - 1; i++)
			{
				Vector2 element = points[i];
				Vector2 diff = points[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;

				Color color = getColor(points[i]) * Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

				Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(
					texture,
					pos - Main.screenPosition,
					frame,
					color,
					rotation,
					origin,
					scale,
					SpriteEffects.None,
					0
				);

				pos += diff;
			}
		}

		public static void DrawSegments(
			Projectile proj,
			List<Vector2> points,
			float timer
		)
		{
			SpriteEffects flip = proj.spriteDirection < 0
				? SpriteEffects.None
				: SpriteEffects.FlipHorizontally;

			Texture2D texture = TextureAssets.Projectile[proj.type].Value;

			Vector2 pos = points[0];

			for (int i = 0; i < points.Count - 1; i++)
			{
				Rectangle frame = new Rectangle(0, 0, 10, 26);
				Vector2 origin = new Vector2(5, 8);
				float scale = 1.4f;

				if (i == points.Count - 2)
				{
					frame.Y = 74;
					frame.Height = 18;

					Projectile.GetWhipSettings(proj, out float timeToFlyOut, out int _, out float _);
					float t = timer / timeToFlyOut;

					scale = 1.5f;
				}
				else if (i > 10)
				{
					frame.Y = 58;
					frame.Height = 16;
				}
				else if (i > 5)
				{
					frame.Y = 42;
					frame.Height = 16;
				}
				else if (i > 0)
				{
					frame.Y = 26;
					frame.Height = 16;
				}

				Vector2 element = points[i];
				Vector2 diff = points[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(element.ToTileCoordinates());

				Main.EntitySpriteDraw(
					texture,
					pos - Main.screenPosition,
					frame,
					color,
					rotation,
					origin,
					scale,
					flip,
					0
				);

				pos += diff;
			}
		}
	}

    public static class RapierWhipDrawer2
    {
        public static void DrawLine(
    List<Vector2> points,
    Func<Vector2, Color> getColor
)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;

                Color color = getColor(points[i]) * Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );

                pos += diff;
            }
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 20, 26);
                Vector2 origin = new Vector2(10, 8);
                float scale = 1.4f;

                if (i == points.Count - 2)
                {
                    frame.Y = 74;
                    frame.Height = 18;

                    Projectile.GetWhipSettings(proj, out float timeToFlyOut, out int _, out float _);
                    float t = timer / timeToFlyOut;

                    scale = MathHelper.Lerp(0.5f, 2.4f,
                        Utils.GetLerpValue(0.1f, 0.7f, t, true) *
                        Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 10)
                {
                    frame.Y = 58;
                    frame.Height = 16;
                }
                else if (i > 5)
                {
                    frame.Y = 42;
                    frame.Height = 16;
                }
                else if (i > 0)
                {
                    frame.Y = 26;
                    frame.Height = 16;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    flip,
                    0
                );

                pos += diff;
            }
        }
    }

    public static class RapierWhipDrawer3
    {
        // ★ 中間②だけ拡大する倍率
        private const float Middle2ScaleMultiplier = 1f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawLine(
            List<Vector2> points,
            Func<Vector2, Color> getColor
        )
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color =
                    getColor(points[i]) *
                    Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );

                pos += diff;
            }
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 20, 26);
                Vector2 origin = new Vector2(10, 8);
                float scale = 1.8f;

                // ===== 先端 =====
                if (i == points.Count - 2)
                {
                    frame.Y = 74;
                    frame.Height = 10;

                    Projectile.GetWhipSettings(
                        proj,
                        out float timeToFlyOut,
                        out int _,
                        out float _
                    );

                    float t = timer / timeToFlyOut;
                    scale = 1.4f;
                }
                // ===== 中間 =====
                else if (i > 0)
                {
                    int[] frames = { 26, 42, 58 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 16;
                    scale = 1.4f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定（3枚ループの2枚目）
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケール変更
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // ★ 位置更新（ここは分離しないのが重要）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（※先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class RapierWhipDrawer4
    {
        // ★ 中間②専用スケール倍率（ここだけ触れば見た目調整できる）
        private const float Middle2ScaleMultiplier = 1.3f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 30, 33);
                Vector2 origin = new Vector2(15, 8);
                float scale = 1.5f;

                if (i == points.Count - 2)
                {
                    // ===== 先端 =====
                    frame.Y = 109;
                    frame.Height = 45;

                    Player owner = Main.player[proj.owner];
                    float progress =
                        1f - owner.itemAnimation / (float)owner.itemAnimationMax;

                    scale = 1.56f;
                }
                else if (i > 0)
                {
                    // ===== 中間 =====
                    int[] frames = { 33, 52, 71, 90 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 19;
                    scale = 1.2f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;
                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 4 == 1);

                // ★ 中間②だけスケールを強調
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                Vector2 drawPos = pos;

                if (isHandle)
                {
                    // ★ 回転方向に沿って「後ろ」にずらす
                    Vector2 backOffset = diff.SafeNormalize(Vector2.UnitY) * -10f;
                    drawPos += backOffset;
                }

                SegmentDrawData data = new()
                {
                    Position = drawPos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // 位置更新（★ ここが分離されているので長さが壊れない）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(15, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class RapierWhipDrawer5
    {
        // ★ 中間②だけ拡大する倍率
        private const float Middle2ScaleMultiplier = 1f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawLine(
            List<Vector2> points,
            Func<Vector2, Color> getColor
        )
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color =
                    getColor(points[i]) *
                    Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );

                pos += diff;
            }
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 20, 26);
                Vector2 origin = new Vector2(10, 8);
                float scale = 1.4f;

                // ===== 先端 =====
                if (i == points.Count - 2)
                {
                    frame.Y = 74;
                    frame.Height = 28;

                    Projectile.GetWhipSettings(
                        proj,
                        out float timeToFlyOut,
                        out int _,
                        out float _
                    );

                    float t = timer / timeToFlyOut;
                    scale = 1.4f;
                }
                // ===== 中間 =====
                else if (i > 0)
                {
                    int[] frames = { 26, 42, 58 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 16;
                    scale = 1.4f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定（3枚ループの2枚目）
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケール変更
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // ★ 位置更新（ここは分離しないのが重要）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（※先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class RapierWhipDrawer6
    {
        // ★ 中間②だけ拡大する倍率
        private const float Middle2ScaleMultiplier = 0.8f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawLine(
            List<Vector2> points,
            Func<Vector2, Color> getColor
        )
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color =
                    getColor(points[i]) *
                    Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );

                pos += diff;
            }
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 20, 26);
                Vector2 origin = new Vector2(10, 8);
                float scale = 1.4f;

                // ===== 先端 =====
                if (i == points.Count - 2)
                {
                    frame.Y = 74;
                    frame.Height = 28;

                    Projectile.GetWhipSettings(
                        proj,
                        out float timeToFlyOut,
                        out int _,
                        out float _
                    );

                    float t = timer / timeToFlyOut;
                    scale = 1.8f;
                }
                // ===== 中間 =====
                else if (i > 0)
                {
                    int[] frames = { 26, 42, 58 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 16;
                    scale = 1.4f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定（3枚ループの2枚目）
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケール変更
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // ★ 位置更新（ここは分離しないのが重要）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（※先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class RapierWhipDrawer7
    {
        // ★ 中間②だけ拡大する倍率
        private const float Middle2ScaleMultiplier = 1.4f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawLine(
            List<Vector2> points,
            Func<Vector2, Color> getColor
        )
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color =
                    getColor(points[i]) *
                    Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );

                pos += diff;
            }
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 20, 26);
                Vector2 origin = new Vector2(10, 8);
                float scale = 1.8f;

                // ===== 先端 =====
                if (i == points.Count - 2)
                {
                    frame.Y = 74;
                    frame.Height = 10;

                    Projectile.GetWhipSettings(
                        proj,
                        out float timeToFlyOut,
                        out int _,
                        out float _
                    );

                    float t = timer / timeToFlyOut;
                    scale = 1.96f;
                }
                // ===== 中間 =====
                else if (i > 0)
                {
                    int[] frames = { 26, 42, 58 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 16;
                    scale = 1.4f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定（3枚ループの2枚目）
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケール変更
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // ★ 位置更新（ここは分離しないのが重要）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（※先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class RapierWhipDrawer8
    {
        // ★ 中間②だけ拡大する倍率
        private const float Middle2ScaleMultiplier = 1.5f;

        private struct SegmentDrawData
        {
            public Vector2 Position;
            public float Rotation;
            public Rectangle Frame;
            public float Scale;
            public Color Color;
        }

        public static void DrawLine(
            List<Vector2> points,
            Func<Vector2, Color> getColor
        )
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color =
                    getColor(points[i]) *
                    Lighting.GetColor(points[i].ToTileCoordinates()).ToVector3().Length();

                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(
                    texture,
                    pos - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0
                );

                pos += diff;
            }
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            SpriteEffects flip = proj.spriteDirection < 0
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            List<SegmentDrawData> normalSegments = new();
            List<SegmentDrawData> prioritySegments = new();
            List<SegmentDrawData> handleSegments = new();
            List<SegmentDrawData> tipSegments = new();

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Rectangle frame = new Rectangle(0, 0, 20, 26);
                Vector2 origin = new Vector2(10, 8);
                float scale = 1.4f;

                // ===== 先端 =====
                if (i == points.Count - 2)
                {
                    frame.Y = 74;
                    frame.Height = 28;

                    Projectile.GetWhipSettings(
                        proj,
                        out float timeToFlyOut,
                        out int _,
                        out float _
                    );

                    float t = timer / timeToFlyOut;
                    scale = 2.1f;
                }
                // ===== 中間 =====
                else if (i > 0)
                {
                    int[] frames = { 26, 42, 58 };
                    frame.Y = frames[i % frames.Length];
                    frame.Height = 16;
                    scale = 1.4f;
                }

                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                bool isTip = (i == points.Count - 2);
                bool isHandle = (i == 0);

                // ★ 中間②判定（3枚ループの2枚目）
                bool isMiddle2 =
                    i > 0 &&
                    i != points.Count - 2 &&
                    (i % 3 == 1);

                // ★ 中間②だけスケール変更
                if (isMiddle2)
                {
                    scale *= Middle2ScaleMultiplier;
                }

                SegmentDrawData data = new()
                {
                    Position = pos,
                    Rotation = rotation,
                    Frame = frame,
                    Scale = scale,
                    Color = color
                };

                // ★ レイヤー優先
                if (isTip)
                {
                    tipSegments.Add(data);          // ★ 先端：最前面
                }
                else if (isHandle)
                {
                    handleSegments.Add(data);       // ★ 持ち手：先端の次
                }
                else if (isMiddle2)
                {
                    prioritySegments.Add(data);     // ★ 中間②
                }
                else
                {
                    normalSegments.Add(data);       // 通常
                }

                // ★ 位置更新（ここは分離しないのが重要）
                pos += diff;
            }

            // ===== 通常セグメント描画 =====
            foreach (var s in normalSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            // ===== 中間②だけ最前面（※先端よりは下） =====
            foreach (var s in prioritySegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in handleSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }

            foreach (var s in tipSegments)
            {
                Main.EntitySpriteDraw(
                    texture,
                    s.Position - Main.screenPosition,
                    s.Frame,
                    s.Color,
                    s.Rotation,
                    new Vector2(10, 8),
                    s.Scale,
                    flip,
                    0
                );
            }
        }
    }

    public static class SimpleWhipSmoothDrawer1
    {

        public static void DrawWhipSmooth(
            Projectile proj,
            List<Vector2> points)
        {
            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            Rectangle frame = texture.Frame();

            // テクスチャの左端中央を基準にする
            Vector2 origin = new Vector2(0f, frame.Height * 0.5f);

            SpriteEffects flip =
                proj.spriteDirection < 0 ?
                SpriteEffects.None :
                SpriteEffects.FlipHorizontally;

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 start = points[i];
                Vector2 end = points[i + 1];

                Vector2 diff = end - start;

                float length = diff.Length();
                float rotation = diff.ToRotation();

                Main.EntitySpriteDraw(
                    texture,
                    start - Main.screenPosition,
                    frame,
                    Lighting.GetColor(start.ToTileCoordinates()),
                    rotation,
                    origin,
                    new Vector2(length / frame.Width, 1f),
                    flip,
                    0);
            }
        }
    }


}