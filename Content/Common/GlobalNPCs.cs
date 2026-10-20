using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CalamitySimpleWhipAddon.Content.Buffs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Systems;
using CalamityMod.NPCs;
using CalamityMod.NPCs.DevourerofGods;

namespace CalamitySimpleWhipAddon.Content.Common.GlobalNPCs
{
    public class WhipGripGlobalNPC : GlobalNPC
    {
        private static int[] whipDebuffIDs;

        private static int[] GetWhipDebuffIDs()
        {
            if (whipDebuffIDs != null)
                return whipDebuffIDs;

            Mod whipAddon = ModContent.GetInstance<CalamitySimpleWhipAddon>();

            var ids = new List<int>();

            for (int i = 0; i < BuffLoader.BuffCount; i++)
            {
                ModBuff buff = BuffLoader.GetBuff(i);

                if (buff?.Mod == whipAddon)
                    ids.Add(i);
            }

            whipDebuffIDs = ids.ToArray();

            return whipDebuffIDs;
        }


        public override void OnHitByProjectile(
            NPC npc,
            Projectile projectile,
            NPC.HitInfo hit,
            int damageDone)
        {
            // ムチ以外は無視
            if (!ProjectileID.Sets.IsAWhip[projectile.type])
                return;

            // オーナー確認
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player player = Main.player[projectile.owner];
            if (!player.active)
                return;

            var gripPlayer = player.GetModPlayer<WhipAccessoryPlayer>();

            // Silk Grip
            if (gripPlayer.silkGrip)
            {
                npc.AddBuff(
                    ModContent.BuffType<SimpleWhipDebuffAcC05>(),
                    240
                );
            }

            // Leather Grip
            if (gripPlayer.leatherGrip)
            {
                npc.AddBuff(
                    ModContent.BuffType<SimpleWhipDebuffAc05>(),
                    240
                );
            }

            // Rubber Grip
            if (gripPlayer.rubberGrip)
            {
                npc.AddBuff(
                    ModContent.BuffType<SimpleWhipDebuffAc05C05>(),
                    240
                );
            }

            // Necromantic Grip
            if (gripPlayer.necromanticGrip)
            {
                npc.AddBuff(
                    ModContent.BuffType<SimpleWhipDebuffAc07C07>(),
                    240
                );
            }

            // Emperors Grip
            if (gripPlayer.emperorsGrip)
            {
                npc.AddBuff(
                    ModContent.BuffType<SimpleWhipDebuffAc10C10>(),
                    240
                );
            }

            // CommanderGrip
            if (gripPlayer.commanderGrip)
            {
                npc.AddBuff(
                    ModContent.BuffType<SimpleWhipDebuffAcHo>(),
                    240
                );
            }

            // CommanderGrip
            if (gripPlayer.magneticGrip)
            {
                npc.AddBuff(
                    ModContent.BuffType<SimpleWhipDebuffAcHo2>(),
                    240
                );
            }
        }

        public override void PostAI(NPC npc)
        {
            int[] ids = GetWhipDebuffIDs();

            bool[] immune = npc.buffImmune;

            for (int i = 0; i < ids.Length; i++)
            {
                int id = ids[i];

                if (id < immune.Length)
                    immune[id] = false;
            }
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (!InfernalEclipseCompatibility.IsEnabled ||
                (!npc.boss && npc.type != NPCID.EaterofWorldsHead &&
                 npc.type != ModContent.NPCType<CalamityMod.NPCs.AquaticScourge.AquaticScourgeHead>()))
            {
                return;
            }

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active)
                    player.GetModPlayer<WhipShieldPlayer>().ResetShieldForBoss();
            }
        }


    }

}

