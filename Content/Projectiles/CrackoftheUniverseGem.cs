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
    public class CrackoftheUniverseGem : ModProjectile
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
        private int recoilTimer;

        private float recoilBlend;

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
            bonusDamage = 0;

            targetWhoAmI = npc.whoAmI;
            timer = 0;
            flashTimer = 0;
            State = GemState.Charging;

            Projectile.netUpdate = true;
        }

        public void AddDamage(int damage)
        {
            if (State == GemState.Charging)
            {
                bonusDamage += (int)(damage * 0.1f);
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

            if (player.HeldItem.type != ModContent.ItemType<CrackoftheUniverse>())
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;

            Vector2 offset = new Vector2(-player.direction * 20f, -60f);
            Vector2 targetPos = player.Center + offset;

            if (recoilTimer > 0)
            {
                recoilTimer--;
                recoilBlend = 1f;
            }
            else
            {
                recoilBlend = MathHelper.Lerp(recoilBlend, 0f, 0.15f);
            }

            float followStrength =
                MathHelper.Lerp(0.07f, 0.015f, recoilBlend);

            float damping =
                MathHelper.Lerp(0.78f, 0.93f, recoilBlend);

            Projectile.velocity += (targetPos - Projectile.Center) * followStrength;
            Projectile.velocity *= damping;

            // 向き
            Projectile.spriteDirection = player.direction;

            // 回転（横移動だけで傾く）
            float horizontal = Projectile.velocity.X;

            // 傾き量
            float tilt = horizontal * 0.06f;

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

            Lighting.AddLight(Projectile.Center, 0.2f, 0.7f, 0.8f);
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

            if (timer >= 44)
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

            if (timer >= 2)
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
                !npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx17>()))
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
                new SoundStyle("CalamityMod/Sounds/Custom/RedJewelFire") with { Volume = 0.75f},
                Projectile.Center);

            Vector2 dir =
                (target.Center - Projectile.Center)
                .SafeNormalize(Vector2.UnitY);


            int p = Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<CrackoftheUniverseBeam>(),
                storedDamage + bonusDamage,
                0f,
                Projectile.owner,
                dir.X,
                dir.Y);

            Main.projectile[p].DamageType = DamageClass.Summon;
            Main.projectile[p].penetrate = 9999;

            Projectile.velocity -= dir * 12f;
            recoilTimer = 8;

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
                    "CalamitySimpleWhipAddon/Content/Projectiles/CrackoftheUniverseFlash");


            if (State == GemState.Charging)
            {
                float t =
                    MathHelper.Clamp(flashTimer / 40f, 0f, 1f);

                // イージング（今の雰囲気を維持）
                float alpha = MathF.Sin(t * MathHelper.PiOver2);
                alpha = MathF.Pow(alpha, 1.5f);

                // ★サイズ用（0.5 → 1.0）
                float scale = MathHelper.Lerp(0.001f, 1f, alpha);

                Main.EntitySpriteDraw(
                    flash.Value,
                    Projectile.Center - Main.screenPosition,
                    null,
                    Color.Black,
                    Projectile.rotation,
                    flash.Value.Size() / 2f,
                    scale * (0.28f + alpha * 0.1f),
                    SpriteEffects.None
                );
            }

            Main.EntitySpriteDraw(
                gem,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation,
                gem.Size() / 2f,
                0.4f,
                SpriteEffects.None);


            return false;
        }
    }
}