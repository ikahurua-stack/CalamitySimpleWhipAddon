using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using System;
using System.IO;
using CalamityMod.NPCs;
using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Systems;
using CalamitySimpleWhipAddon.Content.Items.Weapons;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class OpaqueOnyxGem : ModProjectile
    {
        private int flashTimer;

        public enum GemState
        {
            Idle,
            Charging,
            Cooldown
        }

        public GemState State = GemState.Idle;

        private int timer;
        private int targetWhoAmI = -1;

        private int storedDamage;
        private int bonusDamage;

        public override void SetDefaults()
        {
            Projectile.netImportant = true;

            Projectile.width = 32;
            Projectile.height = 32;

            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 2;
        }

        // ------------------------
        // TagOnHit から呼ぶ
        // ------------------------
        public void RequestCharge(NPC npc, int shotDamage)
        {
            if (State != GemState.Idle)
                return;

            storedDamage = shotDamage;
            targetWhoAmI = npc.whoAmI;
            bonusDamage = 0;
            timer = 0;
            flashTimer = 0;
            State = GemState.Charging;

            Projectile.netUpdate = true;
        }

        public void AddDamage(int damage)
        {
            if (State == GemState.Charging)
            {
                bonusDamage += (int)(damage * 0.10f);
                Projectile.netUpdate = true;
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            if (player.HeldItem.type != ModContent.ItemType<OpaqueOnyx>())
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;

            Vector2 offset = new Vector2(-player.direction * 20f, -60f);
            Vector2 targetPos = player.Center + offset;

            // 柔らか追従（安定版）
            Projectile.velocity += (targetPos - Projectile.Center) * 0.04f;
            Projectile.velocity *= 0.7f;

            // 向き
            Projectile.spriteDirection = player.direction;

            // 回転（横移動だけで傾く）
            float horizontal = Projectile.velocity.X;

            // 傾き量
            float tilt = horizontal * 0.08f;

            // 最大30度制限
            float maxTilt = MathHelper.ToRadians(50f);
            tilt = MathHelper.Clamp(tilt, -maxTilt, maxTilt);

            // 基本角度（ここ重要：逆さ対策）
            float baseRot = 0f;

            // 最終回転
            Projectile.rotation = baseRot + tilt;


            switch (State)
            {
                case GemState.Charging:
                    UpdateCharging();
                    break;

                case GemState.Cooldown:
                    UpdateCooldown();
                    break;
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 0.1f, 0.5f);
        }

        private void UpdateCharging()
        {
            timer++;
            flashTimer = timer;

            NPC target = GetTarget();

            // ターゲット消滅
            if (target == null)
            {
                CancelCharge();
                return;
            }

            if (timer >= (InfernalEclipseCompatibility.IsEnabled ? 30 : 20))
            {
                Fire(target);

                State = GemState.Cooldown;
                timer = 0;
                flashTimer = 0;
            }
        }

        private void UpdateCooldown()
        {
            timer++;

            if (timer >= 6)
            {
                timer = 0;
                State = GemState.Idle;
            }
        }

        private NPC GetTarget()
        {
            if (targetWhoAmI < 0 ||
                targetWhoAmI >= Main.maxNPCs)
                return FindTaggedTarget();

            NPC npc = Main.npc[targetWhoAmI];

            if (!npc.active ||
                npc.friendly ||
                npc.dontTakeDamage ||
                !npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx23>()))
            {
                return FindTaggedTarget();
            }

            return npc;
        }

        private NPC FindTaggedTarget()
        {
            NPC bestTarget = null;
            float bestDistance = 1200f;
            float lowestDR = 2f; // ボス用のDR初期値

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.active || npc.friendly || npc.dontTakeDamage)
                    continue;

                if (!npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx23>()))
                    continue;

                float distance = Vector2.Distance(Projectile.Center, npc.Center);

                // 1200ユニットより遠い敵は除外
                if (distance > bestDistance)
                    continue;

                // 【ボス限定の処理】
                if (npc.boss)
                {
                    float dr = npc.GetGlobalNPC<CalamityGlobalNPC>().DR;

                    // DRがより低いボスを最優先
                    if (dr < lowestDR)
                    {
                        lowestDR = dr;
                        bestDistance = distance;
                        bestTarget = npc;
                    }
                    // DRが同じなら、より距離が近いボスを優先
                    else if (dr == lowestDR && distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestTarget = npc;
                    }
                }
                // 【ザコ敵の処理】（まだボスが見つかっていない場合のみ候補にする）
                else if (lowestDR == 2f)
                {
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestTarget = npc;
                    }
                }
            }

            if (bestTarget != null)
                targetWhoAmI = bestTarget.whoAmI;
            else
                targetWhoAmI = -1;

            return bestTarget;
        }


        private void Fire(NPC target)
        {
            SoundEngine.PlaySound(
                new SoundStyle("CalamityMod/Sounds/Custom/RedJewelFire") with { Volume = 0.75f },
                Projectile.Center);

            CalamityGemBurstFX.Play(
                Projectile.Center,
                Color.Purple,
                Color.Pink,
                Color.HotPink,
                DustID.GemRuby
            );

            Vector2 dir =
                (target.Center - Projectile.Center)
                .SafeNormalize(Vector2.UnitY);

            Vector2 velocity = dir * 2f;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                velocity,
                ModContent.ProjectileType<OpaqueOnyxShot>(),
                storedDamage + bonusDamage,
                0f,
                Projectile.owner,
                target.whoAmI);

            // ★ここが反動
            Projectile.velocity -= dir * 4f;
            Projectile.velocity *= 0.85f;

            Projectile.netUpdate = true;
        }

        private void CancelCharge()
        {
            State = GemState.Idle;
            timer = 0;
            flashTimer = 0;
            targetWhoAmI = -1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((int)State);
            writer.Write(timer);
            writer.Write(targetWhoAmI);
            writer.Write(storedDamage);
            writer.Write(bonusDamage);
            writer.Write(flashTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            State = (GemState)reader.ReadInt32();
            timer = reader.ReadInt32();
            targetWhoAmI = reader.ReadInt32();
            storedDamage = reader.ReadInt32();
            bonusDamage = reader.ReadInt32();
            flashTimer = reader.ReadInt32();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D gem =
                Terraria.GameContent.TextureAssets.Projectile[Type].Value;

            Asset<Texture2D> flash =
                ModContent.Request<Texture2D>(
                    "CalamitySimpleWhipAddon/Content/Projectiles/OpaqueOnyxFlash");


            Main.EntitySpriteDraw(
                gem,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation,
                gem.Size() / 2f,
                1f,
                SpriteEffects.None);

            if (State == GemState.Charging)
            {
                float t =
                    MathHelper.Clamp(flashTimer / 20f, 0f, 1f);

                // イージング（今の雰囲気を維持）
                float alpha = MathF.Sin(t * MathHelper.PiOver2);
                alpha = MathF.Pow(alpha, 1.5f);

                // ★サイズ用（0.5 → 1.0）
                float scale = MathHelper.Lerp(0.05f, 1f, alpha);

                Main.EntitySpriteDraw(
                    flash.Value,
                    Projectile.Center - Main.screenPosition,
                    null,
                    Color.Pink * 0.8f * alpha,
                    Projectile.rotation,
                    flash.Value.Size() / 2f,
                    scale * (1f + alpha * 0.2f),
                    SpriteEffects.None
                );
            }

            return false;
        }
    }
}
