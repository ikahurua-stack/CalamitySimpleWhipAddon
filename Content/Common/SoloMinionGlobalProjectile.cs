using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using System.Collections.Generic;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.DamageClasses;
using CalamitySimpleWhipAddon.Content.Common;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Summon.MirrorofKalandraMinions;
using CalamityMod.Projectiles.Summon.Umbrella;
using CalamityMod.Projectiles.DraedonsArsenal;
using CalamityMod.Projectiles.Summon.SmallAresArms;
using System;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Boss;

namespace CalamitySimpleWhipAddon.Content.Common
{
    public class BuddyEmblemGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private float baseScale = 0f;

        public bool FromMinion;
        public bool FromMinion2;
        public bool FromMinion3;
        public bool FromMinion4;

        public bool FromGunMinion;
        public bool FromGunMinion2;
        public bool FromGunMinion3;
        public bool FromGunMinion4;

        public bool SpawnedFromOnHit;
        public int OriginalProjectileType = -1;

        public bool isOk;


        private static readonly HashSet<int> HalfBonusProjectiles = new()
        {
            ModContent.ProjectileType<SunSpiritBeam>(),
            ModContent.ProjectileType<AtzirisDisfavor>(),
            ModContent.ProjectileType<VengefulSunBeam>(),
            ModContent.ProjectileType<SiriusQuasar>(),
            ModContent.ProjectileType<SiriusBeam>(),
        };

        private static readonly HashSet<int> NoScaleProjectiles = new()
        {
            ModContent.ProjectileType<AmphibiansGuitarProjectile>(),
            ModContent.ProjectileType<DaedalusLightning>(),
            ModContent.ProjectileType<EndoBeam>(),
            ModContent.ProjectileType<CannonLaserbeam>(),
            ModContent.ProjectileType<MountedScannerLaser>(),
            ModContent.ProjectileType<MidnightSunBeam>(),
        };



        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            // 【最速判定】敵の弾（npcProj）、または生成元（source）が空っぽの場合は、何もしないで即終了する
            if (projectile.npcProj || source == null || source is not EntitySource_Parent)
                return;


            SpawnedFromOnHit = source is EntitySource_OnHit;

            if (ExternalNominionProjectileRegistry.Contains(projectile.type))
                return;


