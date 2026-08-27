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


    public class MermaidsTear_Shot : ModProjectile
    {
        private int damageTimer;
        private int hitCount;
        private Vector2 stickOffset;
        private float alphaFloat;
        private Vector2 hitLocalOffset; // NPC中心基準
        private float hitLocalRotation;   // NPC基準の相対角度

        private const int MaxTrailPoints = 25;
        private readonly List<Vector2> trailHistory = new();
        private Vector2 lastTrailPos;

        public override void SetDefaults()
        {
            Projectile.width = 230;
            Projectile.height = 20;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 0;

            Projectile.ArmorPenetration = 50;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;


            Projectile.DamageType = DamageClass.Summon;
        }

        public override void OnSpawn(IEntitySource source)
        {
            lastTrailPos = Projectile.Center;

            if (Projectile.velocity != Vector2.Zero)
                Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.netUpdate = true;
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
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

            trailHistory.Clear();
        }


        public override void AI()
        {
            // 初回SE（サーバー側では再生しないように安全を確保）
            if (Projectile.localAI[0] == 0f)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    SoundEngine.PlaySound(
                        SoundID.Item71 with { Pitch = Main.rand.NextFloat(0.8f, 1.0f), Volume = 5.0f },
                        Projectile.Center
                    );

                    SpawnGateDust();

                    // トレイルの初期位置がズレないように初期化
                    lastTrailPos = Projectile.Center;
                }

                Projectile.localAI[0] = 1f;
                // 状態が変わったので同期を要求
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

                // 継続ダメージ
                damageTimer++;
                if (damageTimer >= 30)
                {
                    damageTimer = 0;
                    hitCount++;

                    // 🌟 弾の生成は「この弾の持ち主（Owner）の画面」でのみ実行する
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(),
                            npc.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<MermaidsTear_Tick>(),
                            Projectile.damage,
                            0f,
                            Projectile.owner
                        );
                    }

                    // 🌟 演出（Dust）はサーバー以外（プレイヤーの画面）でのみ実行する
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
                                DustID.Water,
                                0f,
                                0f,
                                150,
                                default,
                                2.5f
                            );

                            dust.velocity = backward.RotatedByRandom(1.5f) * Main.rand.NextFloat(10f, 15f);
                            dust.noGravity = true;
                        }
                    }
                }

                // 透明度の計算
                alphaFloat += 1f;
                Projectile.alpha = (int)alphaFloat;

                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                    return;
                }

                // 🌟 トレイル履歴更新（サーバー以外でのみ実行してクラッシュを防止）
                if (Main.netMode != NetmodeID.Server)
                {
                    if (Main.GameUpdateCount % 4 == 0) // 4フレームに1回
                    {
                        trailHistory.Add(Projectile.Center);

                        if (trailHistory.Count > MaxTrailPoints / 2)
                            trailHistory.RemoveAt(0);
                    }
                }

                return;
            }

            // 通常飛行中
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, 0.6f, 0.1f, 0.6f);

            // 🌟 ===== トレイル高密度サンプリング ===== （サーバー以外でのみ実行）
            if (Main.netMode != NetmodeID.Server)
            {
                float dist = Vector2.Distance(lastTrailPos, Projectile.Center);

                // 何pxごとに点を打つか（小さいほど密）
                float step = 8f;

                if (dist > 0f)
                {
                    int steps = (int)(dist / step);
                    for (int i = 0; i <= steps; i++)
                    {
                        float t = i / (float)steps;
                        Vector2 p = Vector2.Lerp(lastTrailPos, Projectile.Center, t);
                        trailHistory.Add(p);

                        if (trailHistory.Count > MaxTrailPoints)
                            trailHistory.RemoveAt(0);
                    }
                }

                lastTrailPos = Projectile.Center;
            }
        }

        private void SpawnGateDust()
        {
            int count = 30;              // ダスト数（多いほど滑らか）
            float radiusX = 7f;         // 楕円の横半径
            float radiusY = 20f;         // 楕円の縦半径
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
                    DustID.PinkTorch,
                    Vector2.Zero,
                    150,
                    default,
                    2.9f
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

            if (Projectile.localAI[1] == 1f)
            {
                if (trailHistory.Count > 0)
                    trailHistory.Clear();
            }

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

            if (Projectile.timeLeft == 300) // 生成直後
                return false;

            if (Projectile.velocity == Vector2.Zero && Projectile.localAI[1] == 0f)
                return false;

            // ===== トレイル描画 =====
            if (trailHistory.Count > 1)
            {
                Texture2D tex = TextureAssets.Projectile[Type].Value;

                if (Projectile.localAI[0] == 0f)
                    return false;

                for (int i = 1; i < trailHistory.Count; i++)
                {
                    float t = i / (float)(trailHistory.Count - 1);

                    Vector2 pos = Vector2.Lerp(
                        trailHistory[i - 1],
                        trailHistory[i],
                        0.5f
                    );

                    float rot;

                    if (Projectile.localAI[1] == 0f)
                    {
                        // 飛行中：速度方向
                        rot = Projectile.velocity.ToRotation();
                    }
                    else
                    {
                        // 刺さり中：本体角度
                        rot = Projectile.rotation;
                    }

                    float alpha = Projectile.localAI[1] == 1f
                        ? MathHelper.Lerp(0.02f, 0.12f, t)
                        : MathHelper.Lerp(0.45f, 0.75f, t);

                    float scale = Projectile.localAI[1] == 1f
                        ? MathHelper.Lerp(1.0f, 1.0f, t)
                        : MathHelper.Lerp(1.0f, 1.0f, t);

                    Color color = Color.Pink * alpha * (1f - Projectile.alpha / 255f);

                    Main.EntitySpriteDraw(
                        tex,
                        pos - Main.screenPosition,
                        null,
                        color,
                        rot,
                        tex.Size() / 2f,
                        scale,
                        SpriteEffects.None,
                        0
                    );
                }
            }

            // 本体描画
            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                drawColor,
                Projectile.rotation,
                origin,
                1f,
                SpriteEffects.None,
                0
            );

            return false;
        }


    }
}