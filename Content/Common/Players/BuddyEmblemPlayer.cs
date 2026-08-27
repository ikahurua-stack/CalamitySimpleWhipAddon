using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using System.Collections.Generic;
using CalamitySimpleWhipAddon.Content.Buffs;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Summon.MirrorofKalandraMinions;

namespace CalamitySimpleWhipAddon.Content.Common.Players
{
    public class BuddyEmblemPlayer : ModPlayer
    {
        public bool buddyEmblem;
        public bool SoloBonusActive;
        public bool HalfBonusActive;
        public float BuddyEmblemBonus = 1f;
        public float BuddyEmblemHalfBonus = 1f;

        // ---- 1体ボーナスを受けられないミニオン ----
        private static readonly HashSet<int> NoSoloBonusMinions = new()
        {
            ProjectileID.StardustDragon1,
            ProjectileID.StardustDragon2,
            ProjectileID.StardustDragon3,
            ProjectileID.StardustDragon4,
            ProjectileID.WhiteTigerPounce,
            ProjectileID.StormTigerAttack,
            ProjectileID.StormTigerTier1,
            ProjectileID.StormTigerTier2,
            ProjectileID.StormTigerTier3,
            ProjectileID.StormTigerGem,
        };

        private static readonly HashSet<int> HalfBonusProjectiles = new()
        {
            ModContent.ProjectileType<SunSpiritMinion>(),
            ModContent.ProjectileType<AtzirisDisfavor>(),
            ModContent.ProjectileType<VengefulSunSpiritMinion>(),
            ModContent.ProjectileType<SiriusMinion>(),
        };

        private static readonly Dictionary<int, float> SoloSlotFactors = new()
        {
            { 1, 1.00f },
            { 2, 0.75f },
            { 3, 0.62f },
            { 4, 0.53f },
            { 5, 0.46f },
            { 6, 0.40f },
            { 7, 0.36f },
            { 8, 0.32f },
            { 9, 0.29f },
            { 10, 0.26f },
        };

        public override void ResetEffects()
        {
            buddyEmblem = false;
        }

        public override void PostUpdateMiscEffects()
        {
            SoloBonusActive = false;
            HalfBonusActive = false;

            if (!buddyEmblem)
            {
                BuddyEmblemBonus = 1f;
                Player.ClearBuff(ModContent.BuffType<BuddyEmblemBuff>());
                Player.ClearBuff(ModContent.BuffType<BuddyEmblemSoloBuff>());
                Player.ClearBuff(ModContent.BuffType<BuddyEmblemSoloHalfBuff>());
                return;
            }

            float used = Player.slotsMinions;
            if (used <= 0f)
            {
                BuddyEmblemBonus = 1f;
                return;
            }

            float max = Player.maxMinions;
            float empty = Math.Max(0f, max - used);

            // ---- 基本計算（凹関数＋多体減衰）----
            const float A = 0.7f;
            const float B = 0.85f;

            float slotFactor = (float)Math.Sqrt(empty / used);

            float bonus =
                1f + slotFactor * A / (1f + (used - 1f) * B);



            // ---- 1体限定の追加強化（スロット数別）----
            if (CanApplySoloBonus())
            {
                const float soloMin = 1.3f;
                const float soloMax = 2.9f;

                float t = (max - 2f) / 8f;
                t = Math.Clamp(t, 0f, 1f);

                float soloExtra = soloMin + (soloMax - soloMin) * t;

                int slot = (int)used;

                if (SoloSlotFactors.TryGetValue(slot, out float factor) &&
                    IsSingleMinionWithSlots(slot))
                {
                    bonus *= 1f + (soloExtra - 1f) * factor;
                    SoloBonusActive = true;
                }
            }

            bool hasHalfBonusProjectile = false;
            foreach (int projType in HalfBonusProjectiles)
            {
                if (Player.ownedProjectileCounts[projType] > 0)
                {
                    hasHalfBonusProjectile = true;
                    break;
                }
            }

            if (hasHalfBonusProjectile)
            {
                BuddyEmblemHalfBonus = bonus * 0.5f;
                HalfBonusActive = true;
            }

            BuddyEmblemBonus = bonus;

            int normal = ModContent.BuffType<BuddyEmblemBuff>();
            int solo = ModContent.BuffType<BuddyEmblemSoloBuff>();
            int half = ModContent.BuffType<BuddyEmblemSoloHalfBuff>();

            Player.ClearBuff(normal);
            Player.ClearBuff(solo);
            Player.ClearBuff(half);
            Player.AddBuff(SoloBonusActive ? (HalfBonusActive ? half : solo) : normal, 2);
        }

        private bool IsSingleMinionWithSlots(int requiredSlots)
        {
            int count = 0;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];

                if (!p.active)
                    continue;

                if (p.owner != Player.whoAmI)
                    continue;

                if (!p.minion)
                    continue;

                // ★ スロット消費0のミニオンは判定から除外
                if (p.minionSlots <= 0f)
                    continue;

                count++;

                // 指定スロット数と一致しなければアウト
                if ((int)p.minionSlots != requiredSlots)
                    return false;

                // 2体以上いたら即アウト
                if (count > 1)
                    return false;
            }

            return count == 1;
        }


        private bool CanApplySoloBonus()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (!p.active)
                    continue;

                if (p.owner != Player.whoAmI)
                    continue;

                if (!p.minion)
                    continue;

                if (NoSoloBonusMinions.Contains(p.type))
                    return false;
            }

            return true;
        }

    }
}
