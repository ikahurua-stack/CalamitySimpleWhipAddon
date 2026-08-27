using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using CalamitySimpleWhipAddon.Content.Common;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Buffs;
using CalamityMod.Projectiles.Summon;
using CalamityMod.NPCs;
using System;
using System.Collections.Generic;
using CalamityMod.Projectiles.Ranged;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamitySimpleWhipAddon.Content.DamageClasses;

namespace CalamitySimpleWhipAddon.Content.Common.WhipGlobalProjectile
{
    // -------------------------------
    // Whip ownerHitCheck fix
    // -------------------------------
    public class WhipGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private bool applied;

        public override void AI(Projectile projectile)
        {
            if (applied)
                return;

            if (!ProjectileID.Sets.IsAWhip[projectile.type])
                return;

            var mp = Main.player[projectile.owner]
                .GetModPlayer<WhipAccessoryPlayer>();

            if (mp.necromanticGrip || mp.emperorsGrip || mp.lightSpiritGrip || mp.airflowGrip)
            {
                projectile.ownerHitCheck = false;
                applied = true;
            }
        }
    }

    


    // -------------------------------
    // Summon homing
    // -------------------------------
    public class SummonShotHomingGlobal : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private HashSet<int> hitNPCs;

        private int homingCooldown;

        private int lastHitNPC = -1;
        private bool waitingToLeaveTarget;


        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.npcProj)
                return;

            if (projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass>() && projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>())
                return;

            if (projectile.DamageType == ModContent.GetInstance<PseudoSummonDamageClass>() || projectile.DamageType == ModContent.GetInstance<PseudoSummonDamageClass2>())
            {
                hitNPCs = null;

                if (projectile.velocity.Length() > 20f)
                {
                    // 弾速が早すぎる場合はクールダウンなし
                    homingCooldown = 0;
                }
                else
                {
                    // 通常の速度の弾はクールダウンを5に設定
                    homingCooldown = 5;
                }
            }
        }


        public override void OnHitNPC(
            Projectile projectile,
            NPC target,
            NPC.HitInfo hit,
            int damageDone)
        {
            if (projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass>() && projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>())
                return;

            NPC tagTarget = FindTaggedNPC(projectile);
            if (tagTarget == null)
                return;


            if (projectile.penetrate == -1 || projectile.localNPCHitCooldown == -1)
            {
                hitNPCs ??= new HashSet<int>();
                hitNPCs.Add(target.whoAmI);
            }
            else
            {
                lastHitNPC = target.whoAmI;
                waitingToLeaveTarget = true;

                projectile.velocity *= 1.2f;
            }
        }

        public override void AI(Projectile projectile)
        {
            if (projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass>() && projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>())
                return;

            NPC target = FindTaggedNPC(projectile);
            if (target == null)
                return;

            float speed = projectile.velocity.Length();
            if (speed < 12f)
                speed = 12f;
            else if (speed < 18f)
                speed = 18f;


            if (waitingToLeaveTarget)
            {
                if (lastHitNPC < 0 ||
                    !Main.npc[lastHitNPC].active)
                {
                    waitingToLeaveTarget = false;
                }
                else
                {
                    NPC npc = Main.npc[lastHitNPC];

                    float dist =
                        Vector2.Distance(
                            projectile.Center,
                            npc.Center) - npc.Size.Length() * 0.5f;

                    // 60ピクセル以上離れたら追尾再開
                    if (dist >= 60f)
                        waitingToLeaveTarget = false;
                }
            }

            if (waitingToLeaveTarget)
                return;

            if (homingCooldown > 0)
            {
                homingCooldown--;
                return;
            }

            Vector2 desiredVelocity =
                projectile.DirectionTo(target.Center) * speed;

            projectile.velocity = Vector2.Lerp(
                projectile.velocity,
                desiredVelocity,
                0.125f
            );
        }

        private NPC FindTaggedNPC(Projectile proj)
        {
            Player player = Main.player[proj.owner];
            int targetIndex = player.MinionAttackTargetNPC;

            if (targetIndex >= 0 && (hitNPCs == null || !hitNPCs.Contains(targetIndex)))
            {
                NPC npc = Main.npc[targetIndex];
                if (npc.active && !npc.friendly)
                {
                    if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffAcHo>()) &&
                        Vector2.Distance(proj.Center, npc.Center) <= 500f)
                        return npc;

                    if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffAcHo2>()) &&
                        Vector2.Distance(proj.Center, npc.Center) <= 400f)
                        return npc;
                }
            }

            float maxDist = 500f;
            float maxDist2 = 400f;
            NPC best = null;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (hitNPCs != null && hitNPCs.Contains(i))
                    continue;

                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly)
                    continue;

                float dr = npc.GetGlobalNPC<CalamityGlobalNPC>().DR;

                if (dr >= 0.9f)
                    continue;

                float dist = Vector2.Distance(proj.Center, npc.Center);

                if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffAcHo>()))
                {
                    if (dist < maxDist)
                    {
                        maxDist = dist;
                        best = npc;
                    }
                }
                else if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffAcHo2>()))
                {
                    if (dist < maxDist2)
                    {
                        maxDist2 = dist;
                        best = npc;
                    }
                }
            }

            return best;
        }
    }

    // -------------------------------
    // Summon damage decay (AcHo)
    // -------------------------------
    public class SummonProjectileDecay : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private int onHitCount;

        private bool multiPenetrate = false;

        private bool infiPenetrate = false;

        private bool Confirmed = false;

        public override void OnHitNPC(
            Projectile projectile,
            NPC target,
            NPC.HitInfo hit,
            int damageDone)
        {
            if (projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass>() && projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>())
                return;

            if (!target.HasBuff(ModContent.BuffType<SimpleWhipDebuffAcHo>()))
                return;

            onHitCount++;

            if (Confirmed || projectile.penetrate == 1)
                return;

            if (projectile.penetrate >= 2)
            {
                multiPenetrate = true;
            }
            else if (projectile.penetrate == -1)
            {
                infiPenetrate = true;
            }

            Confirmed = true;

        }

        public override void ModifyHitNPC(
            Projectile projectile,
            NPC target,
            ref NPC.HitModifiers modifiers)
        {
            if (projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass>() && projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>())
                return;

            if (!target.HasBuff(ModContent.BuffType<SimpleWhipDebuffAcHo>()))
                return;

            if (projectile.type == ModContent.ProjectileType<DaedalusPellet>() ||
                projectile.type == ModContent.ProjectileType<GastricBelcherBubble>() ||
                projectile.type == ModContent.ProjectileType<GastricBelcherVomit>())
            {
                modifiers.SourceDamage *= 0.7f;
            }
            else if (projectile.type == ModContent.ProjectileType<DaedalusLightning>())
            {
                modifiers.SourceDamage *= 0.5f;
            }
            else if (projectile.type == ModContent.ProjectileType<AmphibiansGuitarProjectile>())
            {
                modifiers.SourceDamage *= 0.65f;
            }
            else if (projectile.type == ModContent.ProjectileType<BloodRain>())
            {
                modifiers.SourceDamage *= 0.15f;
            }

            if (multiPenetrate && projectile.DamageType == ModContent.GetInstance<PseudoSummonDamageClass2>())
            {
                // ヒット回数に応じてダメージを減衰させる倍率
                float multiplier = MathF.Pow(0.55f, onHitCount);

                // 下がりすぎ防止の最低保証（1%）
                if (multiplier < 0.01f)
                    multiplier = 0.01f;

                if (projectile.TryGetGlobalProjectile<SummonProjectileDecayTag>(out var globalProj))
                {
                    globalProj.stackWhip = 0.50f * multiplier;
                }

                // ダメージに倍率を適用
                modifiers.SourceDamage *= 0.9f;
                modifiers.SourceDamage *= multiplier;
            }
            else if (multiPenetrate && projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>())
            {
                float multiplier = MathF.Pow(0.3f, onHitCount);

                if (multiplier < 0.01f)
                    multiplier = 0.01f;

                if (projectile.TryGetGlobalProjectile<SummonProjectileDecayTag>(out var globalProj))
                {
                    globalProj.stackWhip = 0.50f * multiplier;
                }

                modifiers.SourceDamage *= 0.9f;
                modifiers.SourceDamage *= multiplier;
            }
            else if(infiPenetrate && projectile.DamageType == ModContent.GetInstance<PseudoSummonDamageClass2>())
            {
                float multiplier = MathF.Pow(0.7f, onHitCount);

                if (multiplier < 0.01f)
                    multiplier = 0.01f;

                if (projectile.TryGetGlobalProjectile<SummonProjectileDecayTag>(out var globalProj))
                {
                    globalProj.stackWhip = 1f * multiplier;
                }

                modifiers.SourceDamage *= 0.9f;
                modifiers.SourceDamage *= multiplier;
            }
            else if (infiPenetrate && projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>())
            {
                float multiplier = MathF.Pow(0.65f, onHitCount);

                if (multiplier < 0.01f)
                    multiplier = 0.01f;

                if (projectile.TryGetGlobalProjectile<SummonProjectileDecayTag>(out var globalProj))
                {
                    globalProj.stackWhip = 1f * multiplier;
                }

                modifiers.SourceDamage *= 0.9f;
                modifiers.SourceDamage *= multiplier;
            }
            else
            {
                modifiers.SourceDamage *= 0.9f;
            }
        }
    }

    // -------------------------------
    // Summon damage decay (AcHo2)
    // -------------------------------
    public class SummonProjectileDecay2 : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private int onHitCount;

        private bool multiPenetrate = false;

        private bool infiPenetrate = false;

        private bool Confirmed = false;

        public override void OnHitNPC(
            Projectile projectile,
            NPC target,
            NPC.HitInfo hit,
            int damageDone)
        {
            if (projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass>() && projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>())
                return;

            if (!target.HasBuff(ModContent.BuffType<SimpleWhipDebuffAcHo2>()))
                return;

            onHitCount++;

            if (Confirmed || projectile.penetrate == 1)
                return;

            if (projectile.penetrate >= 2)
            {
                multiPenetrate = true;
            }
            else if (projectile.penetrate == -1)
            {
                infiPenetrate = true;
            }

            Confirmed = true;

        }

        public override void ModifyHitNPC(
            Projectile projectile,
            NPC target,
            ref NPC.HitModifiers modifiers)
        {
            if (projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass>() && projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>())
                return;

            if (!target.HasBuff(ModContent.BuffType<SimpleWhipDebuffAcHo2>()))
                return;

            if (projectile.type == ModContent.ProjectileType<DaedalusPellet>() ||
                projectile.type == ModContent.ProjectileType<GastricBelcherBubble>() ||
                projectile.type == ModContent.ProjectileType<GastricBelcherVomit>())
            {
                modifiers.SourceDamage *= 0.7f;
            }
            else if (projectile.type == ModContent.ProjectileType<DaedalusLightning>())
            {
                modifiers.SourceDamage *= 0.5f;
            }
            else if (projectile.type == ModContent.ProjectileType<AmphibiansGuitarProjectile>())
            {
                modifiers.SourceDamage *= 0.65f;
            }
            else if (projectile.type == ModContent.ProjectileType<BloodRain>())
            {
                modifiers.SourceDamage *= 0.15f;
            }

            if (multiPenetrate && projectile.DamageType == ModContent.GetInstance<PseudoSummonDamageClass2>())
            {
                float multiplier = MathF.Pow(0.35f, onHitCount);

                if (multiplier < 0.01f)
                    multiplier = 0.01f;

                if (projectile.TryGetGlobalProjectile<SummonProjectileDecayTag>(out var globalProj))
                {
                    globalProj.stackWhip = 0.50f * multiplier;
                }

                modifiers.SourceDamage *= 0.85f;
                modifiers.SourceDamage *= multiplier;
            }
            else if (multiPenetrate && projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>())
            {
                float multiplier = MathF.Pow(0.15f, onHitCount);

                if (multiplier < 0.01f)
                    multiplier = 0.01f;

                if (projectile.TryGetGlobalProjectile<SummonProjectileDecayTag>(out var globalProj))
                {
                    globalProj.stackWhip = 0.50f * multiplier;
                }

                modifiers.SourceDamage *= 0.85f;
                modifiers.SourceDamage *= multiplier;
            }
            else if (infiPenetrate && projectile.DamageType == ModContent.GetInstance<PseudoSummonDamageClass2>())
            {
                float multiplier = MathF.Pow(0.55f, onHitCount);

                if (multiplier < 0.01f)
                    multiplier = 0.01f;

                if (projectile.TryGetGlobalProjectile<SummonProjectileDecayTag>(out var globalProj))
                {
                    globalProj.stackWhip = 1f * multiplier;
                }

                modifiers.SourceDamage *= 0.85f;
                modifiers.SourceDamage *= multiplier;
            }
            else if (infiPenetrate && projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>())
            {
                float multiplier = MathF.Pow(0.45f, onHitCount);

                if (multiplier < 0.01f)
                    multiplier = 0.01f;

                if (projectile.TryGetGlobalProjectile<SummonProjectileDecayTag>(out var globalProj))
                {
                    globalProj.stackWhip = 1f * multiplier;
                }

                modifiers.SourceDamage *= 0.85f;
                modifiers.SourceDamage *= multiplier;
            }
            else
            {
                modifiers.SourceDamage *= 0.85f;
            }
        }
    }

    public class DazzlingStabberDecay : GlobalProjectile
    {
        public override bool InstancePerEntity => true;


        public override void ModifyHitNPC(
            Projectile projectile,
            NPC target,
            ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ModContent.ProjectileType<DazzlingStabber>())
            {
                if (projectile.TryGetGlobalProjectile<SummonProjectileDecayTag>(out var globalProj))
                {
                    switch ((int)projectile.ai[2])
                    {
                        case 1: // Crystal
                            globalProj.stackWhip = 1f;
                            break;

                        case 2: // Stone
                            globalProj.stackWhip = 3f;
                            break;

                        case 3: // Fire
                            globalProj.stackWhip = 2f;
                            break;
                    }
                }
            }
        }
    }

    public class SummonProjectileDecayTag : GlobalProjectile
    {
        // GlobalProjectileは射撃物ごとに独立した実体（インスタンス）を持つため、混ざりません
        public override bool InstancePerEntity => true;

        // 弾ごとに個別に保持される鞭タグの倍率（初期値は 1.0 = 100%）
        public float stackWhip = 1f;
    }

    public class SummonTagDecayGlobalNPC : GlobalNPC
    {
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            // 1. ミニオンまたはミニオンの射撃物でなければ処理を終える
            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

            // 2. 攻撃した弾から、個別の stackWhip（減衰倍率）の値を取得する
            float projTagMultiplier = 1f; // デフォルトは1倍（減衰なし）
            if (projectile.TryGetGlobalProjectile<SummonProjectileDecayTag>(out var globalProj))
            {
                projTagMultiplier = globalProj.stackWhip;
            }

            // もし減衰フラグが立っていない（1倍のまま）なら、これ以降の処理はスキップする
            if (projTagMultiplier >= 1f)
                return;

            // .Value を使って現在のタグダメージの数値を float で安全に取得
            float currentTagValue = modifiers.FlatBonusDamage.Value;

            // タグダメージが存在する場合のみ減衰させる
            if (currentTagValue > 0f)
            {
                // 例: 現在のタグが 10 で、倍率が 0.1f なら、最終的なダメージは 1。
                // つまり、差分である「9」を現在の数値から引けば良いことになります。
                float difference = currentTagValue - (currentTagValue * projTagMultiplier);

                // AddableFloat は float の「足し算（+=）」のみ対応しているため、
                // 計算した差分を【マイナス値】にして足し算することで、安全に引き算（相殺）を行います。
                modifiers.FlatBonusDamage += -difference;
            }
        }
    }


}
