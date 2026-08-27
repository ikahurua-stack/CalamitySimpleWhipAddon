using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Systems;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class RubellusGem_Damage : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.netImportant = true;

            Projectile.width = 4;
            Projectile.height = 4;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 2;

            Projectile.penetrate = 1;

            Projectile.DamageType = DamageClass.Summon;

            Projectile.hide = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.ArmorPenetration = 9999;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void AI()
        {
            NPC target = Main.npc[(int)Projectile.ai[0]];

            if (!target.active)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = target.Center;
        }

    }
}
