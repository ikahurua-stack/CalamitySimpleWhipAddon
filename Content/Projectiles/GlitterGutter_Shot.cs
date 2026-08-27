using CalamityMod.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class GlitterGutter_Shot : ModProjectile
    {
        private int state;
        private int stateTimer;
        private float spinSpeed = 0f;
        private bool didBoost = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = 1; // 再ヒット時に消える
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;

            Projectile.alpha = 50;
            Projectile.ArmorPenetration = 10;

            Projectile.scale = 0.5f;

            Projectile.DamageType = DamageClass.Summon;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.velocity != Vector2.Zero)
                Projectile.rotation = Projectile.velocity.ToRotation();

            // ★ 生成直後はヒット不可にする
            Projectile.localAI[0] = 1f;

            Projectile.netUpdate = true;
        }

        public override void AI()
        {
            stateTimer++;

            int targetIndex = (int)Projectile.ai[0];
            NPC target = null;
            if (targetIndex >= 0 && targetIndex < Main.maxNPCs)
                target = Main.npc[targetIndex];

            // ===== フェーズ0：外向き =====
            if (state == 0)
            {
                // 初期スピン設定
                if (stateTimer == 1)
                    spinSpeed = 2.5f;

                // 徐々に減速させる
                spinSpeed *= 0.96f;
                Projectile.rotation += spinSpeed;

                // ほんの少し減速
                Projectile.velocity *= 0.98f;

                if (stateTimer > 30) // 少し長めに
                {
                    state = 1;
                    stateTimer = 0;
                }
            }

            // ===== フェーズ1：停止準備 =====
            else if (state == 1)
            {
                // 急ブレーキ
                Projectile.velocity *= 0.85f;

                // 回転も減速
                spinSpeed *= 0.75f;
                Projectile.rotation += spinSpeed;

                if (target != null && target.active)
                {
                    Vector2 dir =
                        (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX);

                    // だんだん敵方向へ向く
                    float targetRot = dir.ToRotation();
                    Projectile.rotation =
                        MathHelper.Lerp(Projectile.rotation, targetRot, 0.15f);
                }

                if (stateTimer > 50)
                {
                    state = 2;
                    stateTimer = 0;
                }
            }

            // ===== フェーズ2：追尾 =====
            else if (state == 2)
            {
                if (target != null && target.active && !target.dontTakeDamage)
                {
                    Vector2 toTarget =
                        (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);

                    float baseSpeed = 16f;

                    // ★ 追尾開始瞬間ブースト
                    if (!didBoost)
                    {
                        Projectile.velocity = toTarget * 28f;

                        // 🔵 リング波紋
                        int ringCount = 18;

                        for (int i = 0; i < ringCount; i++)
                        {
                            float angle = MathHelper.TwoPi * i / ringCount;
                            Vector2 offset = angle.ToRotationVector2();

                            Dust dust = Dust.NewDustPerfect(
                                Projectile.Center,
                                DustID.PurpleCrystalShard,
                                offset * 4f
                            );

                            dust.noGravity = true;
                            dust.scale = 1.5f;
                        }

                        SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
                        didBoost = true;
                    }


                    Projectile.velocity =
                        Vector2.Lerp(Projectile.velocity, toTarget * baseSpeed, 0.18f);

                    Projectile.rotation = toTarget.ToRotation();
                }

                Projectile.localAI[0] = 0f;
            }

            // 光（少し控えめに）
            Lighting.AddLight(Projectile.Center, 0.6f, 0f, 0.9f);

            // ダスト
            if (Main.rand.NextBool(6))
            {
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.PurpleCrystalShard
                );

                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Projectile.velocity * 0.2f;
            }
        }


        public override bool? CanHitNPC(NPC target)
        {
            // 外向き＆停止中は当たらない
            if (Projectile.localAI[0] == 1f)
                return false;

            return null;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 3; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PurpleCrystalShard, 0f, 0f, Projectile.alpha, new Color(Math.Abs(Projectile.ai[2]), 0, 255, Projectile.alpha));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Projectile.oldVelocity * 0.5f;
                Main.dust[dust].scale = Projectile.scale;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(
                Projectile,
                ProjectileID.Sets.TrailingMode[Type],
                lightColor,
                1
            );

            return false;
        }
    }
}
