using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Projectiles.Typeless;
using Terraria.DataStructures;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class SatelliteTag : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/StarProj";

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.alpha = 0;
            Projectile.scale = Main.rand.NextFloat(0.25f, 0.6f);

            // ミニオン攻撃扱い（維持）
            Projectile.DamageType = DamageClass.Summon;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            // 親だけが分裂
            if (Projectile.ai[1] == 0f)
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(
                        source,
                        Projectile.Center,
                        Vector2.Zero,
                        Projectile.type,
                        0,
                        0f,
                        Projectile.owner,
                        0f,
                        1f // ← 子フラグ
                    );
                }
            }

            // 初速
            Projectile.velocity =
                Main.rand.NextVector2Unit() *
                Main.rand.NextFloat(3f, 8f);

            Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            Projectile.localAI[0] = Main.rand.NextFloat(-0.35f, 0.35f);
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.localAI[0];
            Projectile.velocity *= 0.85f;
            Projectile.velocity += Main.rand.NextVector2Circular(0.04f, 0.04f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex =
                Terraria.GameContent.TextureAssets.Projectile[
                    ModContent.ProjectileType<BlazingStarHeal>()
                ].Value;

            Vector2 pos = Projectile.Center - Main.screenPosition;
            Vector2 origin = tex.Size() * 0.5f;

            float progress = 1f - Projectile.timeLeft / 30f;
            progress = MathHelper.Clamp(progress, 0f, 1f);

            float time = Main.GlobalTimeWrappedHourly * 1.2f;

            // ==== Luminite 風色循環 ====
            Color lumRed = new Color(255, 80, 80);
            Color lumGreen = new Color(50, 255, 220);
            Color lumWhite = new Color(100, 100, 100);

            float cycle =
                (float)Math.Sin(time * MathHelper.TwoPi) * 0.5f + 0.5f;

            Color baseColor =
                Color.Lerp(
                    Color.Lerp(lumRed, lumGreen, cycle),
                    lumWhite,
                    cycle * 0.6f
                );

            // ==== フェード & 強発光 ====
            float fadeIn = Utils.GetLerpValue(0f, 0.2f, progress, true);
            float fadeOut = Utils.GetLerpValue(1f, 0.75f, progress, true);

            float pulse =
                1f + 0.35f *
                (float)Math.Cos(Main.GlobalTimeWrappedHourly * 6f);

            float alpha = fadeIn * fadeOut * pulse;

            Color core = Color.Lerp(baseColor, Color.White, 0.45f) * (alpha * 1.2f);
            Color outer = baseColor * alpha;

            Vector2 scaleA = new Vector2(0.5f, 1f) * Projectile.scale;
            Vector2 scaleB = new Vector2(0.5f, 1f) * Projectile.scale;

            // ==== 外側 ====
            Main.EntitySpriteDraw(tex, pos, null, outer,
                Projectile.rotation, origin, scaleA, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(tex, pos, null, outer,
                Projectile.rotation + MathHelper.PiOver2, origin, scaleB, SpriteEffects.None, 0);

            // ==== コア ====
            Main.EntitySpriteDraw(tex, pos, null, core,
                Projectile.rotation + MathHelper.PiOver4, origin, scaleA * 0.6f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(tex, pos, null, core,
                Projectile.rotation + MathHelper.PiOver4 * 3f, origin, scaleB * 0.6f, SpriteEffects.None, 0);

            // ==== ★ 追加：発光ブラー ====
            Main.EntitySpriteDraw(
                tex,
                pos,
                null,
                Color.White * (alpha * 0.35f),
                Projectile.rotation,
                origin,
                Projectile.scale * 1.6f,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}
