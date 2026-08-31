using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria.ID;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Systems.Collections;
using CalamityMod.Systems;
using CalamityMod;
using CalamityMod.Utilities;
using CalamityMod.Particles;
using Terraria.ModLoader.IO;
using CalamityMod.Cooldowns;
using CalamitySimpleWhipAddon.Content.Cooldowns;
using CalamitySimpleWhipAddon.Content;

namespace CalamitySimpleWhipAddon.Content.Common.Players
{
    public class WhipShieldPlayer : ModPlayer
    {
        public int rechargeCooldown = 0;
        public int rechargeCooldownMax = 0;

        public bool freeDodgeFromShieldAbsorption = false;

        public bool nextHitDealsDefenseDamage = false;

        private Vector2 lastHitSourcePosition;

        public int charge;
        public int shieldLife;
        public int cooldown;

        public int shieldTier = 0;

        public int MaxShield
        {
            get
            {
                return shieldTier switch
                {
                    1 => 50,
                    2 => 100,
                    3 => 150,
                    4 => 200,
                    5 => 250,
                    _ => 50
                };
            }
        }

        public int MaxCharge
        {
            get
            {
                return shieldTier switch
                {
                    1 => 50,
                    2 => 100,
                    3 => 150,
                    4 => 200,
                    5 => 250,
                    _ => 50
                };
            }
        }

        public int shieldHitTimer = 0;

        public bool Charging => charge > 0 && shieldLife <= 0;

        private int shieldProjID = -1;

        public int orbSpawnCooldown = 0;

        public void SetShieldTier(int newTier)
        {
            if (newTier == shieldTier)
                return;

            shieldTier = newTier;

            // rechargeCooldownは維持
            shieldLife = 0;
            charge = 0;
        }

        public void AddCharge(int amount)
        {
            if (cooldown > 0)
                return;

            if (rechargeCooldown > 0)
                return;


            // シールド展開中 → 回復
            if (shieldLife > 0)
            {
                shieldLife += amount;

                if (shieldLife > MaxShield)
                    shieldLife = MaxShield;

                Player.AddCooldown(ShieldConduitDurability.ID, shieldLife);
                return;
            }

            // シールド未展開 → チャージ
            charge += amount;

            if (charge >= MaxCharge)
            {
                charge = 0;
                shieldLife = MaxShield;
            }

            Player.AddCooldown(ShieldConduitRecharge.ID, charge);
        }


        public override void ResetEffects()
        {
            if (orbSpawnCooldown > 0)
                orbSpawnCooldown--;

        }

