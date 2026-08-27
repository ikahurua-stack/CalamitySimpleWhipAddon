using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using CalamityMod.Particles;
using CalamityMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework.Graphics;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamitySimpleWhipAddon.Content.Buffs;
using static CalamityMod.CalamityUtils;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
	public class DestructionChainTag : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 190;
			Projectile.height = 190;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 3;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

            Projectile.scale = 1.3f;

            // ミニオン攻撃扱いにする
            Projectile.DamageType = DamageClass.Summon;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override void AI()
		{
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                Explode();
            }

            for (int i = 0; i < 12; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.InfernoFork, 0f, 0f, 150, default, 1.2f);
                Main.dust[dust].velocity *= 1.8f;
                Main.dust[dust].noGravity = false;
            }

            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.75f / 255f, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.01f / 255f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            var globalNPC = target.GetGlobalNPC<ChainExplosionGlobalNPC>();

            // 当たるたびにカウント増加
            globalNPC.explosionHitCount++;

            // 減衰率
            float falloff = 1f - globalNPC.explosionHitCount * 0.25f;

            // 下限
            falloff = MathHelper.Clamp(falloff, 0f, 1f);

            Projectile.damage = (int)(Projectile.damage * falloff);


            // このヒットで死亡したか？
            bool died =
                hit.InstantKill ||
                target.life <= 0;

            if (!died)
                return;

            int chainDepth = (int)Projectile.ai[0];

            // 無限連鎖防止
            if (chainDepth >= 5)
                return;

            int newDamage = (int)(Projectile.damage * 0.75f);
            if (newDamage <= 0)
                return;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                target.Center,
                Vector2.Zero,
                ModContent.ProjectileType<DestructionChainTag2>(),
                newDamage,
                0f,
                Projectile.owner,
                chainDepth + 1
            );

            SoundEngine.PlaySound(SoundID.Item14, target.Center);
        }




        public void Explode()
        {
            

            Particle explosion3 = new CustomPulse(Projectile.Center, Vector2.Zero, Color.Orchid, "CalamityMod/Particles/ShatteredExplosion", Vector2.One, Main.rand.NextFloat(MathHelper.TwoPi), 0f, 0.135f, 30);
            GeneralParticleHandler.SpawnParticle(explosion3);
            Particle blastRing = new CustomPulse(Projectile.Center, Vector2.Zero, Color.OrangeRed, "CalamityMod/Particles/BloomCircle", Vector2.One, Main.rand.NextFloat(MathHelper.TwoPi), 1f, 2.5f, 25, true);
            GeneralParticleHandler.SpawnParticle(blastRing);
            Particle blastRing2 = new CustomPulse(Projectile.Center, Vector2.Zero, Color.OrangeRed, "CalamityMod/Particles/FlameExplosion", Vector2.One, Main.rand.NextFloat(MathHelper.TwoPi), 0, 0.135f, 25, true, 0.9f);
            GeneralParticleHandler.SpawnParticle(blastRing2);

            for (int i = 0; i < 15; i++)
            {
                Vector2 randVel = new Vector2(12, 12).RotatedByRandom(100) * Main.rand.NextFloat(0.5f, 1.2f);
                Particle smoke = new HeavySmokeParticle(Projectile.Center + randVel, randVel, Color.Black, Main.rand.Next(20, 25 + 1), Main.rand.NextFloat(0.7f, 2f), 0.7f);
                GeneralParticleHandler.SpawnParticle(smoke);
            }
        }
    }
}
