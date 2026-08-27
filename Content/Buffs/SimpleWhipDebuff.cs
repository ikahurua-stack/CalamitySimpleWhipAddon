using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.IO;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamitySimpleWhipAddon;
using CalamitySimpleWhipAddon.Content;
using CalamitySimpleWhipAddon.Content.Common;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamityMod.DataStructures;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamitySimpleWhipAddon.Content.Items.Weapons;
using CalamitySimpleWhipAddon.Content.Particles;
using CalamityMod;
using CalamityMod.Buffs;
using CalamityMod.Systems;
using CalamityMod.Systems.Collections;
using CalamityMod.Systems.Mechanic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using CalamitySimpleWhipAddon.Systems;
using CalamitySimpleWhipAddon.Content.Systems;


namespace CalamitySimpleWhipAddon.Content.Buffs
{
    public class SimpleWhipDebuffAc05 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffAcC05 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffAc05C05 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffAc07C07 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffAc10C10 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff07 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff08 : ModBuff
	{
		public override void SetStaticDefaults()
		{
			BuffID.Sets.IsATagBuff[Type] = true;
		}
	}

    public class SimpleWhipDebuff09 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff10 : ModBuff
	{
		public override void SetStaticDefaults()
		{
			BuffID.Sets.IsATagBuff[Type] = true;
		}
	}

