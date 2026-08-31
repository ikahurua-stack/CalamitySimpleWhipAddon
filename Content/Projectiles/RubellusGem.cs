using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent;
using System.Collections.Generic;
using System;
using System.IO;
using CalamityMod.NPCs;
using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Systems;
using CalamitySimpleWhipAddon.Content.Items.Weapons;
using CalamitySimpleWhipAddon.Content.Common;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class RubellusGem : ModProjectile
    {

        private int flashTimer;

        private int recoilTimer;

        private float recoilBlend;

        private readonly List<NPC> chainTargets = new();

        private readonly List<int> chainDamages = new();


        private bool chainActive;

        private int chainIndex;

        private int chainDelay;

        private const int FramesPerChain = 2;

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

            if (player.HeldItem.type != ModContent.ItemType<Rubellus>())
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

            Lighting.AddLight(Projectile.Center, 1f, 0.3f, 0.3f);

            UpdateChain();
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

            if (timer >= 20)
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
                !npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx18>()))
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
            Vector2 dir =
                (target.Center - Projectile.Center)
                .SafeNormalize(Vector2.UnitY);

            GenerateChain(target);

            chainActive = true;
            chainIndex = 0;
            chainDelay = 0;

            Projectile.velocity -= dir * 5f;
            recoilTimer = 4;

            Projectile.netUpdate = true;
        }

        private void CancelCharge()
        {
            State = GemState.Idle;
            timer = 0;
            flashTimer = 0;
            targetWhoAmI = -1;
        }

        private void GenerateChain(NPC firstTarget)
        {
            chainTargets.Clear();
            chainDamages.Clear();

            NPC current = firstTarget;

            chainTargets.Add(current);

            float damageMultiplier = 1f;
            chainDamages.Add(storedDamage + bonusDamage);

            while (true)
            {
                NPC nextTarget = null;
                float nearestDistance = 2000f;
                float lowestDR = 2f; // ボス用のDR初期値

                for (int j = 0; j < Main.maxNPCs; j++)
                {
                    NPC npc = Main.npc[j];

                    if (!npc.active || npc.friendly || npc.dontTakeDamage)
                        continue;

                    float dr = npc.GetGlobalNPC<CalamityGlobalNPC>().DR;

                    if (dr >= 0.95f)
                        continue;

                    if (!npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx18>()))
                        continue;

                    // 既にチェインに含まれている敵は除外
                    if (chainTargets.Contains(npc))
                        continue;

                    float distance = Vector2.Distance(current.Center, npc.Center);

                    // 2000ユニットより遠い敵は除外
                    if (distance > nearestDistance)
                        continue;

                    // 【ボス限定の処理】
                    if (npc.boss)
                    {
                        // DRがより低いボスを最優先
                        if (dr < lowestDR)
                        {
                            lowestDR = dr;
                            nearestDistance = distance;
                            nextTarget = npc;
                        }
                        // DRが同じなら、より距離が近いボスを優先
                        else if (dr == lowestDR && distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nextTarget = npc;
                        }
                    }
                    // 【ザコ敵の処理】（まだ候補にボスが見つかっていない場合のみ連鎖候補にする）
                    else if (lowestDR == 2f)
                    {
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nextTarget = npc;
                        }
                    }
                }

                if (nextTarget == null)
                    break;

                current = nextTarget;

                chainTargets.Add(current);

                damageMultiplier *= Main.rand.NextFloat(0.75f, 0.85f);

                int damage = Math.Max((int)((storedDamage + bonusDamage) * damageMultiplier), 1);

                chainDamages.Add(damage);

                if (damage < (storedDamage + bonusDamage) * 0.05f)
                    break;
            }
        }


        private void UpdateChain()
        {
            if (!chainActive)
                return;

            if (Projectile.owner != Main.myPlayer)
                return;

            chainDelay++;

            if (chainDelay < FramesPerChain)
                return;

            chainDelay = 0;

            if (chainIndex >= chainTargets.Count)
            {
                chainActive = false;
                return;
            }

            NPC npc = chainTargets[chainIndex];

            if (npc.active)
            {
                Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        npc.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<RubellusGem_Damage>(),
                        chainDamages[chainIndex],
                        0f,
                        Projectile.owner,
                        npc.whoAmI);

                AddLightningSegment(chainIndex);
            }

            chainIndex++;
        }

        private void AddLightningSegment(int index)
        {
            Vector2 start;
            Vector2 end;

            if (index == 0)
            {
                start = Projectile.Center;
                end = chainTargets[0].Center;
            }
            else
            {
                start = chainTargets[index - 1].Center;
                end = chainTargets[index].Center;
            }

            // ★サーバーのみ送信
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = Mod.GetPacket();

                packet.Write((byte)SimpleWhipPacketID.LightningSpawn);

                packet.Write(Projectile.whoAmI);

                // X, Y 個別ではなく Vector2 ごと書き込む
                packet.WriteVector2(start);
                packet.WriteVector2(end);

                packet.Write(index);

                packet.Send();
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
            {
                RubellusGemSystem.AddLightning(start, end, index);
            }
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
                    "CalamitySimpleWhipAddon/Content/Projectiles/RubellusGemFlash");

            Main.EntitySpriteDraw(
                gem,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation,
                gem.Size() / 2f,
                0.4f,
                SpriteEffects.None);

            if (State == GemState.Charging)
            {
                float t =
                    MathHelper.Clamp(flashTimer / 20f, 0f, 1f);

                // イージング（今の雰囲気を維持）
                float alpha = MathF.Sin(t * MathHelper.PiOver2);
                alpha = MathF.Pow(alpha, 1.5f);


                Main.EntitySpriteDraw(
                    flash.Value,
                    Projectile.Center - Main.screenPosition,
                    null,
                    Color.White * 1f * alpha,
                    Projectile.rotation,
                    flash.Value.Size() / 2f,
                    0.4f,
                    SpriteEffects.None
                );
            }

            

            return false;
        }
    }
}