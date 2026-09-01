using System;
using System.Collections.Generic;
using CalamityMod.DataStructures;
using CalamityMod.Systems.Collections;
using CalamityMod.NPCs;
using CalamityMod.NPCs.ExoMechs.Ares;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalamitySimpleWhipAddon;
using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Items.Weapons;
using CalamitySimpleWhipAddon.Content.Items.Accessories;
using CalamitySimpleWhipAddon.Content.Items.InvisibleSummonTagItem;
using CalamitySimpleWhipAddon.Content.Common.WhipGlobalProjectile;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamitySimpleWhipAddon.Content;
using CalamitySimpleWhipAddon.Content.Common;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Systems;
using CalamitySimpleWhipAddon.Content.Rendering;

namespace CalamitySimpleWhipAddon.Systems
{
    public class SimpleWhipSummonTagSystem : ModSystem
    {

        // 登録用データ構造
        private struct SummonTagEntry
        {
            public Func<int> ItemType;
            public Func<int> BuffType;
            public Action<SummonTag> Setup;
        }


        public override void PostSetupContent()
        {
            var entries = new SummonTagEntry[]
            {
                // BreezePiercer
                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<BreezePiercer>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff0D09C>(),
                    Setup = tag =>
                    {

                        tag.TagCritChance = 0.09f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleBreezePiercer",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<Droptide>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff12>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.12f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleDroptide",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<Ectopia>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff0D22C>(),
                    Setup = tag =>
                    {

                        tag.TagCritChance = 0.13f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleEctopia",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<EntwinedBranches>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff27>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.13f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleEntwinedBranches",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<Gelxyribose>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff7D07C>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.07f;
                        tag.TagCritChance = 0.07f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleGelxyribose",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<GlitterGutter>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff0D11C>(),
                    Setup = tag =>
                    {

                        tag.TagCritChance = 0.10f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleGlitterGutter",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<KusariGama>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff13>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.13f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleKusariGama",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<Necropsia>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff0D27C>(),
                    Setup = tag =>
                    {

                        tag.TagCritChance = 0.18f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleNecropsia",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<GreenPhaseScourge>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff09>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.09f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleGreenPhaseScourge",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<GreenPhaseWhip>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff08>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.08f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleGreenPhaseWhip",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<RapierWhip>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff0D08C>(),
                    Setup = tag =>
                    {

                        tag.TagCritChance = 0.06f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleRapierWhip",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<Unfathomable>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff25>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.25f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleUnfathomable",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<WoodenWhip>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff07>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.05f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleWoodenWhip",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<GloamshardConduit>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff10D10C>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.10f;
                        tag.TagCritChance = 0.10f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleGloamshardConduit",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<Loadout>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff7D07C2>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.07f;
                        tag.TagCritChance = 0.07f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleLoadout",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<ResonantVoid>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff15D15C>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.1f;
                        tag.TagCritChance = 0.1f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleResonantVoid",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<MandibleLash>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffT1>(),
                    Setup = tag =>
                    {

                        tag.FlatTagDamage = 1;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleMandibleLash",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<DarkMandibleLash>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffT2>(),
                    Setup = tag =>
                    {

                        tag.FlatTagDamage = 2;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleDarkMandibleLash",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                // Grip
                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<SilkGrip>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffAcC05>(),
                    Setup = tag =>
                    {

                        tag.TagCritChance = 0.05f;
                        tag.AllowsWhipStacking = true;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleSilkGrip",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<LeatherGrip>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffAc05>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.05f;
                        tag.AllowsWhipStacking = true;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleLeatherGrip",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<RubberGrip>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffAc05C05>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.03f;
                        tag.TagCritChance = 0.03f;
                        tag.AllowsWhipStacking = true;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleRubberGrip",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<NecromanticGrip>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffAc07C07>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.05f;
                        tag.TagCritChance = 0.05f;
                        tag.AllowsWhipStacking = true;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleNecromanticGrip",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<EmperorsGrip>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffAc10C10>(),
                    Setup = tag =>
                    {

                        tag.MultiplicativeTagDamage = 0.07f;
                        tag.TagCritChance = 0.07f;
                        tag.AllowsWhipStacking = true;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleEmperorsGrip",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<CommanderGrip>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffAcHo>(),
                    Setup = tag =>
                    {
                        tag.AllowsWhipStacking = true;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleCommanderGrip",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<MagneticGrip>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffAcHo2>(),
                    Setup = tag =>
                    {
                        tag.AllowsWhipStacking = true;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleMagneticGrip",
                                    AssetRequestMode.ImmediateLoad
                                    );
                    }
                },


                // ===== Satellite =====
                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<AncientBonds>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx5>(),
                    Setup = tag =>
                    {
                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleAncientBonds",
                                    AssetRequestMode.ImmediateLoad
                                    );

                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

                            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

                            if (isLargeWorm)
                return;

            float spawnRadiusMin = 160f;   // ★ 敵からの最小距離
            float spawnRadiusMax = 220f;  // ★ 敵からの最大距離
            float speed = 50f;


            int laserDamage = (int)(damageDone * (isLargeWorm ? 0.10f : 0.20f));

            // ランダム角度
            float angle = Main.rand.NextFloat(MathHelper.TwoPi);

            // ランダム距離（ここが「近さ」を決める）
            float radius = Main.rand.NextFloat(spawnRadiusMin, spawnRadiusMax);

            // 敵の周囲に生成
            Vector2 spawnPos = npc.Center + angle.ToRotationVector2() * radius;

            // 敵を必ず貫く方向
            Vector2 direction = npc.Center - spawnPos;
            direction.Normalize();

            int proj = Projectile.NewProjectile(
                Projectile.GetSource_None(),
                spawnPos,
                direction * speed,
                ModContent.ProjectileType<AncientBonds_Shot>(),
                laserDamage,
                0f,
                Main.myPlayer
            );

            // ★ ここで即回転を設定
            Main.projectile[proj].rotation =
                Main.projectile[proj].velocity.ToRotation();


            npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<SimpleWhipDebuffEx5>()));

            int buffType = ModContent.BuffType<SimpleWhipDebuffEx5>();

            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // サーバー → クライアントにフラグを通知
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
            packet.Write(npc.whoAmI);
            packet.Write(buffType);
            packet.Send(); // 全クライアントに送信
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<DestructionChain>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuff2x>(),
                    Setup = tag =>
                    {
                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleDestructionChain",
                                    AssetRequestMode.ImmediateLoad
                                    );

                        // ModifyHitByProjectile 相当
                        tag.TagModifyHitEffects = (Projectile projectile, NPC npc,
                            ref NPC.HitModifiers modifiers, ref float tagDamageMult, ref float critChance) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

            Player player = Main.player[projectile.owner];
            if (!player.active)
                return;


            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();

            float multiplier;

            if (modPlayer.buddyEmblem)
            {
                multiplier = isLargeWorm
                    ? 1.30f
                    : 1f;
            }
            else
            {
                multiplier = isLargeWorm
                    ? 1.40f
                    : 1f;
            }
            modifiers.SourceDamage *= multiplier;
                        };

                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;
                            Player player = Main.player[projectile.owner];
                            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();

            bool isReducedBoss =
                npc.type == ModContent.NPCType<CalamityMod.NPCs.Ravager.RavagerClawLeft>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.Ravager.RavagerClawRight>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.Ravager.RavagerLegLeft>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.Ravager.RavagerLegRight>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.Ravager.RavagerHead>();

            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

                            float explosionRatio;

                            if (modPlayer.buddyEmblem)
            {
                explosionRatio = isLargeWorm
                    ? 0f
                    : isReducedBoss
                        ? 0.5f
                        : 0.85f;
            }
            else
            {
                explosionRatio = isLargeWorm
                    ? 0f
                    : isReducedBoss
                        ? 0.65f
                        : 1f;
            }
            int explosionDamage = (int)(damageDone * explosionRatio);

            Projectile.NewProjectile(
                Projectile.GetSource_None(),
                npc.Center,
                Vector2.Zero,
                ModContent.ProjectileType<DestructionChainTag>(),
                explosionDamage,
                0f,
                Main.myPlayer
            );

            SoundEngine.PlaySound(SoundID.Item14, npc.Center);

            // タグを消す
            npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<SimpleWhipDebuff2x>()));

            int buffType = ModContent.BuffType<SimpleWhipDebuff2x>();


            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // サーバー → クライアントにフラグを通知
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
            packet.Write(npc.whoAmI);
            packet.Write(buffType);
            packet.Send(); // 全クライアントに送信
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<ExistenceBonds>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx7>(),
                    Setup = tag =>
                    {
                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleExistenceBonds",
                                    AssetRequestMode.ImmediateLoad
                                    );
                        

                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

                            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

                            if (isLargeWorm)
                return;

            float spawnRadiusMin = 200f;   // ★ 敵からの最小距離
            float spawnRadiusMax = 280f;  // ★ 敵からの最大距離
            float speed = 70f;


            int laserDamage = (int)(damageDone * (isLargeWorm ? 0.10f : 0.20f));

            // ランダム角度
            float angle = Main.rand.NextFloat(MathHelper.TwoPi);

            // ランダム距離（ここが「近さ」を決める）
            float radius = Main.rand.NextFloat(spawnRadiusMin, spawnRadiusMax);

            // 敵の周囲に生成
            Vector2 spawnPos = npc.Center + angle.ToRotationVector2() * radius;

            // 敵を必ず貫く方向
            Vector2 direction = npc.Center - spawnPos;
            direction.Normalize();

            int proj = Projectile.NewProjectile(
                Projectile.GetSource_None(),
                spawnPos,
                direction * speed,
                ModContent.ProjectileType<ExistenceBonds_Shot>(),
                laserDamage,
                0f,
                Main.myPlayer
            );

            // ★ ここで即回転を設定
            Main.projectile[proj].rotation =
                Main.projectile[proj].velocity.ToRotation();


            npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<SimpleWhipDebuffEx7>()));

            int buffType = ModContent.BuffType<SimpleWhipDebuffEx7>();

            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // サーバー → クライアントにフラグを通知
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
            packet.Write(npc.whoAmI);
            packet.Write(buffType);
            packet.Send(); // 全クライアントに送信
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<InvisibleGloamshardConduit2>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx1>(),
                    Setup = tag =>
                    {


                        tag.AllowsWhipStacking = true;

                        // ModifyHitByProjectile 相当
                        tag.TagModifyHitEffects = (Projectile projectile, NPC npc,
                            ref NPC.HitModifiers modifiers, ref float tagDamageMult, ref float critChance) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

            Player player = Main.player[projectile.owner];
            if (!player.active)
                return;

            bool isReducedBoss =
                npc.type == ModContent.NPCType<CalamityMod.NPCs.AstrumDeus.AstrumDeusBody>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosBody1>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosBody2>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.StormWeaver.StormWeaverBody>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsBody>();

            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();

            float multiplier;

            if (modPlayer.buddyEmblem)
            {
                multiplier = (isReducedBoss || isLargeWorm)
                    ? 1.07f
                    : 1.15f;
            }
            else
            {
                multiplier = (isReducedBoss || isLargeWorm)
                    ? 1.13f
                    : 1.25f;
            }
            modifiers.SourceDamage *= multiplier;
                        };

                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

            bool isReducedBoss =
                npc.type == ModContent.NPCType<CalamityMod.NPCs.AstrumDeus.AstrumDeusBody>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosBody1>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosBody2>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.StormWeaver.StormWeaverBody>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsBody>();

            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

            float explosionRatio =
                (isReducedBoss || isLargeWorm) ? 0f : 0.10f;
            int explosionDamage = (int)(damageDone * explosionRatio);

            Projectile.NewProjectile(
                Projectile.GetSource_None(),
                npc.Center,
                Vector2.Zero,
                ModContent.ProjectileType<GloamshardConduitTag>(),
                explosionDamage,
                0f,
                Main.myPlayer
            );

            SoundEngine.PlaySound(SoundID.Item14, npc.Center);

            // タグを消す
            npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<SimpleWhipDebuffEx1>()));

            int buffType = ModContent.BuffType<SimpleWhipDebuffEx1>();

            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // サーバー → クライアントにフラグを通知
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
            packet.Write(npc.whoAmI);
            packet.Write(buffType);
            packet.Send(); // 全クライアントに送信
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<InvisibleLoadout2>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx4>(),
                    Setup = tag =>
                    {

                        tag.AllowsWhipStacking = true;

                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;
                            Player player = Main.player[projectile.owner];
                            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();

            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

                            float laserRatio = modPlayer.buddyEmblem
                ? (isLargeWorm ? 0.2f : 0.5f)
                : (isLargeWorm ? 0.4f : 0.5f);

            int laserDamage = (int)(damageDone * laserRatio);
            float speed = 30f;

            // 画面矩形
            Rectangle screenRect = new Rectangle(
                (int)Main.screenPosition.X,
                (int)Main.screenPosition.Y,
                Main.screenWidth,
                Main.screenHeight
            );

            // 余白を足した矩形（完全に画面外に出すため）
            Rectangle outerRect = screenRect;
            outerRect.Inflate(400, 400);

            Vector2 spawnPos;
            do
            {
                spawnPos = new Vector2(
                    Main.rand.Next(outerRect.Left, outerRect.Right),
                    Main.rand.Next(outerRect.Top, outerRect.Bottom)
                );
            } while (screenRect.Contains(spawnPos.ToPoint()));

            // 敵に向かう方向
            Vector2 direction = npc.Center - spawnPos;
            direction.Normalize();

            Projectile.NewProjectile(
                Projectile.GetSource_None(),
                spawnPos,
                direction * speed,
                ModContent.ProjectileType<Loadout_Laser>(),
                laserDamage,
                0f,
                Main.myPlayer
            );

            npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<SimpleWhipDebuffEx4>()));

