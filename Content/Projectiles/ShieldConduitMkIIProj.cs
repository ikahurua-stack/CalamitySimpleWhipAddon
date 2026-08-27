using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using CalamitySimpleWhipAddon.Content.Common.Players;
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


    public class ShieldConduitMkIIProj : ModProjectile
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
            Projectile.WhipSettings.Segments = 45;
            Projectile.WhipSettings.RangeMultiplier = 1.6f;
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
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuffS2>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.7f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);

            SimpleWhipDrawerShi.DrawSegments(Projectile, points, Timer);

            return false;
        }

        public override void AI()
        {
            base.AI();

            Player owner = Main.player[Projectile.owner];
            var modPlayer = owner.GetModPlayer<WhipShieldPlayer>();

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

            // ===== attackOrb 条件 =====
            bool attackOrb =
                modPlayer.rechargeCooldown > 0 ||
                modPlayer.shieldLife >= modPlayer.MaxShield;

            Color particleColor = attackOrb ? Color.Red : Color.Cyan;

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

                var p = new TechyHoloysquareParticle(
                    spawnPos,
                    vel,
                    MathHelper.Lerp(1.2f, 2.5f, tDist),
                    particleColor,
                    Main.rand.Next(18, 28)
                );

                GeneralParticleHandler.SpawnParticle(p);
            }


            // ===== 次フレーム用 =====
            oldTip = tip;
        }

    }
}