        public override void PostUpdate()
        {
            if (rechargeCooldown > 0) rechargeCooldown--;
            if (shieldHitTimer > 0) shieldHitTimer--;
            if (cooldown > 0) cooldown--;

            if (charge > 0 && shieldLife <= 0)
            {
                if (!Player.HasCooldown(ShieldConduitRecharge.ID))
                    Player.AddCooldown(ShieldConduitRecharge.ID, charge);
            }

            // ⭐ シールドのプロジェクタイルタイプを取得
            int shieldType = ModContent.ProjectileType<Projectiles.WhipShieldProjectile>();

            if (shieldLife > 0)
            {
                Player.AddCooldown(ShieldConduitDurability.ID, shieldLife);

                // ⭐ 自分がオーナーのシールドが「0個」かつ「自分が操作するプレイヤー」なら生成
                if (Player.ownedProjectileCounts[shieldType] <= 0 && Main.myPlayer == Player.whoAmI)
                {
                    Projectile.NewProjectile(
                        Player.GetSource_Misc("WhipShield"),
                        Player.Center,
                        Vector2.Zero,
                        shieldType,
                        0,
                        0f,
                        Player.whoAmI
                    );
                }
            }
            else
            {
                // ⭐ 耐久値が0になったら、自分がオーナーのシールドをすべて消す
                if (Main.myPlayer == Player.whoAmI && Player.ownedProjectileCounts[shieldType] > 0)
                {
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        Projectile p = Main.projectile[i];
                        if (p.active && p.type == shieldType && p.owner == Player.whoAmI)
                        {
                            p.Kill();
                        }
                    }
                }
            }
        }


        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            lastHitSourcePosition = npc.Center;
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (shieldLife > 0)
            {
                modifiers.ModifyHurtInfo += ModifyHurtInfo_Shield;
            }
        }

        private void ModifyHurtInfo_Shield(ref Player.HurtInfo info)
        {
            bool shieldsFullyAbsorbedHit = false;
            bool shieldsTookHit = false;
            bool anyShieldBroke = false;
            int totalDamageBlocked = 0;
            var cal = Player.Calamity();

            if (cal.RoverDriveShieldDurability > 0 || cal.LunicCorpsShieldDurability > 0 || cal.SpongeShieldDurability > 0 || cal.pSoulShieldDurability > 0 || cal.Starshield > 0 && cal.StratusStarburst > 0)
            {
                return;
            }

            if (cal.chaliceOfTheBloodGod)
            {
                info.Damage = cal.chaliceHitOriginalDamage;
            }


            if (shieldLife > 0 && !shieldsFullyAbsorbedHit)
            {
                int shieldLifeBeforeHit = shieldLife;

                bool thisShieldCanFullyAbsorb = shieldLife >= info.Damage;


                int shieldDamageBlocked = Math.Min(shieldLife, info.Damage);
                totalDamageBlocked += shieldDamageBlocked;


                shieldLife -= info.Damage;
                float shieldLifeRatio = (float)shieldLife / MaxShield;

                // 5秒～50秒の範囲
                int minCooldown = 60 * 5;   // 5秒
                int maxCooldown = 60 * 50;  // 50秒

                int scaledCooldown = (int)MathHelper.Lerp(maxCooldown, minCooldown, shieldLifeRatio);

                rechargeCooldown = scaledCooldown;
                rechargeCooldownMax = scaledCooldown;

                Player.AddCooldown(ShieldConduitRecovery.ID, rechargeCooldown);

                shieldsTookHit = true;


                if (shieldLife <= 0)
                {
                    shieldLife = 0;
                    rechargeCooldown = 60 * 60;
                    rechargeCooldownMax = 60 * 60;
                    charge = 0;

                    Player.AddCooldown(ShieldConduitRecovery.ID, rechargeCooldown);
                    SoundEngine.PlaySound(RoverDrive.BreakSound, Player.Center);
                    Player.Calamity().GeneralScreenShakePower += anyShieldBroke ? 0.5f : 2f;
                    anyShieldBroke = true;
                }


                if (thisShieldCanFullyAbsorb)
                    shieldsFullyAbsorbedHit = true;

                shieldHitTimer = 10;
                SoundStyle shatterSound = SoundID.Shatter with
                {
                    Volume = 0.7f,
                    Pitch = 0.2f,
                    PitchVariance = 0.25f
                };

                SoundEngine.PlaySound(shatterSound, Player.Center);
                info.Damage -= shieldDamageBlocked;


                int numParticles = Main.rand.Next(6, 9) + (anyShieldBroke ? 9 : 0);

                float lifeRatio;

                if (anyShieldBroke)
                    lifeRatio = (float)shieldLifeBeforeHit / MaxShield; // ★ 割れるときだけ前の色
                else
                    lifeRatio = (float)shieldLife / MaxShield;          // ★ 通常は後の色

                for (int i = 0; i < numParticles; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(3f, 14f);
                    velocity.X += 5f * info.HitDirection;

                    float scale = Main.rand.NextFloat(0.5f, 4f);

                    float halfSize = 20f;
                    Vector2 spawnPosition = Player.Center;

                    if (Math.Abs(info.HitDirection) > 0)
                    {
                        spawnPosition.X += info.HitDirection > 0 ? -halfSize : halfSize;
                        spawnPosition.Y += Main.rand.NextFloat(-halfSize, halfSize);
                    }
                    else
                    {
                        // 上下っぽいとき → 中心から少し上下にずらす
                        float verticalOffset = Main.rand.NextFloat(10f, 20f); // ← 好きな距離に調整可
                        spawnPosition.Y += Main.rand.NextBool() ? verticalOffset : -verticalOffset;

                        // 横方向も少しランダムに散らす
                        spawnPosition.X += Main.rand.NextFloat(-10f, 10f);
                    }

                    Color particleColor;
                    if (lifeRatio > 0.5f)
                        particleColor = Color.Lerp(Color.Yellow, Color.Cyan, (lifeRatio - 0.5f) * 2f);
                    else
                        particleColor = Color.Lerp(Color.Red, Color.Yellow, lifeRatio * 2f);

                    int lifetime = 25;

                    var shieldParticle = new TechyHoloysquareParticle(spawnPosition, velocity, scale, particleColor, lifetime);
                    GeneralParticleHandler.SpawnParticle(shieldParticle);
                }
            }

            if (shieldsTookHit)
            {
                string shieldDamageText = (-totalDamageBlocked).ToString();
                Rectangle location = new Rectangle((int)Player.position.X, (int)Player.position.Y - 16, Player.width, Player.height);
                CombatText.NewText(location, Color.LightBlue, shieldDamageText);

                int shieldHitIFrames = Player.ComputeHitIFrames(info);
                Player.GiveIFrames(info.CooldownCounter, shieldHitIFrames, true);

                Player.AddCooldown(ShieldConduitDurability.ID, shieldLife);

                

            }

            if (shieldsFullyAbsorbedHit)
            {
                Player.Calamity().freeDodgeFromShieldAbsorption = true;

                Player.Calamity().nextHitDealsDefenseDamage = false;

            }

            if (cal.chaliceOfTheBloodGod && !shieldsFullyAbsorbedHit && info.Damage > 5)
            {
                cal.chaliceBleedoutToApplyOnHurt = info.Damage - 5;

                cal.chaliceHitOriginalDamage = info.Damage;
                info.Damage = 5;
            }

        }

        public override void UpdateDead()
        {
            shieldLife = 0;
            charge = 0;
            rechargeCooldown = 0;
            rechargeCooldownMax = 0;
            shieldTier = 0;
        }

        public void ResetShieldForBoss()
        {
            shieldLife = 0;
            charge = 0;

            rechargeCooldown = 0;
            rechargeCooldownMax = 0;

            cooldown = 0;
            shieldHitTimer = 0;
        }
    }
}

