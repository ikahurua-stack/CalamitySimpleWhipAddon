using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using System;
using CalamityMod.Graphics.Primitives;
using static CalamityMod.CalamityUtils;
using CalamityMod.Graphics.Metaballs;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class ResonantVoidProj : ModProjectile
    {

        // メタボールの生成間隔制御用タイマー
        private float particleTimer;

        // 前フレームのムチ先端位置（距離補間用）
        private Vector2? lastTipPos = null;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 77;
            Projectile.WhipSettings.RangeMultiplier = 5.4f;
            Projectile.ArmorPenetration = 9999;

            Projectile.DamageType = DamageClass.Summon;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuffEx2>(), 240);
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuff15D15C>(), 240);

            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.98f);
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            particleTimer++;

            // ムチの制御点（軌跡）を取得
            List<Vector2> points = new();
            Projectile.FillWhipControlPoints(Projectile, points);

            if (points.Count < 2)
                return;

            // ムチの先端位置
            Vector2 tip = points[^1];
            // 先端直前の点
            Vector2 prev = points[^2];

            // ムチの進行方向（先端が向いている向き）
            Vector2 dir = Vector2.Normalize(tip - prev);

            Vector2 backDir = -dir;

            float directionBlend = 0.65f;

            Vector2 blendedDir = Vector2.Lerp(dir, -dir, directionBlend);
            blendedDir.Normalize();

            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            float swingProgress = Projectile.ai[0] / swingTime;

            // サイズ用カーブ（振り抜き専用）
            float sizeFactor =
                Utils.GetLerpValue(0.4f, 0.8f, swingProgress, true);

            // 最終サイズ
            float metaballSize =
                MathHelper.Lerp(0.05f, 150f, sizeFactor);

            float spawnFactor =
                Utils.GetLerpValue(0f, 0.95f, swingProgress, true);

            int spawnChance =
                (int)MathHelper.Lerp(1f, 4f, spawnFactor);

            // プレイヤー付近ではエフェクトを出さない
            // → 根元がうるさくならないようにするため
            if (Vector2.Distance(tip, owner.Center) < 70f)
                return;

            // ==================================================
            // ▼ メタボール生成（距離補間方式）
            // ==================================================

            if (Vector2.Distance(tip, owner.Center) < 70f)
            {
                lastTipPos = tip;
                return;
            }

            if (lastTipPos.HasValue)
            {
                Vector2 prevTip = lastTipPos.Value;
                Vector2 delta = tip - prevTip;
                float distance = delta.Length();

                if (distance > 0.1f)
                {
                    Vector2 moveDir = Vector2.Normalize(delta);

                    // 密度：小さいほど隙間が埋まる
                    float spacing = 40f;
                    int count = (int)(distance / spacing);

                    for (int i = 0; i <= count; i++)
                    {
                        float t = i / (float)(count + 1);
                        Vector2 spawnPos = Vector2.Lerp(prevTip, tip, t);

                        // 進行方向と逆に少し押し戻す
                        spawnPos += backDir * 18f;

                        // ===============================
                        // ▼ メイン生成①（元のまま）
                        // ===============================
                        if (Main.rand.NextBool(1))
                        {
                            Vector2 velocity =
                                blendedDir.RotatedByRandom(MathHelper.ToRadians(20f)) *
                                Main.rand.NextFloat(1.5f, 2.5f);

                            VoidGeneratorMetaball.SpawnParticle(
                                spawnPos + Main.rand.NextVector2Circular(10f, 20f),
                                velocity,
                                metaballSize * Main.rand.NextFloat(0.65f, 1.4f)
                            );
                        }

                    }
                }
            }

            if (Main.rand.NextBool(1))
            {
                Vector2 velocity =
                    blendedDir.RotatedByRandom(MathHelper.ToRadians(20f)) *
                    Main.rand.NextFloat(1.5f, 2.5f);

                VoidGeneratorMetaball.SpawnParticle(
                    tip,
                    velocity,
                    metaballSize * Main.rand.NextFloat(0.65f, 1.4f)
                );
            }

            if (Main.rand.NextBool(1) && particleTimer > 10f)
            {
                Vector2 velocity =
                    blendedDir.RotatedByRandom(MathHelper.ToRadians(50f)) *
                    Main.rand.NextFloat(-5.5f, 8.5f);

                VoidGeneratorMetaball.SpawnParticle(
                    tip,
                    velocity * 1.2f,
                    Main.rand.NextFloat(10f, 20f)
                );
            }

            // 次フレーム用に保存
            lastTipPos = tip;

            // 光の演出（メタボールとは直接関係なし）
            foreach (var p in points)
            {
                Lighting.AddLight(p, 0.8f, 0.0f, 0.75f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> pts = new();
            Projectile.FillWhipControlPoints(Projectile, pts);

            SimpleWhipDrawer8.DrawSegments(
                Projectile,
                pts,
                Projectile.ai[0]
            );

            return false;
        }
    }
}

