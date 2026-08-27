using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Particles;
using Terraria.Audio;
using CalamityMod.Items.Weapons.Ranged;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
	public class ResonantVoidTag : ModProjectile
	{
        public Player Owner => Main.player[Projectile.owner];
        public Color color1 = Color.Purple;
        public Color color2 = Color.Black;

        public override void SetDefaults()
		{
			Projectile.width = 400;
			Projectile.height = 400;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 3;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

			// ミニオン攻撃扱いにする
			Projectile.DamageType = DamageClass.Summon;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;

            Projectile.ArmorPenetration = 9999;
        }

		public override void AI()
		{

            if (Projectile.timeLeft == 3)
            {
                SoundEngine.PlaySound(DeadSunsWind.Explosion with { Volume = 0.5f }, Projectile.Center);
            }


            float scaleMain = 1.125f;
            float scaleSub1 = 0.75f;
            float scaleSub2 = 0.5f;

            Particle explosion = new DetailedExplosion(Projectile.Center, Vector2.Zero, color1, Vector2.One, Main.rand.NextFloat(-5, 5), 0f, scaleMain, Main.rand.Next(15, 22));
            GeneralParticleHandler.SpawnParticle(explosion);
            Particle explosion2 = new DetailedExplosion(Projectile.Center, Vector2.Zero, Color.Black, Vector2.One, Main.rand.NextFloat(-5, 5), 0f, scaleSub1, Main.rand.Next(15, 22), false);
            GeneralParticleHandler.SpawnParticle(explosion2);
            Particle explosion3 = new DetailedExplosion(Projectile.Center, Vector2.Zero, Color.Black, Vector2.One, Main.rand.NextFloat(-5, 5), 0f, scaleSub2, Main.rand.Next(15, 22), false);
            GeneralParticleHandler.SpawnParticle(explosion3);
            Particle orb = new GenericBloom(Projectile.Center, Projectile.velocity, color1, 1f, 10, true);
            GeneralParticleHandler.SpawnParticle(orb);
            Particle orb2 = new GenericBloom(Projectile.Center, Projectile.velocity, color2, 1f, 10, true, true);
            GeneralParticleHandler.SpawnParticle(orb2);
            
        }
	}
}
