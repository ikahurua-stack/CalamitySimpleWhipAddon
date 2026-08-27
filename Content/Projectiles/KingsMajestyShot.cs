using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class KingsMajestyShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 2;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override string Texture =>
            "CalamityMod/Projectiles/Boss/JewelProjectile";

        public override void SetDefaults()
        {
            Projectile.netImportant = true;

            Projectile.width = 10;
            Projectile.height = 10;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 700;

            Projectile.tileCollide = false;

            Projectile.penetrate = 4;

            Projectile.timeLeft = 600;

            Projectile.extraUpdates = 1;

            Projectile.DamageType = DamageClass.Summon;

            Projectile.ArmorPenetration = 9999;
        }

        public override void AI()
        {
            Projectile.rotation += 0.3f * Projectile.direction;

            for (int i = 0; i < 2; i++)
            {
                int ruby = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GemRuby);

                Main.dust[ruby].noGravity = true;
                Main.dust[ruby].velocity *= 0.3f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
        }
    }

}