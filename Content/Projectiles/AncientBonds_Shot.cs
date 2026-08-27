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
using Terraria.DataStructures;
using System;
using System.IO;


namespace CalamitySimpleWhipAddon.Content.Projectiles
{


    public class AncientBonds_Shot : ModProjectile
    {
        private int damageTimer;
        private int hitCount;
        private Vector2 stickOffset;
        private Vector2 hitLocalOffset; // NPC中心基準
        private float hitLocalRotation;   // NPC基準の相対角度 // NPC基準の相対角度
        private float alphaFloat;


        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 15;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 120;
            Projectile.extraUpdates = 0;

            Projectile.ArmorPenetration = 5;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.DamageType = DamageClass.Summon;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 🌟 弾の持ち主（Owner）の画面でのみ処理を行う（マルチプレイの二重処理・同期ズレ防止）
            if (Projectile.owner == Main.myPlayer && Projectile.localAI[1] == 0f)
            {
                Projectile.localAI[1] = 1f;

                Projectile.ai[0] = target.whoAmI;
                Projectile.ai[1] = Projectile.rotation;

                // ★ 刺さった位置の差分を保存
                Vector2 dir = Projectile.velocity.SafeNormalize(Vector2.UnitX);
                Vector2 hitWorldPos = Projectile.Center + dir;

                // NPC基準ローカル座標に変換
                hitLocalOffset = (hitWorldPos - target.Center).RotatedBy(-target.rotation);

                // 刺さった角度も NPC基準で保存
                hitLocalRotation = Projectile.rotation - target.rotation;

                stickOffset = hitLocalOffset;

                Projectile.velocity = Vector2.Zero;

                // 🌟 これにより、SendExtraAI が呼び出され、
                // 独自変数（hitLocalOffset等）と localAI の中身が全員に送信されます！
                Projectile.netUpdate = true;
            }
        }


        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.velocity != Vector2.Zero)
                Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.netUpdate = true;
        }

        public override void AI()
        {
            // 初回SE（サーバー側では再生しないように安全を確保）
            if (Projectile.localAI[0] == 0f)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    SoundEngine.PlaySound(
                        SoundID.Item73 with { Volume = 3f },
                        Projectile.Center
                    );
                    SpawnGateDust();
                }

                Projectile.localAI[0] = 1f;
                // 初回フラグが立ったら同期（必要に応じて）
                Projectile.netUpdate = true;
            }

            // 貼り付き中
            if (Projectile.localAI[1] == 1f)
            {
                int npcIndex = (int)Projectile.ai[0];
                if (npcIndex < 0 || npcIndex >= Main.maxNPCs)
                {
                    Projectile.Kill();
                    return;
                }

                NPC npc = Main.npc[npcIndex];
                if (!npc.active || npc.dontTakeDamage)
                {
                    Projectile.Kill();
                    return;
                }

                // NPCに追従、回転を考慮して復元
                Vector2 rotatedOffset = hitLocalOffset.RotatedBy(npc.rotation);
                Projectile.Center = npc.Center + rotatedOffset;
                Projectile.rotation = hitLocalRotation + npc.rotation;

                damageTimer++;
                if (damageTimer >= 30)
                {
                    damageTimer = 0;
                    hitCount++;

                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(),
                            npc.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<AncientBonds_Tick>(),
                            Projectile.damage,
                            0f,
                            Projectile.owner
                        );
                    }

                    if (Main.netMode != NetmodeID.Server)
                    {
                        Vector2 backward = Projectile.ai[1].ToRotationVector2() * -1f;

                        for (int i = 0; i < 8; i++)
                        {
                            Vector2 hitPos = npc.Center + hitLocalOffset;

                            Dust dust = Dust.NewDustDirect(
                                hitPos,
                                0,
                                0,
                                DustID.Lava,
                                0f,
                                0f,
                                150,
                                default,
                                1.2f
                            );

                            dust.velocity = backward.RotatedByRandom(0.8f) * Main.rand.NextFloat(5f, 7f);
                            dust.noGravity = true;
                        }
                    }
                }

                alphaFloat += 2f;
                Projectile.alpha = (int)alphaFloat;

                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                    return;
                }


                return;
            }

            // 通常飛行中
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, 0.6f, 0.35f, 0.15f);
        }


        private void SpawnGateDust()
        {
            int count = 24;              // ダスト数（多いほど滑らか）
            float radiusX = 5f;         // 楕円の横半径
            float radiusY = 15f;         // 楕円の縦半径
            float rotation = Projectile.rotation;

            for (int i = 0; i < count; i++)
            {
                float angle = MathHelper.TwoPi * i / count;

                // 楕円座標
                Vector2 offset = new Vector2(
                    (float)Math.Cos(angle) * radiusX,
                    (float)Math.Sin(angle) * radiusY
                );

                // プロジェクタイルの向きに合わせて回転
                offset = offset.RotatedBy(rotation);

                Vector2 pos = Projectile.Center + offset;

                Dust dust = Dust.NewDustPerfect(
                    pos,
                    DustID.Torch,
                    Vector2.Zero,
                    150,
                    default,
                    1.2f
                );

                // 外側に少しだけ押し出す
                dust.velocity = offset.SafeNormalize(Vector2.Zero) * 1.5f;
                dust.noGravity = true;
            }
        }


        public override bool? CanHitNPC(NPC target)
        {
            // 既に貼り付いている場合は「自分が刺さった相手」以外も許可
            if (Projectile.localAI[1] == 1f)
                return false;

            return null;
        }

        public override void SendExtraAI(System.IO.BinaryWriter writer)
        {
            writer.WriteVector2(hitLocalOffset);
            writer.Write(hitLocalRotation);

            // localAIの状態を書き込み
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);

            // 小数管理の透明度も同期対象に追加
            writer.Write(alphaFloat);
        }

        public override void ReceiveExtraAI(System.IO.BinaryReader reader)
        {
            hitLocalOffset = reader.ReadVector2();
            hitLocalRotation = reader.ReadSingle();

            stickOffset = hitLocalOffset;

            // 送った順番通りに読み込み
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();

            // 透明度を読み込み
            alphaFloat = reader.ReadSingle();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() / 2f;

            Color drawColor = Color.White * (1f - Projectile.alpha / 255f);
            float drawScale = 1.2f;

            if (Projectile.timeLeft == 300) // 生成直後
                return false;

            if (Projectile.velocity == Vector2.Zero && Projectile.localAI[1] == 0f)
                return false;

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                drawColor,
                Projectile.rotation,
                origin,
                drawScale,
                SpriteEffects.None,
                0
            );

            return false;
        }

    }
}