using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class ExistenceBonds_Tick : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = 1;
            Projectile.timeLeft = 2;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.ArmorPenetration = 15;

            Projectile.DamageType = DamageClass.Summon;

            Projectile.hide = true; // 描画されない
        }

        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }

        public override void AI()
        {
            Projectile.velocity = Microsoft.Xna.Framework.Vector2.Zero;
        }
    }
}