            int buffType = ModContent.BuffType<SimpleWhipDebuffEx4>();

            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // サーバー → クライアントにフラグを通知
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
            packet.Write(npc.whoAmI);
            packet.Write(buffType);
            packet.Send(); // 全クライアントに送信
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<MermaidsTear>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx8>(),
                    Setup = tag =>
                    {
                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleMermaidsTear",
                                    AssetRequestMode.ImmediateLoad
                                    );
                        

                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

                            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

                            if (isLargeWorm)
                return;

            float spawnRadiusMin = 420f;   // ★ 敵からの最小距離
            float spawnRadiusMax = 530f;  // ★ 敵からの最大距離
            float speed = 85f;

            int laserDamage = (int)(damageDone * (isLargeWorm ? 0.15f : 0.25f));

            // ランダム角度
            float angle = Main.rand.NextFloat(MathHelper.TwoPi);

            // ランダム距離（ここが「近さ」を決める）
            float radius = Main.rand.NextFloat(spawnRadiusMin, spawnRadiusMax);

            // 敵の周囲に生成
            Vector2 spawnPos = npc.Center + angle.ToRotationVector2() * radius;

            // 敵を必ず貫く方向
            Vector2 direction = npc.Center - spawnPos;
            direction.Normalize();

            int proj = Projectile.NewProjectile(
                Projectile.GetSource_None(),
                spawnPos,
                direction * speed,
                ModContent.ProjectileType<MermaidsTear_Shot>(),
                laserDamage,
                0f,
                Main.myPlayer
            );

