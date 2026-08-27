using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class MandibleLash_Explosion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 120;   // 爆発半径
            Projectile.height = 120;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 4;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.ArmorPenetration = 9999;

            Projectile.DamageType = DamageClass.Summon;
            Projectile.alpha = 255; // 完全透明
        }

        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false; // 描画しない
        }
    }
}
