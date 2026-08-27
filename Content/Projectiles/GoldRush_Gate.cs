using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using System;
using System.IO;
using System.Collections.Generic;
using CalamitySimpleWhipAddon.Content.Systems;
using CalamitySimpleWhipAddon.Content.Buffs;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class GoldRush_Gate : ModProjectile, IAlphaBlendProjectile
    {
        public override void SetDefaults()
        {
            Projectile.netImportant = true;

            Projectile.width = 40;
            Projectile.height = 80;
            Projectile.scale = 0f;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }


        private Vector2 offsetFromPlayer;
        private bool initialized;
        private int syncedTimer;

        private int GateMode
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }



        public override void AI()
        {
            GoldRushManager.Update();

            Player player = Main.player[Projectile.owner];
            if (!player.active) { Projectile.Kill(); return; }

            Vector2 currentMouseDir;
            if (Main.myPlayer == Projectile.owner)
            {
                // 自分が持ち主なら、自分のマウス位置を使い、値を同期させる
                currentMouseDir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);

                float rot = currentMouseDir.ToRotation();

                if (Math.Abs(MathHelper.WrapAngle(rot - Projectile.ai[2])) > 0.05f)
                {
                    Projectile.ai[2] = rot;
                    Projectile.netUpdate = true;
                }
            }

            // 持ち主以外の画面では、同期された ai[2] から方向を復元する
            float baseRotation = Projectile.ai[2];
            Vector2 storedDir = baseRotation.ToRotationVector2();
            // ======================================
            // ★ モード判定（初期化時に一度だけ決定）
            // ======================================
            // Projectile.ai[1] をモード管理に使用: 0 = 未設定, 1 = プレイヤー背後, 2 = 敵の周り
            int mode = GateMode;
            int targetNPCIndex = (int)Projectile.ai[0];
            NPC targetNPC = (targetNPCIndex >= 0 && targetNPCIndex < 200) ? Main.npc[targetNPCIndex] : null;

            if (!initialized)
            {
                initialized = true;

                // ownerだけが初期配置を決める
                if (Main.myPlayer == Projectile.owner)
                {
                    bool hasDebuffTarget =
                        targetNPC != null &&
                        targetNPC.active &&
                        targetNPC.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx14>());

                    GateMode =
                        hasDebuffTarget && Main.rand.NextBool(2)
                        ? 2
                        : 1;

                    mode = GateMode;

                    float r;
                    Vector2 origin;
                    float finalRotation;

                    if (mode == 2)
                    {
                        float enemyRadius =
                            (targetNPC.width + targetNPC.height) / 2f;

                        float minDistanceToEnemy = 250f;

                        r =
                            enemyRadius +
                            minDistanceToEnemy +
                            Main.rand.NextFloat(0f, 30f);

                        origin = targetNPC.Center;
                        finalRotation = 0f;
                    }
                    else
                    {
                        r = Main.rand.NextFloat(80f, 160f);

                        origin =
                            player.Center +
                            (-storedDir * 80f);

                        finalRotation = baseRotation;
                    }

                    float randomAngle =
                        Main.rand.NextFloat(MathHelper.TwoPi);

                    offsetFromPlayer =
                        new Vector2(
                            MathF.Cos(randomAngle),
                            MathF.Sin(randomAngle)) * r;

                    // 重なり防止
                    for (int i = 0; i < 10; i++)
                    {
                        bool overlapping = false;

                        Vector2 potentialPos =
                            origin +
                            offsetFromPlayer.RotatedBy(finalRotation);

                        foreach (Projectile other in Main.projectile)
                        {
                            if (other.active &&
                                other.type == Projectile.type &&
                                other.whoAmI != Projectile.whoAmI)
                            {
                                if (Vector2.Distance(
                                    potentialPos,
                                    other.Center) < 40f)
                                {
                                    overlapping = true;
                                    break;
                                }
                            }
                        }

                        if (overlapping)
                            offsetFromPlayer =
                                offsetFromPlayer.RotatedBy(
                                    MathHelper.ToRadians(45));
                        else
                            break;
                    }

                    Projectile.Center =
                        origin +
                        offsetFromPlayer.RotatedBy(finalRotation);

                    Projectile.netUpdate = true;
                }

                // ownerから同期されるまで待機
                if (offsetFromPlayer == Vector2.Zero)
                    return;
            }

            // =========================
            // ★ 追従と回転（毎フレーム）
            // =========================
            Vector2 targetPos;
            

            if (mode == 2 && targetNPC != null && targetNPC.active)
            {
                // 敵にガッチリ追従
                targetPos = targetNPC.Center + offsetFromPlayer;
                // ゲートを常に敵の中心に向ける
                Projectile.rotation = (targetNPC.Center - Projectile.Center).ToRotation();
            }
            else
            {
                // プレイヤー背後に追従
                targetPos = player.Center + (-storedDir * 80f) + offsetFromPlayer.RotatedBy(baseRotation);
                Projectile.rotation = baseRotation;
            }

            // 追従速度（Lerp）
            Projectile.Center = Vector2.Lerp(Projectile.Center, targetPos, 0.25f);

            Projectile.velocity = Vector2.Zero;

            // =========================
            // ★ タイマー・スケール・寿命（既存）
            // =========================
            syncedTimer++;

            float timer = syncedTimer;
            float maxTime = 70f;

            if (timer <= 5f) Projectile.scale = MathHelper.Lerp(0f, 1f, timer / 5f);
            else if (timer >= maxTime - 5f) Projectile.scale = MathHelper.Lerp(1f, 0f, (timer - (maxTime - 5f)) / 5f);
            else Projectile.scale = 1f;

            if (timer == 1 && Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<GoldRush_Shot>(),
                    Projectile.damage,
                    0f,
                    Projectile.owner,
                    targetNPCIndex,
                    Projectile.identity,
                    0);

            }

            if (timer > maxTime) Projectile.Kill();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(GateMode);
            writer.WriteVector2(offsetFromPlayer);
            writer.Write(syncedTimer);
            writer.Write(Projectile.rotation);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            GateMode = reader.ReadInt32();
            offsetFromPlayer = reader.ReadVector2();
            syncedTimer = Math.Max(syncedTimer, reader.ReadInt32());
            Projectile.rotation = reader.ReadSingle();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public void DrawAlphaBlend(SpriteBatch spriteBatch)
        {
            if (!initialized || offsetFromPlayer == Vector2.Zero || syncedTimer <= 0)
                return;

            Texture2D tex = TextureAssets.Projectile[Type].Value;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = tex.Size() / 2f;

            float targetWidth = 50f;
            float targetHeight = 100f;

            Vector2 drawScale = new Vector2(targetWidth / tex.Width, targetHeight / tex.Height) * Projectile.scale;

            // --- ここからシェーダー描画の準備 ---
            // 現在の SpriteBatch の設定を引き継いだまま、一度 End して Begin し直すことでシェーダーを安全に適用します
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

            var effect = CalamitySimpleWhipAddon.GateEffect;
            effect.Parameters["rotation"].SetValue(Main.GlobalTimeWrappedHourly * 3f);
            effect.Parameters["tilt"].SetValue(0.5f);
            effect.Parameters["aspectRatio"].SetValue(targetHeight / targetWidth);
            effect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(
                tex,
                drawPos,
                null,
                Color.White * Projectile.scale,
                Projectile.rotation,
                origin,
                drawScale,
                SpriteEffects.None,
                0
            );

            // --- ここでシェーダーの適用を終了（超重要） ---
            // 描き終わったら即座に End し、通常の描画モード（SpriteSortMode.Deferred）に戻します
            // これにより、この後に描画されるトレイルにシェーダーが漏れ出るのを完全に防ぎます
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
        }

    }

    public static class GoldRushManager
    {
        public static int GlobalCooldown = 0;
        public const int DefaultCooldown = 3;

        public static void Update()
        {
            if (GlobalCooldown > 0)
                GlobalCooldown--;
        }

        public static bool CanSpawnGate()
        {


            return GlobalCooldown <= 0;
        }

        public static void OnSpawnGate(int cooldown = DefaultCooldown)
        {
            GlobalCooldown = cooldown;
        }
    }
}
