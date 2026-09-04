using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using CalamitySimpleWhipAddon.Content.Common.GlobalNPCs;
using CalamityMod.Particles;
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


    public class CrackoftheUniverseProj : ModProjectile
    {
        private Vector2 oldTip;
        private bool tipInitialized = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 75;
            Projectile.WhipSettings.RangeMultiplier = InfernalEclipseCompatibility.IsEnabled ? 2.7f : 3.45f;
        }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private float ChargeTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }



        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuffEx17>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.96f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);

            SimpleWhipDrawer16.DrawSegments(Projectile, points, Timer);

            return false;
        }

        public override void AI()
        {
            base.AI();

            Player owner = Main.player[Projectile.owner];

            // ===== ムチの先端取得 =====
            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);

            if (points.Count < 2)
                return;

            Vector2 tip = points[points.Count - 1];
            Vector2 prev = points[points.Count - 2];

            // ===== 初期化 =====
            if (!tipInitialized)
            {
                oldTip = tip;
                tipInitialized = true;
            }

            // ===== 先端方向 =====
            Vector2 dir = tip - prev;
            if (dir != Vector2.Zero)
                dir.Normalize();
            else
                dir = Vector2.UnitX;

            // ===== プレイヤー付近では出さない =====
            float distanceToPlayer = Vector2.Distance(tip, owner.MountedCenter);
            if (distanceToPlayer < 50f)
            {
                oldTip = tip;
                return;
            }


            // ===== プレイヤー距離 =====
            float dist = Vector2.Distance(tip, owner.MountedCenter);

            // 調整用距離
            float minDist = 50f;
            float maxDist = 350f;

            // 0～1に正規化
            float tDist = Utils.GetLerpValue(minDist, maxDist, dist, true);

            // ===== フレーム補間 =====
            float moveDist = Vector2.Distance(oldTip, tip);

            // 距離に応じて生成数変化
            int steps = (int)(moveDist / 30f * tDist) + 1;
            steps = Math.Min(steps, 25);

            // 距離に応じて拡散半径変化
            float spreadRadius = MathHelper.Lerp(4f, 25f, tDist);

            // 距離に応じて速度変化（外側ほど速く）
            float speedMin = MathHelper.Lerp(1f, 5f, tDist);
            float speedMax = MathHelper.Lerp(3f, 10f, tDist);

            // ===== 移動方向 =====
            Vector2 moveDir = tip - oldTip;
            if (moveDir != Vector2.Zero)
                moveDir.Normalize();
            else
                moveDir = Vector2.UnitX;

            for (int i = 0; i < steps; i++)
            {
                float t = i / (float)steps;
                Vector2 interpPos = Vector2.Lerp(oldTip, tip, t);

                // ===== 周囲ランダム（距離依存）=====
                Vector2 randomOffset =
                    Main.rand.NextVector2Circular(spreadRadius, spreadRadius);

                Vector2 spawnPos = interpPos + randomOffset;

                // ===== 逆方向噴出 =====
                Vector2 vel =
                    -moveDir * Main.rand.NextFloat(speedMin, speedMax);

                Particle spark = new CustomSpark(
                            spawnPos,
                            vel,
                            "CalamityMod/Particles/ProvidenceMarkParticle",
                            false,
                            40, // 寿命（キレよく消える）
                            Main.rand.NextFloat(0.6f, 1.4f), // スケール
                            Main.rand.NextBool() ? Color.Fuchsia : Color.Cyan,
                            new Vector2(1.4f, 0.4f), // 進行方向に長く伸びる形状
                            true,
                            false,
                            0,
                            false,
                            false,
                            Main.rand.NextFloat(0.4f, 0.5f)
                        );

                GeneralParticleHandler.SpawnParticle(spark);

                Dust dust = Dust.NewDustPerfect(spawnPos + new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-15f, 15f)) - (vel * 1.2f), Main.rand.NextBool(8) ? 180 : 295, vel.RotatedByRandom(MathHelper.ToRadians(10f)) * Main.rand.NextFloat(0.1f, 0.8f), 0, default, Main.rand.NextFloat(1.8f, 2.8f));
                
                dust.noGravity = dust.type == 180 ? false : true;
                dust.fadeIn = 0.5f;
                if (dust.type == 180)
                {
                    dust.scale = Main.rand.NextFloat(0.8f, 1.2f);
                    dust.velocity += new Vector2(0, -2.5f) * Main.rand.NextFloat(0.8f, 1.2f);
                }

                Dust dust2 = Dust.NewDustPerfect(spawnPos + Main.rand.NextVector2Circular(6, 6) - vel * 2, DustID.FrostStaff);
                dust2.velocity = vel * Main.rand.NextFloat(0.6f, 1.4f);
                dust2.scale = Main.rand.NextFloat(0.9f, 1.4f);
                dust2.noGravity = true;
            }

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 pos = points[i];
                Lighting.AddLight(pos, (Main.rand.NextBool() ? Color.Fuchsia.ToVector3() : Color.Cyan.ToVector3()) * 0.5f);
            }
            Lighting.AddLight(tip, (Main.rand.NextBool() ? Color.Fuchsia.ToVector3() : Color.Cyan.ToVector3()) * 0.7f);

            // ===== 次フレーム用 =====
            oldTip = tip;
        }

        

    }
}
