using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class DarkMandibleLash_Explosion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 160;   // 爆発半径
            Projectile.height = 160;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;

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
