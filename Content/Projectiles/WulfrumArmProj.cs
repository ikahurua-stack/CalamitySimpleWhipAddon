using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalamitySimpleWhipAddon.Content.Buffs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class WulfrumArmProj : ModProjectile
    {
        private const string GlowTexturePath =
            "CalamitySimpleWhipAddon/Content/Projectiles/WulfrumArmProj_Glow";


        private const int SoundInterval = 30;

        private int SoundTimer = 0;

        private readonly List<int> hookedItems = new();

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 2;
            Projectile.WhipSettings.RangeMultiplier = 0.3f;
            Projectile.extraUpdates = 2;
        }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuffEx19>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.75f);
        }

        public override void AI()
        {
            base.AI();

            Player owner = Main.player[Projectile.owner];
            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            float swingProgress = Projectile.ai[0] / swingTime;
            bool canSound = swingProgress > 0.15f;

            if (canSound)
            {
                SoundTimer++;

                if (SoundTimer >= SoundInterval)
                {
                    SoundEngine.PlaySound(
                        SoundID.Item108 with { Pitch = Main.rand.NextFloat(0.8f, 1.1f), Volume = 2.8f },
                        Projectile.Center
                    );

                    SoundTimer = 0;
                }
            }
            else
            {
                SoundTimer = 0;
            }

            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);

            Vector2 tip = points[points.Count - 1];
            Vector2 prev = points[points.Count - 2];

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }

            for (int i = 0; i < Main.maxItems; i++)
            {
                Item item = Main.item[i];

                if (!item.active || item.IsAir)
                    continue;

                if (Vector2.Distance(item.Center, tip) < 24f)
                {
                    if (!hookedItems.Contains(i))
                        hookedItems.Add(i);
                }
            }

            for (int j = hookedItems.Count - 1; j >= 0; j--)
            {
                int index = hookedItems[j];

                Item item = Main.item[index];

                if (!item.active || item.IsAir)
                {
                    hookedItems.RemoveAt(j);
                    continue;
                }

                item.Center = tip;
                item.velocity = Vector2.Zero;
            }

            if (Main.rand.NextBool(6))
            {
                Vector2 dir = tip - prev;
                if (dir != Vector2.Zero)
                    dir.Normalize();
                else
                    dir = Vector2.UnitX;

                int dust = Dust.NewDust(
                    tip,
                    4,
                    4,
                    DustID.PortalBoltTrail);

                Main.dust[dust].noGravity = false;
                Main.dust[dust].color = Color.GreenYellow;

                Main.dust[dust].scale =
                    Main.rand.NextFloat(
                        0.3f,
                        0.5f);


                Main.dust[dust].velocity =
                    dir * Main.rand.NextFloat(1f, 3f);

                Main.dust[dust].fadeIn = 0.5f;
            }

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 pos = points[i];
                Lighting.AddLight(pos, 0.15f, 0.25f, 0.025f);
            }
            Lighting.AddLight(tip, 0.375f, 0.5f, 0.1f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);

            

            DrawSegments(Projectile, points, Timer);

            return false;
        }

        public static void DrawSegments(
            Projectile proj,
            List<Vector2> points,
            float timer
        )
        {
            Player player = Main.player[proj.owner];
            SpriteEffects flip = player.direction < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Texture2D texture = TextureAssets.Projectile[proj.type].Value;

            Texture2D glowTexture =
                ModContent.Request<Texture2D>(GlowTexturePath).Value;

            float baseScale = 1.25f;


            for (int i = 0; i < points.Count; i++)
            {
                Vector2 currentPoint = points[i];
                Color color = Lighting.GetColor(currentPoint.ToTileCoordinates());

                Vector2 diff = Vector2.Zero;
                float rotation = 0f;

                if (i < points.Count - 1)
                {
                    diff = points[i + 1] - currentPoint;
                    rotation = diff.ToRotation() - MathHelper.PiOver2;
                }
                else if (points.Count > 1)
                {
                    diff = currentPoint - points[i - 1];
                    rotation = diff.ToRotation() - MathHelper.PiOver2;
                }

                // --- ① 各セグメント（金属パーツ）の描画設定 ---
                Rectangle frame = new Rectangle(0, 0, 16, 26);
                Vector2 origin = new Vector2(8, frame.Height / 2f);

                if (i == 0) // 持ち手
                {
                    frame.Y = 0;
                    frame.Height = 26;
                    origin = new Vector2(8, frame.Height / 2f);
                }
                else if (i == points.Count - 1)
                {
                    const int tipFrameHeight = 25;
                    const int tipFrameStartY = 75;

                    frame = new Rectangle(
                        0,
                        tipFrameStartY + tipFrameHeight * proj.frame,
                        16,
                        tipFrameHeight
                    );

                    origin = new Vector2(8, 4);
                    baseScale = 1.65f;
                }
                else // 中間セグメント
                {
                    frame.Y = 59;
                    frame.Height = 16;
                    origin = new Vector2(8, frame.Height / 2f);
                }

                // --- ② セグメント間を繋ぐ「伸縮する身（シャフト）」の描画 ---
                if (i < points.Count - 1)
                {
                    Rectangle connectionFrame = new Rectangle(0, 26, 16, 33);
                    Vector2 connectionOrigin = new Vector2(connectionFrame.Width / 2f, 0f);

                    float actualDistance = diff.Length();
                    Vector2 connectionScale = new Vector2(
                        baseScale,
                        actualDistance / connectionFrame.Height
                    );

                    Main.EntitySpriteDraw(
                        texture,
                        currentPoint - Main.screenPosition,
                        connectionFrame,
                        color,
                        rotation,
                        connectionOrigin,
                        connectionScale,
                        flip,
                        0
                    );
                }

                // セグメント本体の描画
                Main.EntitySpriteDraw(
                    texture,
                    currentPoint - Main.screenPosition,
                    frame,
                    color,
                    rotation,
                    origin,
                    baseScale,
                    flip,
                    0
                );

                Main.EntitySpriteDraw(
                    glowTexture,
                    currentPoint - Main.screenPosition,
                    frame,
                    Color.White,
                    rotation,
                    origin,
                    baseScale,
                    flip,
                    0
                );
            }
        }
    }
}
