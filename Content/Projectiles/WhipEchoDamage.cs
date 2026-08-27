using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class WhipEchoDamage : ModProjectile
    {
        private bool activated;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;

            Projectile.timeLeft = 10;

            Projectile.friendly = true;
            Projectile.tileCollide = false;

            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;

            Projectile.DamageType = DamageClass.Summon;

            Projectile.ArmorPenetration = 9999;
        }

        public override void AI()
        {
            if (!activated)
            {
                activated = true;
                Projectile.friendly = false;
                return;
            }

            Projectile.friendly = true;

            int npcIndex = (int)Projectile.ai[0];

            if (npcIndex >= 0 && npcIndex < Main.maxNPCs)
            {
                NPC npc = Main.npc[npcIndex];

                if (npc.active)
                    Projectile.Center = npc.Center;
                else
                    Projectile.Kill();
            }
        }
    }
}