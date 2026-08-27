using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Rendering;
using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Items.Weapons;
using CalamityMod.NPCs;

namespace CalamitySimpleWhipAddon.Content.Systems
{
    public static class WhipChargeSystem
    {
        public const int ReleaseThreshold = 100;

        public static bool HasAnyCharge
        {
            get
            {
                foreach (Projectile projectile in Main.ActiveProjectiles)
                {
                    if (projectile.ModProjectile is ChargeCoreProjectile)
                        return true;
                }

                return false;
            }
        }

        public static void Update()
        {
        }

        public static void AddCharge(
            Player player,
            NPC npc,
            Projectile projectile,
            NPC.HitInfo hit,
            float stored)
        {
            ChargeTheme theme = GetHeldTheme(player);

            if (theme == ChargeTheme.None)
                return;

            var mp = player.GetModPlayer<BuddyEmblemPlayer>();

            float chargeAmount = 1f;

            if (mp.buddyEmblem && mp.BuddyEmblemBonus >= 1.01f)
                chargeAmount *= mp.BuddyEmblemBonus * 0.9f;

            Projectile core = GetOrCreateCore(player, theme);

            if (core == null)
                return;

            Vector2 baseDir = core.Center - npc.Center;
            Vector2 velocity = GetPerpendicularBothSides(baseDir, Main.rand.NextFloat(6f, 9f), 0.6f);

            int ballID = Projectile.NewProjectile(
                player.GetSource_Misc("ChargeBall"),
                npc.Center,
                velocity,
                ModContent.ProjectileType<ChargeBallProjectile>(),
                0,
                0,
                player.whoAmI,
                stored,
                (float)theme,
                chargeAmount);

            if (ballID >= 0 && ballID < Main.maxProjectiles)
                Main.projectile[ballID].netUpdate = true;

            BaseChargeMetaball.SpawnExplosionMetaballs(npc.Center, 5, stored, theme);
        }

        public static ChargeTheme GetHeldTheme(Player player)
        {
            if (player.HeldItem.type == ModContent.ItemType<MassofWailing>())
                return ChargeTheme.massofWailing;

            if (player.HeldItem.type == ModContent.ItemType<ChorusofExecration>())
                return ChargeTheme.chorusofExecration;

            return ChargeTheme.None;
        }

        public static Projectile GetOrCreateCore(Player player, ChargeTheme theme)
        {
            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.owner == player.whoAmI &&
                    projectile.ModProjectile is ChargeCoreProjectile)
                {
                    return projectile;
                }
            }

            int id = Projectile.NewProjectile(
                player.GetSource_Misc("ChargeCore"),
                player.Center + new Vector2(0f, -120f),
                Vector2.Zero,
                ModContent.ProjectileType<ChargeCoreProjectile>(),
                0,
                0,
                player.whoAmI,
                0f,
                (float)theme,
                0f);

            if (id < 0 || id >= Main.maxProjectiles)
                return null;

            Main.projectile[id].netUpdate = true;
            return Main.projectile[id];
        }

        public static Vector2 GetPerpendicularBothSides(Vector2 baseDir, float speed, float spread)
        {
            if (baseDir.LengthSquared() < 0.001f)
                return Main.rand.NextVector2CircularEdge(speed, speed);

            baseDir = Vector2.Normalize(baseDir);

            Vector2 perp = new(-baseDir.Y, baseDir.X);

            if (Main.rand.NextBool())
                perp *= -1f;

            float angleOffset = Main.rand.NextFloat(-spread, spread);

            Vector2 dir = Vector2.Lerp(baseDir, perp, 0.85f);

            dir = Vector2.Normalize(dir);
            dir = Vector2.Transform(dir, Matrix.CreateRotationZ(angleOffset));

            return dir * speed;
        }

        public static NPC FindTarget(Player player)
        {
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];

                if (npc.active &&
                    !npc.friendly &&
                    !npc.dontTakeDamage &&
                    npc.GetGlobalNPC<CalamityGlobalNPC>().DR < 0.9f &&
                    Vector2.Distance(player.Center, npc.Center) <= 2000f)
                {
                    return npc;
                }
            }

            NPC taggedTarget = null;
            float taggedDist = 2000f;

            NPC normalTarget = null;
            float normalDist = 2000f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.active || npc.friendly || npc.dontTakeDamage)
                    continue;

                if (npc.GetGlobalNPC<CalamityGlobalNPC>().DR >= 0.9f)
                    continue;

                float dist = Vector2.Distance(player.Center, npc.Center);

                if (dist > 2000f)
                    continue;

                if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx21>()) && npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx22>()))
                {
                    if (taggedTarget == null || dist < taggedDist)
                    {
                        taggedTarget = npc;
                        taggedDist = dist;
                    }
                }
                else if (normalTarget == null || dist < normalDist)
                {
                    normalTarget = npc;
                    normalDist = dist;
                }
            }

            return taggedTarget ?? normalTarget;
        }
    }
}
