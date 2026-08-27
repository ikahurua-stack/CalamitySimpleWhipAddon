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
	public class GloamshardConduitTag : ModProjectile
	{
        public Player Owner => Main.player[Projectile.owner];
        public Color color1 = Color.LightGreen;
        public Color color2 = Color.Black;

        public override void SetDefaults()
		{
			Projectile.width = 150;
			Projectile.height = 150;
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


            float scaleMain = 0.45f;
            float scaleSub1 = 0.3f;
            float scaleSub2 = 0.2f;

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
            for (int i = 0; i < 25; i++) // 数を増やすと密度が上がる
            {
                // 爆心からの初期位置（広がり具合）
                Vector2 offset = Main.rand.NextVector2Circular(15f, 25f);
                // ↑ 40f を大きくすると最初から広い範囲に散る

                // 速度（爆発の勢い）
                Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f)
                    * Main.rand.NextFloat(3f, 12f);
                // ↑ 6f～14f を上げると勢いが強くなる

                int dustType = Main.rand.NextBool(2) ? 66 : 263;
                // ↑ 1/3 の確率で 278、残りは 263
                // NextBool(2) → 半々
                // NextBool(4) → 278 が少なめ

                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center + offset,
                    dustType,
                    velocity
                );
               
                dust.noGravity = true; // true: 空中で散る / false: 落下
                dust.color = color1;

                // サイズ（爆発感の要）
                dust.scale = Main.rand.NextFloat(0.6f, 1.7f);
                // ↑ 2.0f 以上にすると「爆発ダスト」らしくなる

            }
        }
	}
}
