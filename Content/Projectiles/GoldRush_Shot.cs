using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent.Drawing;
using CalamitySimpleWhipAddon.Content.Systems;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class GoldRush_Shot : ModProjectile, IAlphaBlendProjectile
    {
        private float _impactRotation = 0f;

        private const int MAX_HIT_TIME = 15; // 消えるまでの時間

        private const int MAX_TILE_STAY_TIME = 200; // ブロックに刺さってから消えるまでの時間

        private List<Vector2> _oldPositions = new List<Vector2>();
        private const int MAX_TRAIL_POINTS = 20; // トレイルの長さ


        private Matrix GetWorldViewProjection()
        {
            Matrix screenDescriptor = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            return screenDescriptor;
        }


        private Texture2D SelectedTex =>
            ModContent.Request<Texture2D>(SwordTexturePaths[_textureIndex]).Value;
        private bool _isInitialized = false;


        // 抽選するテクスチャのパスリスト
        private static readonly string[] SwordTexturePaths = {
            "CalamitySimpleWhipAddon/Content/Projectiles/GoldRush_Shot01",
            "CalamitySimpleWhipAddon/Content/Projectiles/GoldRush_Shot02",
            "CalamitySimpleWhipAddon/Content/Projectiles/GoldRush_Shot03",
            "CalamitySimpleWhipAddon/Content/Projectiles/GoldRush_Shot04",
            "CalamitySimpleWhipAddon/Content/Projectiles/GoldRush_Shot05",
            "CalamitySimpleWhipAddon/Content/Projectiles/GoldRush_Shot06",
            "CalamitySimpleWhipAddon/Content/Projectiles/GoldRush_Shot07"
        };

        private enum SwordState
        {
            Spawning,
            Flying,
            Hit,
            StuckInTile
        }

        private SwordState _state;

        private int _textureIndex;
        private float _spawnRotation;

        private Vector2 _impactPosition;


        private int _spawnTimer;
        private int _flyTimer;
        private int _hitTimer;
        private int _tileTimer;

        private Projectile GetOwningGate()
        {
            int gateIdentity = (int)Projectile.ai[1];

            if (gateIdentity < 0)
                return null;

            int gateType = ModContent.ProjectileType<GoldRush_Gate>();

            foreach (Projectile candidate in Main.projectile)
            {
                if (candidate.active &&
                    candidate.type == gateType &&
                    candidate.owner == Projectile.owner &&
                    candidate.identity == gateIdentity)
                {
                    return candidate;
                }
            }

            return null;
        }


        public override void SetDefaults()
        {
            Projectile.netImportant = true;

            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1; // 同一NPCに1回のみ
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {

            int initialTargetIndex = (int)Projectile.ai[0]; // ゲートから渡されたターゲット
            int trackingTargetIndex = (int)Projectile.ai[2];
            Projectile gate = GetOwningGate();
            Vector2 shootDir = gate != null ? gate.rotation.ToRotationVector2() : Projectile.velocity.SafeNormalize(Vector2.UnitX);

            if (!_isInitialized)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    _textureIndex = Main.rand.Next(SwordTexturePaths.Length);
                    _spawnRotation = gate?.rotation ?? Projectile.rotation;

                    Projectile.netUpdate = true;
                }

                _state = SwordState.Spawning;

                _isInitialized = true;
            }

            if (_state == SwordState.Spawning &&
                (gate == null || !gate.active))
            {
                Projectile.Kill();
                return;
            }

            if (_state != SwordState.Spawning)
            {
                if (_state == SwordState.Flying)
                {
                    _oldPositions.Add(Projectile.Center);

                    if (_oldPositions.Count > MAX_TRAIL_POINTS)
                        _oldPositions.RemoveAt(0);
                }

                if (_oldPositions.Count > MAX_TRAIL_POINTS)
                    _oldPositions.RemoveAt(0);
            }



            // =========================
            // フェーズ0：出現
            // =========================
            // AI内のフェーズ0（出現）
            if (_state == SwordState.Spawning)
            {
                Projectile.friendly = false;

                if (gate == null)
                {
                    Projectile.Kill();
                    return;
                }


                float swordLength =
                    SelectedTex.Width * 1.414f * Projectile.scale;

                float p = (float)_spawnTimer / 120f;


                Vector2 dir = gate.rotation.ToRotationVector2();

                Projectile.Center =
                    gate.Center +
                    dir * (swordLength * 0.75f * p);

                Projectile.rotation = gate.rotation;

                _spawnTimer++;

                if (_spawnTimer >= 120)
                {
                    Projectile.velocity = dir * 20f;

                    Projectile.tileCollide = true;

                    _state = SwordState.Flying;

                    Projectile.netUpdate = true;

                    SoundEngine.PlaySound(
                        SoundID.Item71,
                        Projectile.Center);
                }

                return;
            }


            // =========================
            // フェーズ1：射出・自動追尾
            // =========================
            else
            {
                if (_state == SwordState.Flying)
                {
                    _flyTimer++;

                    // 普通に飛ぶ
                    Projectile.rotation = Projectile.velocity.ToRotation();

                    if (_flyTimer > 10 &&
                        Projectile.owner == Main.myPlayer)
                    {
                        NPC closest = null;
                        float shortest = 325f;

                        foreach (NPC npc in Main.npc)
                        {
                            if (!npc.CanBeChasedBy())
                                continue;

                            float distance = Vector2.Distance(npc.Center, Projectile.Center);

                            if (distance < shortest)
                            {
                                shortest = distance;
                                closest = npc;
                            }
                        }

                        if (closest != null)
                        {
                            Vector2 dir =
                                (closest.Center - Projectile.Center)
                                .SafeNormalize(Vector2.UnitX);

                            Projectile.velocity = dir * 45f;
                            Projectile.rotation = Projectile.velocity.ToRotation();
                            Projectile.friendly = true;

                            // もうロックオンしないように
                            _flyTimer = -9999;

                            Projectile.netUpdate = true;
                        }
                    }
                }


                if ((_state == SwordState.Flying) &&
                    Main.rand.NextBool(3))
                {
                    Vector2 backward = -Projectile.velocity.SafeNormalize(Vector2.Zero);

                    Vector2 randomOffset =
                        backward.RotatedBy(MathHelper.PiOver2) *
                        Main.rand.NextFloat(-10f, 10f);

                    Vector2 lengthOffset =
                        backward *
                        Main.rand.NextFloat(0f, 20f);

                    ParticleOrchestrator.RequestParticleSpawn(
                        false,
                        ParticleOrchestraType.SilverBulletSparkle,
                        new ParticleOrchestraSettings()
                        {
                            PositionInWorld =
                                Projectile.Center +
                                randomOffset +
                                lengthOffset,

                            MovementVector =
                                backward *
                                Main.rand.NextFloat(3f, 6f)
                        });
                }

                if (_state == SwordState.Flying)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                }
            }

            if (_state == SwordState.Hit)
            {
                Projectile.velocity *= 0.2f;

                _hitTimer++;

                if (_hitTimer >= MAX_HIT_TIME)
                    Projectile.Kill();

                return;
            }

            if (_state == SwordState.StuckInTile)
            {
                Projectile.velocity = Vector2.Zero;

                _tileTimer++;

                if (_tileTimer >= MAX_TILE_STAY_TIME)
                    Projectile.Kill();

                return;
            }

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            _oldPositions.Clear();

            if (_state == SwordState.Hit)
                return false;


            _impactRotation = Projectile.rotation;
            _impactPosition = Projectile.Center;

            _impactPosition += oldVelocity.SafeNormalize(Vector2.Zero) * 50f;

            Projectile.velocity = Vector2.Zero;

            _state = SwordState.StuckInTile;

            // 演出：音とパーティクル
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.SilverBulletSparkle, new ParticleOrchestraSettings()
                {
                    PositionInWorld = Projectile.Center + Main.rand.NextVector2Circular(40f, 40f),
                    MovementVector = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 7f)
                });
            }

            Projectile.netUpdate = true;

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            _oldPositions.Clear();

            if (_state == SwordState.Hit)
                return;


            _impactRotation = Projectile.rotation;
            _impactPosition = Projectile.Center;

            Projectile.friendly = false;
            Projectile.damage = 0;
            Projectile.velocity *= 0.1f;

            _state = SwordState.Hit;


            int particleCount = Main.rand.Next(3, 5);

            for (int i = 0; i < particleCount; i++)
            {

                Vector2 dir =
                Projectile.velocity.SafeNormalize(Vector2.UnitX);

                Vector2 spreadVelocity = dir.RotatedByRandom(0.5f) * Main.rand.NextFloat(4f, 10f);

                ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.PaladinsHammer, new ParticleOrchestraSettings()
                {
                    PositionInWorld = _impactPosition,
                    MovementVector = spreadVelocity // 逆方向に飛ばす
                });
            }

            for (int i = 0; i < 1; i++)
            {

                ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.Excalibur, new ParticleOrchestraSettings()
                {
                    PositionInWorld = _impactPosition,
                    MovementVector = Vector2.One
                });
            }

            // ヒット時の効果音（任意で追加。Excalibur系ならItem68なども合います）
            SoundEngine.PlaySound(SoundID.Item105, Projectile.Center);

            Projectile.netUpdate = true;
        }

        public void DrawBoardTrail()
        {
            if (_oldPositions.Count < 3) return;

            // トレイル用のエフェクト（ロード処理はModのLoadで行うか、ここでRequest）
            var effect = CalamitySimpleWhipAddon.ShotTrailEffect;
            // トレイル用のテクスチャ（中心が白く、上下が透明なグラデーションを推奨）
            var trailTex = ModContent.Request<Texture2D>("CalamitySimpleWhipAddon/Content/Textures/SoftTrail").Value;

            // 補間後のセグメント数（1つの区間を4分割して滑らかにする）
            int subdivisions = 4;
            int totalSegments = (_oldPositions.Count - 1) * subdivisions;
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[totalSegments * 6];

            for (int i = 0; i < totalSegments; i++)
            {
                float progress = (float)i / totalSegments;
                float nextProgress = (float)(i + 1) / totalSegments;

                // 1. 滑らかな基本座標を取得
                Vector2 basePos = GetSmoothedPosition(progress);
                Vector2 nextBasePos = GetSmoothedPosition(nextProgress);

                // 2. 進行方向の逆（後ろ向き）のベクトルを計算
                // offsetDist の数値を大きくすると、より後ろからトレイルが始まります
                float offsetDist = 0f;
                Vector2 backward = -(nextBasePos - basePos).SafeNormalize(Vector2.Zero);

                // 3. 座標を後ろにずらす
                Vector2 cur = (basePos + backward * offsetDist) - Main.screenPosition;
                Vector2 next = (nextBasePos + backward * offsetDist) - Main.screenPosition;

                Vector2 dir = (next - cur).SafeNormalize(Vector2.Zero);
                Vector2 normal = new Vector2(-dir.Y, dir.X);

                // 幅と透明度の計算（先端に向かって細く、透明に）
                float width = Projectile.scale * 7f * progress;
                float nextWidth = Projectile.scale * 7f * nextProgress;
                Color col = Color.LightGoldenrodYellow * progress * 0.6f;
                Color nextCol = Color.Gold * nextProgress * 0.6f;

                int v = i * 6;
                vertices[v] = new VertexPositionColorTexture(new Vector3(cur + normal * width, 0), col, new Vector2(progress, 0));
                vertices[v + 1] = new VertexPositionColorTexture(new Vector3(cur - normal * width, 0), col, new Vector2(progress, 1));
                vertices[v + 2] = new VertexPositionColorTexture(new Vector3(next - normal * nextWidth, 0), nextCol, new Vector2(nextProgress, 1));

                vertices[v + 3] = new VertexPositionColorTexture(new Vector3(cur + normal * width, 0), col, new Vector2(progress, 0));
                vertices[v + 4] = new VertexPositionColorTexture(new Vector3(next - normal * nextWidth, 0), nextCol, new Vector2(nextProgress, 1));
                vertices[v + 5] = new VertexPositionColorTexture(new Vector3(next + normal * nextWidth, 0), nextCol, new Vector2(nextProgress, 0));
            }

            effect.Parameters["uTransform"].SetValue(GetWorldViewProjection());
            effect.Parameters["uTime"].SetValue((float)Main.timeForVisualEffects * 0.05f);
            Main.graphics.GraphicsDevice.Textures[0] = trailTex;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);
            }
        }

        private void DrawSpriteBatchTrail(SpriteBatch spriteBatch)
        {
            if (_oldPositions.Count < 2)
                return;

            Texture2D trailTex = ModContent.Request<Texture2D>("CalamitySimpleWhipAddon/Content/Textures/SoftTrail").Value;

            // 原点を「中心」ではなく「左端の中央」にする（これで隙間が消えます）
            Vector2 origin = new Vector2(0f, trailTex.Height / 2f);

            for (int i = 1; i < _oldPositions.Count; i++)
            {
                Vector2 start = _oldPositions[i - 1];
                Vector2 end = _oldPositions[i];
                Vector2 segment = end - start;
                float length = segment.Length();

                if (length <= 1f)
                    continue;

                float progress = i / (float)(_oldPositions.Count - 1);
                float width = MathHelper.Lerp(2f, 10f, progress) * Projectile.scale;
                Color color = Color.Lerp(Color.LightGoldenrodYellow, Color.Gold, progress) * (0.45f * progress);


                spriteBatch.Draw(
                    trailTex,
                    start - Main.screenPosition, // 始点から描画を開始
                    null,
                    color,
                    segment.ToRotation(), // セグメントの進行方向へ向ける
                    origin,
                    new Vector2(length / trailTex.Width, width / trailTex.Height), // 長さと幅を正確に引き伸ばす
                    SpriteEffects.None,
                    0f);
            }
        }


        // 座標を滑らかに補完するヘルパー関数
        private Vector2 GetSmoothedPosition(float progress)
        {
            float realIndex = progress * (_oldPositions.Count - 1);
            int i = (int)realIndex;
            float t = realIndex - i;

            if (i >= _oldPositions.Count - 1) return _oldPositions.Last();

            // 近隣の4点を使って補間
            Vector2 p0 = _oldPositions[Math.Max(i - 1, 0)];
            Vector2 p1 = _oldPositions[i];
            Vector2 p2 = _oldPositions[Math.Min(i + 1, _oldPositions.Count - 1)];
            Vector2 p3 = _oldPositions[Math.Min(i + 2, _oldPositions.Count - 1)];

            return Vector2.CatmullRom(p0, p1, p2, p3, t);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)_state);

            writer.Write(_textureIndex);

            writer.Write(_spawnRotation);

            writer.Write(_spawnTimer);
            writer.Write(_flyTimer);
            writer.Write(_hitTimer);
            writer.Write(_tileTimer);

            writer.Write(_impactRotation);
            writer.WriteVector2(_impactPosition);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            _state = (SwordState)reader.ReadByte();

            _textureIndex = reader.ReadInt32();

            _spawnRotation = reader.ReadSingle();

            _spawnTimer = reader.ReadInt32();
            _flyTimer = reader.ReadInt32();
            _hitTimer = reader.ReadInt32();
            _tileTimer = reader.ReadInt32();

            _impactRotation = reader.ReadSingle();
            _impactPosition = reader.ReadVector2();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public void DrawAlphaBlend(SpriteBatch spriteBatch)
        {
            Texture2D tex = SelectedTex;

            Vector2 finalDrawPos = Projectile.Center - Main.screenPosition;
            float currentRotation = Projectile.rotation;
            float cutoff = 1f;

            float swordFullLen = tex.Width * 1.414f * Projectile.scale;

            // ===== 出現中 =====
            if (_state == SwordState.Spawning)
            {
                Projectile gate = GetOwningGate();

                if (gate != null)
                {
                    float moveDist = Vector2.Distance(gate.Center, Projectile.Center);

                    cutoff = moveDist / swordFullLen;
                    cutoff = MathHelper.Clamp(cutoff, 0f, 0.75f);
                }
            }

            // ===== 敵ヒット後 =====
            else if (_state == SwordState.Hit)
            {

                finalDrawPos = _impactPosition - Main.screenPosition;

                cutoff = 1f - ((float)_hitTimer / MAX_HIT_TIME);
                cutoff = MathHelper.Clamp(cutoff, 0f, 1f);

                currentRotation = _impactRotation;

            }

            // ===== ブロック刺さり =====
            else if (_state == SwordState.StuckInTile)
            {

                finalDrawPos = _impactPosition - Main.screenPosition;

                int remaining = MAX_TILE_STAY_TIME - _tileTimer;

                if (remaining <= 60)
                {
                    cutoff = (float)remaining / 60f;
                    cutoff = MathHelper.Clamp(cutoff, 0f, 1f);
                }

                currentRotation = _impactRotation;
            }

            else if (_state == SwordState.Flying)
            {
                DrawSpriteBatchTrail(spriteBatch);
            }

            Vector2 origin = new Vector2(tex.Width, 0);

            Vector2 drawOffset =
                _state == SwordState.StuckInTile
                ? _impactRotation.ToRotationVector2() * 10f
                : Vector2.Zero;

            float drawRotation = currentRotation + MathHelper.PiOver4;

            Vector3 currentEdgeColor = new Vector3(1f, 0.8f, 0.5f);
            float glowIntensity = 1.5f;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

            var effect = CalamitySimpleWhipAddon.DiagonalCutEffect;

            effect.Parameters["cutoff"].SetValue(cutoff);

            if (effect.Parameters["edgeColor"] != null)
                effect.Parameters["edgeColor"].SetValue(currentEdgeColor * glowIntensity);

            effect.CurrentTechnique.Passes[0].Apply();

            Color drawColor = Color.White * cutoff;

            spriteBatch.Draw(
                tex,
                finalDrawPos + drawOffset,
                null,
                drawColor,
                drawRotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
        }


    }
}