    public class SimpleWhipDebuff11 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff12 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff13 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff14 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff15 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff20 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff25 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff27 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff30 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffT1 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffT2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff7D07C : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff7D07C2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff10D10C : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff15D15C : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff0D08C : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff0D09C : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff0D10C : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff0D11C : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff0D22C : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff0D27C : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuff2x : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffC100 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffC100x3 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx1 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx3 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx4 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx5 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx6 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx7 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx8 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx9 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx10 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx11 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx12 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx13 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx14 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx15 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx16 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx17 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx18 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx19 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx20 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx21 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx22 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffEx23 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffAcHo : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffAcHo2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffS1 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffS2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffS3 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffS4 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffS5 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffT1St : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffT2St : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffT2St2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffT3St : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffT4St : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffT5St : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffT10St : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SimpleWhipDebuffT20St : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }


    public class ChainExplosionGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int explosionHitCount = 0;

        public override void ResetEffects(NPC npc)
        {
            // 時間経過で少しずつリセットしたい場合はここで減らす
            if (explosionHitCount > 0)
                explosionHitCount--;
        }
    }

    // NPCごとのクールダウン管理
    public class GateTagNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int gateCooldown = 0;

        public override void ResetEffects(NPC npc)
        {
            if (gateCooldown > 0)
                gateCooldown--;
        }
    }


    public class StackTagNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int[] stackTimers = new int[1000];
        public int StackCount = 0;

        public void AddStack(int time)
        {
            if (StackCount < stackTimers.Length)
            {
                stackTimers[StackCount] = time;
                StackCount++;
            }
        }

        public override void PostAI(NPC npc)
        {
            for (int i = StackCount - 1; i >= 0; i--)
            {
                stackTimers[i]--;
                if (stackTimers[i] <= 0)
                {
                    stackTimers[i] = stackTimers[StackCount - 1];
                    StackCount--;
                }
            }
        }
    }

    public class StackTagNPC2 : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public const int MaxGlobalStacks = 100;

        public static Queue<(int npcID, int projID, int wormID, int index)> GlobalQueue = new();

        public int[] stackTimers = new int[1000];
        public int[] mothIDs = new int[1000];
        public int StackCount = 0;

        public int mothType = -1;
        public int buffType = -1;

        private static readonly HashSet<int> StackWhipBuffs = new()
        {
            ModContent.BuffType<SimpleWhipDebuffT2St>(),
            ModContent.BuffType<SimpleWhipDebuffT3St>(),
            ModContent.BuffType<SimpleWhipDebuffT5St>(),
            ModContent.BuffType<SimpleWhipDebuffT10St>(),
            ModContent.BuffType<SimpleWhipDebuffT20St>(),
        };

        private static void CleanQueue()
        {
            int count = GlobalQueue.Count;

            for (int i = 0; i < count; i++)
            {
                var entry = GlobalQueue.Dequeue();

                if (entry.projID < 0 || entry.projID >= Main.maxProjectiles)
                    continue;

                Projectile proj = Main.projectile[entry.projID];

                if (!proj.active)
                    continue;

                GlobalQueue.Enqueue(entry);
            }
        }

        private static int CountGlobalStacks()
        {
            return GlobalQueue.Count;
        }

        private static int CountWormStacks(int wormID)
        {
            int count = 0;

            foreach (var entry in GlobalQueue)
            {
                if (entry.wormID == wormID)
                    count++;
            }

            return count;
        }

        public void AddStack(NPC npc, int time, int owner, int projectileType, int debuffType)
        {
            bool isWorm = WormLikeNPCUtils.IsLargeWormLike(npc);
            int wormID = npc.realLife >= 0 ? npc.realLife : npc.whoAmI;


            CleanQueue();

            if (isWorm && StackCount > 0)
                return;

            if (isWorm && CountWormStacks(wormID) >= 70)
                return;

            while (CountGlobalStacks() >= MaxGlobalStacks && GlobalQueue.Count > 0)
            {
                var oldest = GlobalQueue.Dequeue();

                if (oldest.projID >= 0 && oldest.projID < Main.maxProjectiles)
                {
                    Projectile proj = Main.projectile[oldest.projID];

                    if (proj.active)
                        proj.Kill();
                }

                if (oldest.npcID >= 0 && oldest.npcID < Main.maxNPCs)
                {
                    NPC targetNPC = Main.npc[oldest.npcID];

                    if (targetNPC.active)
                    {
                        var modNPC = targetNPC.GetGlobalNPC<StackTagNPC2>();

                        for (int i = 0; i < modNPC.StackCount; i++)
                        {
                            if (modNPC.mothIDs[i] == oldest.projID)
                            {
                                modNPC.stackTimers[i] = modNPC.stackTimers[modNPC.StackCount - 1];
                                modNPC.mothIDs[i] = modNPC.mothIDs[modNPC.StackCount - 1];
                                modNPC.StackCount--;
                                break;
                            }
                        }
                    }
                }
            }

            if (StackCount >= stackTimers.Length)
                return;

            mothType = projectileType;
            buffType = debuffType;

            stackTimers[StackCount] = time;

            if (Main.myPlayer == owner)
            {
                var p = Projectile.NewProjectileDirect(
                    npc.GetSource_FromAI(),
                    npc.Center,
                    Vector2.Zero,
                    projectileType,
                    0,
                    0,
                    owner,
                    npc.whoAmI,
                    0f,
                    Main.rand.Next()
                );

                mothIDs[StackCount] = p.whoAmI;

                GlobalQueue.Enqueue((npc.whoAmI, p.whoAmI, wormID, StackCount));
            }

            StackCount++;
        }

        public override void PostAI(NPC npc)
        {
            if (StackCount > 0 && HasNonStackableExternalWhipTag(npc))
            {
                ClearStacks(npc);
                return;
            }

            for (int i = StackCount - 1; i >= 0; i--)
            {
                stackTimers[i]--;

                if (stackTimers[i] <= 0)
                {
                    int id = mothIDs[i];

                    if (id >= 0 && id < Main.maxProjectiles)
                    {
                        Projectile proj = Main.projectile[id];

                        if (proj.active && proj.type == mothType)
                        {
                            proj.Kill();

                        }
                    }

                    stackTimers[i] = stackTimers[StackCount - 1];
                    mothIDs[i] = mothIDs[StackCount - 1];
                    StackCount--;
                }
            }

        }

        private static bool HasNonStackableExternalWhipTag(NPC npc)
        {
            for (int i = 0; i < npc.buffType.Length; i++)
            {
                int buffType = npc.buffType[i];

                if (buffType <= 0)
                    continue;

                // 自分のスタック系タグは無視する。
                if (StackWhipBuffs.Contains(buffType))
                    continue;

                if (buffType >= 0 && buffType < CalamityBuffSets.SummonTagDebuff.Length)
                {
                    SummonTag tag = CalamityBuffSets.SummonTagDebuff[buffType];

                    if (tag != null && !tag.AllowsWhipStacking)
                        return true;
                }
            }

            return false;
        }

        private void ClearStacks(NPC npc)
        {
            for (int i = 0; i < StackCount; i++)
            {
                int id = mothIDs[i];

                if (id >= 0 && id < Main.maxProjectiles)
                {
                    Projectile proj = Main.projectile[id];

                    if (proj.active && proj.type == mothType)
                        proj.Kill();
                }
            }

            StackCount = 0;

            int count = GlobalQueue.Count;

            for (int i = 0; i < count; i++)
            {
                var entry = GlobalQueue.Dequeue();

                if (entry.npcID != npc.whoAmI)
                    GlobalQueue.Enqueue(entry);
            }
        }

    }




    public class SimpleWhipDebuffNPC1 : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool tagged;

        public override void ResetEffects(NPC npc)
        {
            tagged = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.HasBuff(ModContent.BuffType<Buffs.SimpleWhipDebuffEx10>()))
                tagged = true;
        }


        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry)
                return;

            if (!tagged)
                return;

            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);
            int laserDamage = (int)(damageDone * (isLargeWorm ? 0.20f : 0.40f));

            int shotCount = 1;
            float speed = 8f;

            for (int i = 0; i < shotCount; i++)
            {
                float angle = MathHelper.TwoPi * i / shotCount + Main.rand.NextFloat(-0.4f, 0.4f);
                Vector2 velocity = angle.ToRotationVector2() * speed;

                int proj = Projectile.NewProjectile(
                    Projectile.GetSource_None(),
                    npc.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<MandibleLash_Shot>(),
                    laserDamage,
                    0f,
                    Main.myPlayer,
                    npc.whoAmI // ai[0] にターゲット保存
                );

                Main.projectile[proj].rotation =
                    Main.projectile[proj].velocity.ToRotation();
            }

            int buffType = ModContent.BuffType<SimpleWhipDebuffEx10>();
            int index = npc.FindBuffIndex(buffType);

            if (index != -1)
            {
                npc.DelBuff(index);
            }

            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // サーバー → クライアントにフラグを通知
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
            packet.Write(npc.whoAmI);
            packet.Write(buffType);
            packet.Send(); // 全クライアントに送信
        }
    }

    public class SimpleWhipDebuffNPC2 : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool tagged;

        public override void ResetEffects(NPC npc)
        {
            tagged = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.HasBuff(ModContent.BuffType<Buffs.SimpleWhipDebuffEx11>()))
                tagged = true;
        }


        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry)
                return;

            if (!tagged)
                return;

            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);
            int laserDamage = (int)(damageDone * (isLargeWorm ? 0.20f : 0.40f));

            int shotCount = 1;
            float speed = 8f;

            for (int i = 0; i < shotCount; i++)
            {
                float angle = MathHelper.TwoPi * i / shotCount + Main.rand.NextFloat(-0.4f, 0.4f);
                Vector2 velocity = angle.ToRotationVector2() * speed;

                int proj = Projectile.NewProjectile(
                    Projectile.GetSource_None(),
                    npc.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<DarkMandibleLash_Shot>(),
                    laserDamage,
                    0f,
                    Main.myPlayer,
                    npc.whoAmI // ai[0] にターゲット保存
                );

                Main.projectile[proj].rotation =
                    Main.projectile[proj].velocity.ToRotation();
            }
            
            int buffType = ModContent.BuffType<SimpleWhipDebuffEx11>();
            int index = npc.FindBuffIndex(buffType);

            if (index != -1)
            {
                npc.DelBuff(index);
            }

            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // サーバー → クライアントにフラグを通知
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
            packet.Write(npc.whoAmI);
            packet.Write(buffType);
            packet.Send(); // 全クライアントに送信
        }
    }

    

}