            // 生成元が別の射撃物（Projectile）であるかをチェック
            if (source is EntitySource_Parent pSource && pSource.Entity is Projectile parentProjectile)
            {

                // 同一Projectileの再生成は連鎖させない
                if (parentProjectile.type == projectile.type)
                    return;

                // 親の GlobalProjectile コンポーネントを取得
                if (parentProjectile.TryGetGlobalProjectile(out BuddyEmblemGlobalProjectile parentGlobal))
                {
                    // 処理が連鎖して複数適用されないよう、else if で上から順番に判定を流します
                    if (parentGlobal.FromMinion3)
                    {
                        projectile.DamageType = ModContent.GetInstance<PseudoSummonDamageClass>();
                        FromMinion4 = true;

                        if (!projectile.stopsDealingDamageAfterPenetrateHits && !ProjectileID.Sets.MinionShot[projectile.type] && source is not EntitySource_OnHit)
                        {
                            projectile.minion = true;
                        }
                    }
                    else if (parentGlobal.FromGunMinion3)
                    {
                        projectile.DamageType = ModContent.GetInstance<PseudoSummonDamageClass2>();
                        FromGunMinion4 = true;

                        projectile.minion = true;
                    }
                    else if (parentGlobal.FromMinion2) // else if にしたことで、上の判定が通ったらここはスキップされて軽くなります
                    {
                        projectile.DamageType = ModContent.GetInstance<PseudoSummonDamageClass>();
                        FromMinion3 = true;

                        if (!projectile.stopsDealingDamageAfterPenetrateHits && !ProjectileID.Sets.MinionShot[projectile.type] && source is not EntitySource_OnHit)
                        {
                            projectile.minion = true;
                        }
                    }
                    else if (parentGlobal.FromGunMinion2)
                    {
                        projectile.DamageType = ModContent.GetInstance<PseudoSummonDamageClass2>();
                        FromGunMinion3 = true;

                        projectile.minion = true;
                    }
                    else if (parentGlobal.FromMinion)
                    {
                        projectile.DamageType = ModContent.GetInstance<PseudoSummonDamageClass>();
                        FromMinion2 = true;

                        if (!projectile.stopsDealingDamageAfterPenetrateHits && !ProjectileID.Sets.MinionShot[projectile.type] && source is not EntitySource_OnHit)
                        {
                            projectile.minion = true;
                        }
                    }
                    else if (parentGlobal.FromGunMinion)
                    {
                        projectile.DamageType = ModContent.GetInstance<PseudoSummonDamageClass2>();
                        FromGunMinion2 = true;

                        projectile.minion = true;
                    }
                }

                // 2. 親自身がミニオンスロットを持つミニオン本体である場合（最初の判定）
                if (parentProjectile.minionSlots > 0f)
                {
                    if (projectile.DamageType == DamageClass.Ranged)
                    {
                        projectile.DamageType = ModContent.GetInstance<PseudoSummonDamageClass2>();
                        FromGunMinion = true;

                        projectile.minion = true;
                    }
                    else
                    {
                        projectile.DamageType = ModContent.GetInstance<PseudoSummonDamageClass>();
                        FromMinion = true;

                        if (!projectile.stopsDealingDamageAfterPenetrateHits && !ProjectileID.Sets.MinionShot[projectile.type] && source is not EntitySource_OnHit)
                        {
                            OriginalProjectileType = projectile.type;
                            projectile.minion = true;
                        }
                    }
                }
            }
        }



        public override void ModifyHitNPC(
            Projectile projectile,
            NPC target,
            ref NPC.HitModifiers modifiers)
        {
            if (projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass>() && 
                projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>() && 
                !projectile.minion)
                return;

            if (projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass>() &&
                projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>() &&
                projectile.minionSlots <= 0f)
                return;


            Player player = Main.player[projectile.owner];
            if (!player.active)
                return;

            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
            if (!modPlayer.buddyEmblem)
                return;

            float bonus = modPlayer.BuddyEmblemBonus;

            // 半減対象プロジェクタイル
            if (HalfBonusProjectiles.Contains(projectile.type))
            {
                bonus *= 0.50f;
            }

            modifiers.SourceDamage *= bonus;
            modifiers.CritDamage *= 0.8f;
        }

        public override void AI(Projectile projectile)
        {

            if (!FromMinion && !FromMinion2 && !FromMinion3 && !FromMinion4 &&
                !FromGunMinion && !FromGunMinion2 && !FromGunMinion3 && !FromGunMinion4)
                return;

            if (isOk)
                return;

            if (OriginalProjectileType == projectile.type)
                return;

            if (ExternalNominionProjectileRegistry.Contains(projectile.type))
                return;

            if (FromMinion || FromMinion2 || FromMinion3 || FromMinion4)
            {
                projectile.DamageType = ModContent.GetInstance<PseudoSummonDamageClass>();

                if (!projectile.stopsDealingDamageAfterPenetrateHits && !ProjectileID.Sets.MinionShot[projectile.type] && !SpawnedFromOnHit)
                {
                    projectile.minion = true;
                }

                isOk = true;
            }
            else if (FromGunMinion || FromGunMinion2 || FromGunMinion3 || FromGunMinion4)
            {
                projectile.DamageType = ModContent.GetInstance<PseudoSummonDamageClass2>();

                projectile.minion = true;
                

                isOk = true;
            }


        }

        public override void PostAI(Projectile projectile)
        {
            if (projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass>() &&
                projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>() &&
                !projectile.minion)
                return;

            if (projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass>() &&
                projectile.DamageType != ModContent.GetInstance<PseudoSummonDamageClass2>() &&
                projectile.minionSlots <= 0f)
                return;

            if (NoScaleProjectiles.Contains(projectile.type))
                return;

            Player player = Main.player[projectile.owner];
            if (!player.active)
                return;

            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
            if (!modPlayer.buddyEmblem)
                return;

            // ---- 初期 scale を一度だけ保存 ----
            if (baseScale == 0f)
            {
                baseScale = Math.Abs(projectile.scale);
            }

            float sign = Math.Sign(projectile.scale);
            if (sign == 0f)
                sign = 1f;

            float bonus = modPlayer.BuddyEmblemBonus;

            float scaleMultiplier = MathHelper.Lerp(
                1f,
                1.5f,
                Math.Clamp((bonus - 1f) / 6.3f, 0f, 1f)
            );

            projectile.scale = sign * baseScale * scaleMultiplier;
        }

    }
}