            // ★ ここで即回転を設定
            Main.projectile[proj].rotation =
                Main.projectile[proj].velocity.ToRotation();


            npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<SimpleWhipDebuffEx8>()));

            int buffType = ModContent.BuffType<SimpleWhipDebuffEx8>();

            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // サーバー → クライアントにフラグを通知
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
            packet.Write(npc.whoAmI);
            packet.Write(buffType);
            packet.Send(); // 全クライアントに送信
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<InvisibleMilkyway2>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffC100x3>(),
                    Setup = tag =>
                    {
                        tag.AllowsWhipStacking = true;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleMilkyway2",
                                    AssetRequestMode.ImmediateLoad
                                    );
                        
                        // ModifyHitByProjectile 相当
                        tag.TagModifyHitEffects = (Projectile projectile, NPC npc,
                            ref NPC.HitModifiers modifiers, ref float tagDamageMult, ref float critChance) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

            Player player = Main.player[projectile.owner];
            if (!player.active)
                return;


            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

            float multiplier = modPlayer.buddyEmblem
                ? (isLargeWorm ? 1.0f : 1.25f)
                : (isLargeWorm ? 1.2f : 1.5f);

            modifiers.CritDamage *= multiplier;
                            critChance = 1f;
                        };

                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

            int explosionDamage = (int)(damageDone * 0f);

            Projectile.NewProjectile(
                Projectile.GetSource_None(),
                npc.Center,
                Vector2.Zero,
                ModContent.ProjectileType<MilkywayTag>(),
                explosionDamage,
                0f,
                Main.myPlayer
            );

            SoundEngine.PlaySound(SoundID.DD2_WitherBeastDeath with { Volume = 0.40f }, npc.Center);

            npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<SimpleWhipDebuffC100x3>()));

            int buffType = ModContent.BuffType<SimpleWhipDebuffC100x3>();

            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // サーバー → クライアントにフラグを通知
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
            packet.Write(npc.whoAmI);
            packet.Write(buffType);
            packet.Send(); // 全クライアントに送信
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<PrimalBonds>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx6>(),
                    Setup = tag =>
                    {
                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisiblePrimalBonds",
                                    AssetRequestMode.ImmediateLoad
                                    );
                        
                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

                            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

                            if (isLargeWorm)
                return;

            float spawnRadiusMin = 180f;   // ★ 敵からの最小距離
            float spawnRadiusMax = 240f;  // ★ 敵からの最大距離
            float speed = 60f;


            int laserDamage = (int)(damageDone * (isLargeWorm ? 0.10f : 0.20f));

            // ランダム角度
            float angle = Main.rand.NextFloat(MathHelper.TwoPi);

            // ランダム距離（ここが「近さ」を決める）
            float radius = Main.rand.NextFloat(spawnRadiusMin, spawnRadiusMax);

            // 敵の周囲に生成
            Vector2 spawnPos = npc.Center + angle.ToRotationVector2() * radius;

            // 敵を必ず貫く方向
            Vector2 direction = npc.Center - spawnPos;
            direction.Normalize();

            int proj = Projectile.NewProjectile(
                Projectile.GetSource_None(),
                spawnPos,
                direction * speed,
                ModContent.ProjectileType<PrimalBonds_Shot>(),
                laserDamage,
                0f,
                Main.myPlayer
            );

            // ★ ここで即回転を設定
            Main.projectile[proj].rotation =
                Main.projectile[proj].velocity.ToRotation();


            npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<SimpleWhipDebuffEx6>()));

            int buffType = ModContent.BuffType<SimpleWhipDebuffEx6>();

            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // サーバー → クライアントにフラグを通知
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
            packet.Write(npc.whoAmI);
            packet.Write(buffType);
            packet.Send(); // 全クライアントに送信
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<InvisibleResonantVoid2>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx2>(),
                    Setup = tag =>
                    {


                        tag.AllowsWhipStacking = true;

                        // ModifyHitByProjectile 相当
                        tag.TagModifyHitEffects = (Projectile projectile, NPC npc,
                            ref NPC.HitModifiers modifiers, ref float tagDamageMult, ref float critChance) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

            Player player = Main.player[projectile.owner];
            if (!player.active)
                return;

            bool isReducedBoss =
                npc.type == ModContent.NPCType<CalamityMod.NPCs.AstrumDeus.AstrumDeusBody>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosBody1>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosBody2>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.StormWeaver.StormWeaverBody>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsBody>();

            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();

            float multiplier;

            if (modPlayer.buddyEmblem)
            {
                multiplier = (isReducedBoss || isLargeWorm)
                    ? 1.4f
                    : 1.2f;
            }
            else
            {
                multiplier = (isReducedBoss || isLargeWorm)
                    ? 1.50f
                    : 1.25f;
            }
            modifiers.SourceDamage *= multiplier;
                        };

                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

            bool isReducedBoss =
                npc.type == ModContent.NPCType<CalamityMod.NPCs.AstrumDeus.AstrumDeusBody>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosBody1>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosBody2>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.StormWeaver.StormWeaverBody>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsBody>();

            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

            float explosionRatio =
                (isReducedBoss || isLargeWorm) ? 0f : 0.20f;

            int explosionDamage = (int)(damageDone * explosionRatio);

            Projectile.NewProjectile(
                Projectile.GetSource_None(),
                npc.Center,
                Vector2.Zero,
                ModContent.ProjectileType<ResonantVoidTag>(),
                explosionDamage,
                0f,
                Main.myPlayer
            );

            SoundEngine.PlaySound(SoundID.Item14, npc.Center);

            npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<SimpleWhipDebuffEx2>()));

            int buffType = ModContent.BuffType<SimpleWhipDebuffEx2>();

            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // サーバー → クライアントにフラグを通知
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
            packet.Write(npc.whoAmI);
            packet.Write(buffType);
            packet.Send(); // 全クライアントに送信
                        };
                    }
                },


                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<SkybreakerCoil>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx3>(),
                    Setup = tag =>
                    {

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleSkybreakerCoil",
                                    AssetRequestMode.ImmediateLoad
                                    );
                        
                        // ModifyHitByProjectile 相当
                        tag.TagModifyHitEffects = (Projectile projectile, NPC npc,
                            ref NPC.HitModifiers modifiers, ref float tagDamageMult, ref float critChance) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

            Player player = Main.player[projectile.owner];
            if (!player.active)
                return;

            bool isReducedBoss =
                npc.type == ModContent.NPCType<CalamityMod.NPCs.AstrumDeus.AstrumDeusBody>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosBody1>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosBody2>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.StormWeaver.StormWeaverBody>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsBody>();

            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();

            float multiplier;

            if (modPlayer.buddyEmblem)
            {
                multiplier = (isReducedBoss || isLargeWorm) ? 1.20f : 1.15f;
                modifiers.SourceDamage *= multiplier;
                modifiers.DefenseEffectiveness *= 0f;
            }
            else
            {
                multiplier = (isReducedBoss || isLargeWorm) ? 1.20f : 1.30f;
                modifiers.SourceDamage *= multiplier;
                modifiers.DefenseEffectiveness *= 0f;
            }
                        };

                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

            Player player = Main.player[projectile.owner];
            if (!player.active)
                return;

            bool isReducedBoss =
                npc.type == ModContent.NPCType<CalamityMod.NPCs.AstrumDeus.AstrumDeusBody>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosBody1>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.ExoMechs.Thanatos.ThanatosBody2>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.StormWeaver.StormWeaverBody>() ||
                npc.type == ModContent.NPCType<CalamityMod.NPCs.DevourerofGods.DevourerofGodsBody>();

            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();

            float explosionRatio;
            int explosionDamage;

            if (modPlayer.buddyEmblem)
            {
                explosionRatio = (isReducedBoss || isLargeWorm) ? 0f : 0.50f;
            }
            else
            {
                explosionRatio = (isReducedBoss || isLargeWorm) ? 0f : 0.60f;
            }

            explosionDamage = (int)(damageDone * explosionRatio);

            Projectile.NewProjectile(
                Projectile.GetSource_None(),
                npc.Center,
                Vector2.Zero,
                ModContent.ProjectileType<SkybreakerCoilTag>(),
                explosionDamage,
                0f,
                Main.myPlayer
            );

            SoundEngine.PlaySound(AresTeslaCannon.TeslaOrbShootSound with { Volume = 0.7f }, npc.Center);

            npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<SimpleWhipDebuffEx3>()));

            int buffType = ModContent.BuffType<SimpleWhipDebuffEx3>();

            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // サーバー → クライアントにフラグを通知
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
            packet.Write(npc.whoAmI);
            packet.Write(buffType);
            packet.Send(); // 全クライアントに送信
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<InvisibleGlitterGutter2>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx9>(),
                    Setup = tag =>
                    {
                        tag.AllowsWhipStacking = true;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleGlitterGutter2",
                                    AssetRequestMode.ImmediateLoad
                                    );
                        
                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

                bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);
                    int laserDamage = (int)(damageDone * (isLargeWorm ? 0.10f : 0.20f));

                    int shotCount = 2;
                    float speed = 8f;

                    for (int i = 0; i < shotCount; i++)
                    {
                        float angle = MathHelper.TwoPi * i / shotCount + Main.rand.NextFloat(-0.4f, 0.4f);
                        Vector2 velocity = angle.ToRotationVector2() * speed;

                        int proj = Projectile.NewProjectile(
                            Projectile.GetSource_None(),
                            npc.Center,
                            velocity,
                            ModContent.ProjectileType<GlitterGutter_Shot>(),
                            laserDamage,
                            0f,
                            Main.myPlayer,
                            npc.whoAmI // ai[0] にターゲット保存
                        );

                        Main.projectile[proj].rotation =
                            Main.projectile[proj].velocity.ToRotation();
                    }

                    npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<SimpleWhipDebuffEx9>()));

            int buffType = ModContent.BuffType<SimpleWhipDebuffEx9>();

            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // サーバー → クライアントにフラグを通知
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
            packet.Write(npc.whoAmI);
            packet.Write(buffType);
            packet.Send(); // 全クライアントに送信
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<Satellite>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffC100>(),
                    Setup = tag =>
                    {
                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleSatellite",
                                    AssetRequestMode.ImmediateLoad
                                    );
                        
                        // ModifyHitByProjectile 相当
                        tag.TagModifyHitEffects = (Projectile projectile, NPC npc,
                            ref NPC.HitModifiers modifiers, ref float tagDamageMult, ref float critChance) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            Player player = Main.player[projectile.owner];
                            if (player == null || !player.active)
                                return;

                            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
                            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

                            float multiplier = modPlayer.buddyEmblem
                                ? (isLargeWorm ? 1.05f : 1.1f)
                                : (isLargeWorm ? 1.15f : 1.25f);

                            modifiers.CritDamage *= multiplier;

                            // 100% クリティカル
                            critChance = 1f;
                        };

                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            int explosionDamage = (int)(damageDone * 0f);

                            Projectile.NewProjectile(
                                Projectile.GetSource_None(),
                                npc.Center,
                                Vector2.Zero,
                                ModContent.ProjectileType<SatelliteTag>(),
                                explosionDamage,
                                0f,
                                Main.myPlayer
                            );

                            SoundEngine.PlaySound(
                                SoundID.DD2_WitherBeastDeath with { Volume = 0.50f },
                                npc.Center
                            );

                            // デバフを即時削除（1回限り）
                            int buffType = ModContent.BuffType<SimpleWhipDebuffC100>();
                            npc.DelBuff(npc.FindBuffIndex(buffType));

                            if (Main.netMode == NetmodeID.SinglePlayer)
                                return;
                            // サーバー → クライアントにフラグを通知
                            ModPacket packet = Mod.GetPacket();
                            packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
                            packet.Write(npc.whoAmI);
                            packet.Write(buffType);
                            packet.Send(); // 全クライアントに送信
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<ShieldConduit>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffS1>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.FlatTagDamage = 2;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleShieldConduit",
                                    AssetRequestMode.ImmediateLoad
                                    );
                        
                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            int tier = 1;

                            Player player = Main.player[projectile.owner];
                            var modPlayer = player.GetModPlayer<WhipShieldPlayer>();
                            var buddyPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
                            if (projectile.owner != Main.myPlayer) return;

                            // ===== クールダウン判定 =====
                            if (modPlayer.orbSpawnCooldown > 0)
                                return;

                            bool attackOrb =
                            modPlayer.rechargeCooldown > 0 ||
                            modPlayer.shieldLife >= modPlayer.MaxShield;

                            // ===== 通常オーブはシールドMAXなら出さない =====
                            if (!attackOrb && modPlayer.shieldLife >= modPlayer.MaxShield)
                                return;

                            if (Main.myPlayer == player.whoAmI)
                            {
                                bool soloActive = buddyPlayer.SoloBonusActive;

                                int damage = attackOrb ? (int)(damageDone * 0.1f + 3) : 0;

                                int orbCount = 1;
                                if (soloActive)
                                    orbCount = Main.rand.NextBool(2) ? 2 : 1;


                                for (int i = 0; i < orbCount; i++)
                                {
                                   Vector2 spawnPos = npc.Center;

                                   // 少しばらけさせたい場合（任意）
                                   Vector2 velocity = Main.rand.NextVector2Circular(1f, 1f);

                                   Projectile.NewProjectile(
                                       player.GetSource_OnHit(npc),
                                       spawnPos,
                                       velocity,
                                       ModContent.ProjectileType<WhipShieldChargeOrbProj>(),
                                       damage,
                                       0f,
                                       player.whoAmI,
                                       attackOrb ? 1f : 0f,
                                       tier
                                   );
                                }

                                // ===== クールダウン開始 =====
                                modPlayer.orbSpawnCooldown = 6;
                            }
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<ShieldConduitMkII>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffS2>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.FlatTagDamage = 4;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleShieldConduitMkII",
                                    AssetRequestMode.ImmediateLoad
                                    );
                        
                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            int tier = 2;

                            Player player = Main.player[projectile.owner];
                            var modPlayer = player.GetModPlayer<WhipShieldPlayer>();
                            var buddyPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
                            if (projectile.owner != Main.myPlayer) return;

                            // ===== クールダウン判定 =====
                            if (modPlayer.orbSpawnCooldown > 0)
                                return;

                            bool attackOrb =
                            modPlayer.rechargeCooldown > 0 ||
                            modPlayer.shieldLife >= modPlayer.MaxShield;

                            // ===== 通常オーブはシールドMAXなら出さない =====
                            if (!attackOrb && modPlayer.shieldLife >= modPlayer.MaxShield)
                                return;

                            if (Main.myPlayer == player.whoAmI)
                            {
                                int damage = attackOrb ? (int)(damageDone * 0.1f + 5) : 0;

                                bool soloActive = buddyPlayer.SoloBonusActive;

                                int orbCount = 1;
                                if (soloActive)
                                    orbCount = Main.rand.NextBool(2) ? 2 : 1;


                                for (int i = 0; i < orbCount; i++)
                                {
                                   Vector2 spawnPos = npc.Center;

                                   // 少しばらけさせたい場合（任意）
                                   Vector2 velocity = Main.rand.NextVector2Circular(1f, 1f);

                                   Projectile.NewProjectile(
                                       player.GetSource_OnHit(npc),
                                       spawnPos,
                                       velocity,
                                       ModContent.ProjectileType<WhipShieldChargeOrbProj>(),
                                       damage,
                                       0f,
                                       player.whoAmI,
                                       attackOrb ? 1f : 0f,
                                       tier
                                   );
                                }

                                // ===== クールダウン開始 =====
                                modPlayer.orbSpawnCooldown = 6;
                            }
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<ShieldConduitMkIII>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffS3>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.FlatTagDamage = 7;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleShieldConduitMkIII",
                                    AssetRequestMode.ImmediateLoad
                                    );
                        
                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            int tier = 3;

                            Player player = Main.player[projectile.owner];
                            var modPlayer = player.GetModPlayer<WhipShieldPlayer>();
                            var buddyPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
                            if (projectile.owner != Main.myPlayer) return;

                            // ===== クールダウン判定 =====
                            if (modPlayer.orbSpawnCooldown > 0)
                                return;

                            bool attackOrb =
                            modPlayer.rechargeCooldown > 0 ||
                            modPlayer.shieldLife >= modPlayer.MaxShield;

                            // ===== 通常オーブはシールドMAXなら出さない =====
                            if (!attackOrb && modPlayer.shieldLife >= modPlayer.MaxShield)
                                return;

                            if (Main.myPlayer == player.whoAmI)
                            {
                                int damage = attackOrb ? (int)(damageDone * 0.1f + 25) : 0;

                                bool soloActive = buddyPlayer.SoloBonusActive;

                                int orbCount = 1;
                                if (soloActive)
                                    orbCount = Main.rand.NextBool(2) ? 2 : 1;

                                for (int i = 0; i < orbCount; i++)
                                {
                                   Vector2 spawnPos = npc.Center;

                                   // 少しばらけさせたい場合（任意）
                                   Vector2 velocity = Main.rand.NextVector2Circular(1f, 1f);

                                   Projectile.NewProjectile(
                                       player.GetSource_OnHit(npc),
                                       spawnPos,
                                       velocity,
                                       ModContent.ProjectileType<WhipShieldChargeOrbProj>(),
                                       damage,
                                       0f,
                                       player.whoAmI,
                                       attackOrb ? 1f : 0f,
                                       tier
                                   );
                                }

                                // ===== クールダウン開始 =====
                                modPlayer.orbSpawnCooldown = 6;
                            }
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<ShieldConduitMkIV>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffS4>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.FlatTagDamage = 12;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleShieldConduitMkIV",
                                    AssetRequestMode.ImmediateLoad
                                    );
                        
                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            int tier = 4;

                            Player player = Main.player[projectile.owner];
                            var modPlayer = player.GetModPlayer<WhipShieldPlayer>();
                            var buddyPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
                            if (projectile.owner != Main.myPlayer) return;

                            // ===== クールダウン判定 =====
                            if (modPlayer.orbSpawnCooldown > 0)
                                return;

                            bool attackOrb =
                            modPlayer.rechargeCooldown > 0 ||
                            modPlayer.shieldLife >= modPlayer.MaxShield;

                            // ===== 通常オーブはシールドMAXなら出さない =====
                            if (!attackOrb && modPlayer.shieldLife >= modPlayer.MaxShield)
                                return;

                            if (Main.myPlayer == player.whoAmI)
                            {
                                int damage = attackOrb ? (int)(damageDone * 0.1f + 50) : 0;

                                bool soloActive = buddyPlayer.SoloBonusActive;

                                int orbCount = 1;
                                if (soloActive)
                                    orbCount = Main.rand.NextBool(2) ? 2 : 1;

                                for (int i = 0; i < orbCount; i++)
                                {
                                   Vector2 spawnPos = npc.Center;

                                   // 少しばらけさせたい場合（任意）
                                   Vector2 velocity = Main.rand.NextVector2Circular(1f, 1f);

                                   Projectile.NewProjectile(
                                       player.GetSource_OnHit(npc),
                                       spawnPos,
                                       velocity,
                                       ModContent.ProjectileType<WhipShieldChargeOrbProj>(),
                                       damage,
                                       0f,
                                       player.whoAmI,
                                       attackOrb ? 1f : 0f,
                                       tier
                                   );
                                }

                                // ===== クールダウン開始 =====
                                modPlayer.orbSpawnCooldown = 6;
                            }
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<ShieldConduitMkV>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffS5>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.FlatTagDamage = 20;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleShieldConduitMkV",
                                    AssetRequestMode.ImmediateLoad
                                    );
                        
                        // OnHitByProjectile 相当
                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            int tier = 5;

                            Player player = Main.player[projectile.owner];
                            var modPlayer = player.GetModPlayer<WhipShieldPlayer>();
                            var buddyPlayer = player.GetModPlayer<BuddyEmblemPlayer>();

                            if (projectile.owner != Main.myPlayer) return;

                            // ===== クールダウン判定 =====
                            if (modPlayer.orbSpawnCooldown > 0)
                                return;

                            bool attackOrb =
                            modPlayer.rechargeCooldown > 0 ||
                            modPlayer.shieldLife >= modPlayer.MaxShield;

                            // ===== 通常オーブはシールドMAXなら出さない =====
                            if (!attackOrb && modPlayer.shieldLife >= modPlayer.MaxShield)
                                return;

                            if (Main.myPlayer == player.whoAmI)
                            {
                                int damage = attackOrb ? (int)(damageDone * 0.1f + 150) : 0;

                                bool soloActive = buddyPlayer.SoloBonusActive;

                                int orbCount = 1;
                                if (soloActive)
                                    orbCount = Main.rand.NextBool(2) ? 2 : 1;

                                for (int i = 0; i < orbCount; i++)
                                {
                                   Vector2 spawnPos = npc.Center;

                                   // 少しばらけさせたい場合（任意）
                                   Vector2 velocity = Main.rand.NextVector2Circular(1f, 1f);

                                   Projectile.NewProjectile(
                                       player.GetSource_OnHit(npc),
                                       spawnPos,
                                       velocity,
                                       ModContent.ProjectileType<WhipShieldChargeOrbProj>(),
                                       damage,
                                       0f,
                                       player.whoAmI,
                                       attackOrb ? 1f : 0f,
                                       tier
                                   );
                                }

                                // ===== クールダウン開始 =====
                                modPlayer.orbSpawnCooldown = 6;
                            }
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<LayeredPain>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffT1St>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleLayeredPain",
                                    AssetRequestMode.ImmediateLoad
                                    );

                        // ModifyHitByProjectile 相当
                        tag.TagModifyHitEffects = (Projectile projectile, NPC npc,
                            ref NPC.HitModifiers modifiers, ref float tagDamageMult, ref float critChance) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                    return;
                            var projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
                                StackTagNPC tagNPC = npc.GetGlobalNPC<StackTagNPC>();

                                int stacks = tagNPC.StackCount;

                                modifiers.FlatBonusDamage += stacks * 1 * projTagMultiplier;
                        };

                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<NightButterfly>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffT2St>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleNightButterfly",
                                    AssetRequestMode.ImmediateLoad
                                    );

                        // ModifyHitByProjectile 相当
                        tag.TagModifyHitEffects = (Projectile projectile, NPC npc,
                            ref NPC.HitModifiers modifiers, ref float tagDamageMult, ref float critChance) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                    return;
                            var projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];

                                StackTagNPC2 tagNPC = npc.GetGlobalNPC<StackTagNPC2>();

                                int stacks = tagNPC.StackCount;

                                modifiers.FlatBonusDamage += stacks * 2 * projTagMultiplier;
                        };

                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<ActiasAliena>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffT3St>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleActiasAliena",
                                    AssetRequestMode.ImmediateLoad
                                    );

                        // ModifyHitByProjectile 相当
                        tag.TagModifyHitEffects = (Projectile projectile, NPC npc,
                            ref NPC.HitModifiers modifiers, ref float tagDamageMult, ref float critChance) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                    return;
                            var projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
                                StackTagNPC2 tagNPC = npc.GetGlobalNPC<StackTagNPC2>();

                                int stacks = tagNPC.StackCount;

                                modifiers.FlatBonusDamage += stacks * 3 * projTagMultiplier;
                        };

                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<ButterflyEffect>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffT5St>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleButterflyEffect",
                                    AssetRequestMode.ImmediateLoad
                                    );

                        // ModifyHitByProjectile 相当
                        tag.TagModifyHitEffects = (Projectile projectile, NPC npc,
                            ref NPC.HitModifiers modifiers, ref float tagDamageMult, ref float critChance) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                    return;
                            var projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
                                StackTagNPC2 tagNPC = npc.GetGlobalNPC<StackTagNPC2>();

                                int stacks = tagNPC.StackCount;

                                modifiers.FlatBonusDamage += stacks * 5 * projTagMultiplier;
                        };

                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<EtaCarinae>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffT10St>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleEtaCarinae",
                                    AssetRequestMode.ImmediateLoad
                                    );

                        // ModifyHitByProjectile 相当
                        tag.TagModifyHitEffects = (Projectile projectile, NPC npc,
                            ref NPC.HitModifiers modifiers, ref float tagDamageMult, ref float critChance) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                    return;
                            var projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
                                StackTagNPC2 tagNPC = npc.GetGlobalNPC<StackTagNPC2>();

                                int stacks = tagNPC.StackCount;

                                modifiers.FlatBonusDamage += stacks * 10 * projTagMultiplier;
                        };

                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<Milkyway>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffT20St>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleMilkyway",
                                    AssetRequestMode.ImmediateLoad
                                    );

                        // ModifyHitByProjectile 相当
                        tag.TagModifyHitEffects = (Projectile projectile, NPC npc,
                            ref NPC.HitModifiers modifiers, ref float tagDamageMult, ref float critChance) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                    return;
                            var projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
                                StackTagNPC2 tagNPC = npc.GetGlobalNPC<StackTagNPC2>();

                                int stacks = tagNPC.StackCount;

                                modifiers.FlatBonusDamage += stacks * 10 * projTagMultiplier;
                        };

                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<GoldRush>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx12>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.FlatTagDamage = 8;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleGoldRush",
                                    AssetRequestMode.ImmediateLoad
                                    );


                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            float dr = npc.GetGlobalNPC<CalamityGlobalNPC>().DR;

                            if (dr >= 0.9f)
                                return;

                            Player player = Main.player[projectile.owner];

                            if (projectile.owner != Main.myPlayer) return;

                            if (player.HeldItem.type != ModContent.ItemType<GoldRush>())
                                return;


                            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

                            NPC target = npc;
                            NPC cooldownTarget = npc;

                            if (isLargeWorm && npc.realLife >= 0)
                            {
                                // クールダウンは頭で共有
                                cooldownTarget = Main.npc[npc.realLife];
                            
                                // DRを突破できる部位を記録
                                GateTargetManager.LastWormTarget = npc.whoAmI;

                                // 前回記録した部位をターゲットにする
                                if (GateTargetManager.LastWormTarget >= 0 &&
                                    GateTargetManager.LastWormTarget < Main.maxNPCs &&
                                    Main.npc[GateTargetManager.LastWormTarget].active)
                                {
                                    target = Main.npc[GateTargetManager.LastWormTarget];
                                }
                            }


                            var modNPC = cooldownTarget.GetGlobalNPC<GateTagNPC>();

                            if (modNPC.gateCooldown > 0)
                                return;

                            modNPC.gateCooldown = 15;


                            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();

                            int shotDamage = (int)(damageDone * (modPlayer.buddyEmblem ? 0.25f : 0.4f));

                            if (GoldRushManager.CanSpawnGate())
                            {
                                // 位置は player.Center でとりあえず生成（ゲートAIが即座に背後へ配置するため）
                                Projectile.NewProjectile(
                                   Projectile.GetSource_None(),
                                   player.Center,
                                   Vector2.Zero,
                                   ModContent.ProjectileType<GoldRush_Gate>(),
                                   shotDamage,
                                   0f,
                                   player.whoAmI,
                                   target.whoAmI // ターゲット情報は一応渡しておく
                                );

                                GoldRushManager.OnSpawnGate(10);
                            }
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<GildedReliquary>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx13>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.FlatTagDamage = 20;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleGildedReliquary",
                                    AssetRequestMode.ImmediateLoad
                                    );


                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            float dr = npc.GetGlobalNPC<CalamityGlobalNPC>().DR;

                            if (dr >= 0.9f)
                                return;

                            Player player = Main.player[projectile.owner];

                            if (projectile.owner != Main.myPlayer) return;

                            if (player.HeldItem.type != ModContent.ItemType<GildedReliquary>())
                                return;


                            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

                            NPC target = npc;
                            NPC cooldownTarget = npc;

                            if (isLargeWorm && npc.realLife >= 0)
                            {
                                // クールダウンは頭で共有
                                cooldownTarget = Main.npc[npc.realLife];
                            
                                // DRを突破できる部位を記録
                                GateTargetManager.LastWormTarget = npc.whoAmI;

                                // 前回記録した部位をターゲットにする
                                if (GateTargetManager.LastWormTarget >= 0 &&
                                    GateTargetManager.LastWormTarget < Main.maxNPCs &&
                                    Main.npc[GateTargetManager.LastWormTarget].active)
                                {
                                    target = Main.npc[GateTargetManager.LastWormTarget];
                                }
                            }


                            var modNPC = cooldownTarget.GetGlobalNPC<GateTagNPC>();

                            if (modNPC.gateCooldown > 0)
                                return;

                            modNPC.gateCooldown = 6; // 0.1秒


                            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
                            var modPlayer2 = player.GetModPlayer<EchoWhipPlayer>();

                            int shotDamage = (int)(damageDone * (modPlayer.buddyEmblem ? 1.0f : (modPlayer2.echoAccessory ? 1.0f : 0.8f)));

                            if (GoldRushManager.CanSpawnGate())
                            {
                                // 位置は player.Center でとりあえず生成（ゲートAIが即座に背後へ配置するため）
                                Projectile.NewProjectile(
                                   Projectile.GetSource_None(),
                                   player.Center,
                                   Vector2.Zero,
                                   ModContent.ProjectileType<GoldRush_Gate>(),
                                   shotDamage,
                                   0f,
                                   player.whoAmI,
                                   target.whoAmI // ターゲット情報は一応渡しておく
                                );

                                if (modPlayer2.echoAccessory)
                                {
                                    Projectile.NewProjectile(
                                   Projectile.GetSource_None(),
                                   player.Center,
                                   Vector2.Zero,
                                   ModContent.ProjectileType<GoldRush_Gate>(),
                                   shotDamage,
                                   0f,
                                   player.whoAmI,
                                   target.whoAmI // ターゲット情報は一応渡しておく
                                );
                                }

                                GoldRushManager.OnSpawnGate(5);
                            }
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<AurelianSanctum>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx14>(),
                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.FlatTagDamage = 30;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleAurelianSanctum",
                                    AssetRequestMode.ImmediateLoad
                                    );


                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            float dr = npc.GetGlobalNPC<CalamityGlobalNPC>().DR;

                            if (dr >= 0.9f)
                                return;

                            Player player = Main.player[projectile.owner];

                            if (projectile.owner != Main.myPlayer) return;

                            if (player.HeldItem.type != ModContent.ItemType<AurelianSanctum>())
                                return;

                            bool isLargeWorm = WormLikeNPCUtils.IsLargeWormLike(npc);

                            NPC target = npc;
                            NPC cooldownTarget = npc;

                            if (isLargeWorm && npc.realLife >= 0)
                            {
                                // クールダウンは頭で共有
                                cooldownTarget = Main.npc[npc.realLife];
                            
                                // DRを突破できる部位を記録
                                GateTargetManager.LastWormTarget = npc.whoAmI;

                                // 前回記録した部位をターゲットにする
                                if (GateTargetManager.LastWormTarget >= 0 &&
                                    GateTargetManager.LastWormTarget < Main.maxNPCs &&
                                    Main.npc[GateTargetManager.LastWormTarget].active)
                                {
                                    target = Main.npc[GateTargetManager.LastWormTarget];
                                }
                            }


                            var modNPC = cooldownTarget.GetGlobalNPC<GateTagNPC>();

                            if (modNPC.gateCooldown > 0)
                                return;

                            modNPC.gateCooldown = 5;


                            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
                            var modPlayer2 = player.GetModPlayer<EchoWhipPlayer>();

                            int shotDamage = (int)(damageDone * (modPlayer.buddyEmblem ? 0.8f : (modPlayer2.echoAccessory ? 1.75f : 1.0f)));

                            if (GoldRushManager.CanSpawnGate())
                            {
                                // 位置は player.Center でとりあえず生成（ゲートAIが即座に背後へ配置するため）
                                Projectile.NewProjectile(
                                   Projectile.GetSource_None(),
                                   player.Center,
                                   Vector2.Zero,
                                   ModContent.ProjectileType<GoldRush_Gate>(),
                                   shotDamage,
                                   0f,
                                   player.whoAmI,
                                   target.whoAmI // ターゲット情報は一応渡しておく
                                );

                                if (modPlayer2.echoAccessory)
                                {
                                    Projectile.NewProjectile(
                                   Projectile.GetSource_None(),
                                   player.Center,
                                   Vector2.Zero,
                                   ModContent.ProjectileType<GoldRush_Gate>(),
                                   shotDamage,
                                   0f,
                                   player.whoAmI,
                                   target.whoAmI // ターゲット情報は一応渡しておく
                                );
                                }

                                GoldRushManager.OnSpawnGate(2);
                            }
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<KingsMajesty>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx15>(),

                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.FlatTagDamage = 2;
                        tag.TagCritChance = 0.04f;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                            "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleKingsMajesty",
                            AssetRequestMode.ImmediateLoad
                        );

                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            Player player = Main.player[projectile.owner];
                            if (projectile.owner != Main.myPlayer)
                                return;

                            if (!npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx15>()))
                                return;

                            Projectile gem = null;

                            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
                            var modPlayer2 = player.GetModPlayer<EchoWhipPlayer>();

                            int shotDamage = (int)(damageDone * (modPlayer.buddyEmblem ? 0.4f : (modPlayer2.echoAccessory ? 1.0f : 0.5f)));
                            int chargingDamage = (int)(damageDone * (modPlayer.buddyEmblem ? 0.8f : (modPlayer2.echoAccessory ? 1.2f : 1.0f)));

                            for (int i = 0; i < Main.maxProjectiles; i++)
                            {
                                Projectile p = Main.projectile[i];

                                if (p.active &&
                                    p.owner == projectile.owner &&
                                    p.type == ModContent.ProjectileType<KingsMajestyGem>())
                                {
                                    gem = p;
                                    break;
                                }
                            }

                            if (gem == null)
                                return;

                            var g = gem.ModProjectile as KingsMajestyGem;

                            if (g.State == KingsMajestyGem.GemState.Charging)
                                g.AddDamage(chargingDamage);
                            else
                                g.RequestCharge(npc, shotDamage);
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<EmeraldSplash>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx16>(),

                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.FlatTagDamage = 5;
                        tag.TagCritChance = 0.07f;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                            "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleEmeraldSplash",
                            AssetRequestMode.ImmediateLoad
                        );

                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            Player player = Main.player[projectile.owner];
                            if (projectile.owner != Main.myPlayer)
                                return;

                            if (!npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx16>()))
                                return;

                            Projectile gem = null;

                            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
                            var modPlayer2 = player.GetModPlayer<EchoWhipPlayer>();

                            int shotDamage = (int)(damageDone * (modPlayer.buddyEmblem ? 0.8f : (modPlayer2.echoAccessory ? 1.2f : 1.0f)));
                            int chargingDamage = (int)(damageDone * (modPlayer.buddyEmblem ? 0.8f : (modPlayer2.echoAccessory ? 1.2f : 1.0f)));

                            for (int i = 0; i < Main.maxProjectiles; i++)
                            {
                                Projectile p = Main.projectile[i];

                                if (p.active &&
                                    p.owner == projectile.owner &&
                                    p.type == ModContent.ProjectileType<EmeraldSplashGem>())
                                {
                                    gem = p;
                                    break;
                                }
                            }

                            if (gem == null)
                                return;

                            var g = gem.ModProjectile as EmeraldSplashGem;

                            if (g.State == EmeraldSplashGem.GemState.Charging)
                                g.AddDamage(chargingDamage);
                            else
                                g.RequestCharge(npc, shotDamage);
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<CrackoftheUniverse>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx17>(),

                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.FlatTagDamage = 8;
                        tag.TagCritChance = 0.06f;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                            "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleCrackoftheUniverse",
                            AssetRequestMode.ImmediateLoad
                        );

                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            Player player = Main.player[projectile.owner];
                            if (projectile.owner != Main.myPlayer)
                                return;

                            if (!npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx17>()))
                                return;

                            Projectile gem = null;

                            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
                            var modPlayer2 = player.GetModPlayer<EchoWhipPlayer>();

                            int shotDamage = (int)(damageDone * (modPlayer.buddyEmblem ? 0.8f : (modPlayer2.echoAccessory ? 1.5f : 1.0f)));
                            int chargingDamage = (int)(damageDone * (modPlayer.buddyEmblem ? 0.8f : (modPlayer2.echoAccessory ? 1.5f : 1.0f)));

                            for (int i = 0; i < Main.maxProjectiles; i++)
                            {
                                Projectile p = Main.projectile[i];

                                if (p.active &&
                                    p.owner == projectile.owner &&
                                    p.type == ModContent.ProjectileType<CrackoftheUniverseGem>())
                                {
                                    gem = p;
                                    break;
                                }
                            }

                            if (gem == null)
                                return;

                            var g = gem.ModProjectile as CrackoftheUniverseGem;

                            if (g.State == CrackoftheUniverseGem.GemState.Charging)
                                g.AddDamage(chargingDamage);
                            else
                                g.RequestCharge(npc, shotDamage);

                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<Rubellus>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx18>(),

                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.FlatTagDamage = 8;
                        tag.TagCritChance = 0.04f;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                            "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleRubellus",
                            AssetRequestMode.ImmediateLoad
                        );

                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            Player player = Main.player[projectile.owner];
                            if (projectile.owner != Main.myPlayer)
                                return;

                            if (!npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx18>()))
                                return;

                            Projectile gem = null;

                            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
                            var modPlayer2 = player.GetModPlayer<EchoWhipPlayer>();

                            int shotDamage = (int)(damageDone * (modPlayer.buddyEmblem ? 0.8f : (modPlayer2.echoAccessory ? 1.2f : 1.0f)));
                            int chargingDamage = (int)(damageDone * (modPlayer.buddyEmblem ? 0.8f : (modPlayer2.echoAccessory ? 1.2f : 1.0f)));

                            for (int i = 0; i < Main.maxProjectiles; i++)
                            {
                                Projectile p = Main.projectile[i];

                                if (p.active &&
                                    p.owner == projectile.owner &&
                                    p.type == ModContent.ProjectileType<RubellusGem>())
                                {
                                    gem = p;
                                    break;
                                }
                            }

                            if (gem == null)
                                return;

                            var g = gem.ModProjectile as RubellusGem;

                            if (g.State == RubellusGem.GemState.Charging)
                                g.AddDamage(chargingDamage);
                            else
                                g.RequestCharge(npc, shotDamage);
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<WulfrumArm>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx19>(),
                    Setup = tag =>
                    {

                        tag.FlatTagDamage = 4;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleWulfrumArm",
                                    AssetRequestMode.ImmediateLoad
                                    );

                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            Player player = Main.player[projectile.owner];
                            if (projectile.owner != Main.myPlayer)
                                return;

                            if (!npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx19>()))
                                return;

                            bool isLargeWorm =
                                WormLikeNPCUtils.IsLargeWormLike(npc);

                            int lightningDamage =
                                (int)(damageDone *
                                (isLargeWorm ? 0.30f : 0.60f));

                            WulfrumLightningSystem.StartChain(
                                projectile,
                                npc,
                                lightningDamage);

                            int buffType = ModContent.BuffType<SimpleWhipDebuffEx19>();
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
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<MechanicalArm>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx20>(),
                    Setup = tag =>
                    {

                        tag.FlatTagDamage = 15;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleMechanicalArm",
                                    AssetRequestMode.ImmediateLoad
                                    );

                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            Player player = Main.player[projectile.owner];
                            if (projectile.owner != Main.myPlayer)
                                return;

                            if (!npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx20>()))
                                return;

                            bool isLargeWorm =
                                WormLikeNPCUtils.IsLargeWormLike(npc);

                            int lightningDamage =
                                (int)(damageDone *
                                (isLargeWorm ? 0.50f : 1.0f));

                            MechanicalLightningSystem.StartChain(
                                projectile,
                                npc,
                                lightningDamage);

                            int buffType = ModContent.BuffType<SimpleWhipDebuffEx20>();
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
                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<MassofWailing>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx21>(),
                    Setup = tag =>
                    {

                        tag.TagCritChance = 0.05f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleMassofWailing",
                                    AssetRequestMode.ImmediateLoad
                                    );

                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            Player player = Main.player[projectile.owner];
                            if (projectile.owner != Main.myPlayer)
                                return;

                            float dr = npc.GetGlobalNPC<CalamityGlobalNPC>().DR;

                            if (dr >= 0.9f)
                                return;

                            if (!npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx21>()))
                                return;

                            var mp = player.GetModPlayer<BuddyEmblemPlayer>();

                            float stored = mp.buddyEmblem
                                ? (damageDone * 0.20f)
                                : (damageDone * 0.15f);

                            WhipChargeSystem.AddCharge(
                                player,
                                npc,
                                projectile,
                                hit,
                                stored);

                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<ChorusofExecration>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx22>(),
                    Setup = tag =>
                    {

                        tag.TagCritChance = 0.1f;
                        tag.AutoDrawTooltip = false;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                                    "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleChorusofExecration",
                                    AssetRequestMode.ImmediateLoad
                                    );

                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            Player player = Main.player[projectile.owner];
                            if (projectile.owner != Main.myPlayer)
                                return;

                            float dr = npc.GetGlobalNPC<CalamityGlobalNPC>().DR;

                            if (dr >= 0.9f)
                                return;

                            if (!npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx22>()))
                                return;

                            var mp = player.GetModPlayer<BuddyEmblemPlayer>();

                            float stored = mp.buddyEmblem
                                ? (damageDone * 0.25f)
                                : (damageDone * 0.2f);

                            WhipChargeSystem.AddCharge(
                                player,
                                npc,
                                projectile,
                                hit,
                                stored);

                        };
                    }
                },

                new SummonTagEntry
                {
                    ItemType = () => ModContent.ItemType<OpaqueOnyx>(),
                    BuffType = () => ModContent.BuffType<SimpleWhipDebuffEx23>(),

                    Setup = tag =>
                    {
                        tag.AutoDrawTooltip = false;
                        tag.FlatTagDamage = 3;
                        tag.TagCritChance = 0.05f;

                        tag.TagTexture = ModContent.Request<Texture2D>(
                            "CalamitySimpleWhipAddon/Content/Items/InvisibleSummonTagItem/InvisibleOpaqueOnyx",
                            AssetRequestMode.ImmediateLoad
                        );

                        tag.TagOnHit = (NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) =>
                        {
                            if (!projectile.minion && !ProjectileID.Sets.MinionShot[projectile.type] && !projectile.sentry && !ProjectileID.Sets.SentryShot[projectile.type])
                                return;

                            Player player = Main.player[projectile.owner];
                            if (projectile.owner != Main.myPlayer)
                                return;

                            if (!npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx23>()))
                                return;

                            Projectile gem = null;

                            var modPlayer = player.GetModPlayer<BuddyEmblemPlayer>();
                            var modPlayer2 = player.GetModPlayer<EchoWhipPlayer>();

                            int shotDamage = (int)(damageDone * (modPlayer.buddyEmblem ? 0.5f : (modPlayer2.echoAccessory ? 1.1f : 0.75f)));
                            int chargingDamage = (int)(damageDone * (modPlayer.buddyEmblem ? 0.8f : (modPlayer2.echoAccessory ? 1.2f : 1.0f)));

                            for (int i = 0; i < Main.maxProjectiles; i++)
                            {
                                Projectile p = Main.projectile[i];

                                if (p.active &&
                                    p.owner == projectile.owner &&
                                    p.type == ModContent.ProjectileType<OpaqueOnyxGem>())
                                {
                                    gem = p;
                                    break;
                                }
                            }

                            if (gem == null)
                                return;

                            var g = gem.ModProjectile as OpaqueOnyxGem;

                            if (g.State == OpaqueOnyxGem.GemState.Charging)
                                g.AddDamage(chargingDamage);
                            else
                                g.RequestCharge(npc, shotDamage);
                        };
                    }
                },
            };

            foreach (var e in entries)
            {
                var tag = new SummonTag(e.ItemType());
                e.Setup?.Invoke(tag);

                int debuff = e.BuffType();

                CalamityBuffSets.SummonTagDebuff[debuff] = tag;
            }
        }
    }

    public static class GateTargetManager
    {
        public static int LastWormTarget = -1;
    }
}
