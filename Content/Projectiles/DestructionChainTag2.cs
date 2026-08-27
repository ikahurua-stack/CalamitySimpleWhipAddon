using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using CalamityMod.Particles;
using CalamityMod.Items.Weapons.Melee;
using CalamitySimpleWhipAddon.Content.Buffs;
using Microsoft.Xna.Framework.Graphics;
using static CalamityMod.CalamityUtils;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
	public class DestructionChainTag2 : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 190;
			Projectile.height = 190;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 30;
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
            Projectile.ai[1]++; // タイマー

            int delay = 12;

            if (Projectile.ai[1] == delay)
            {
                // この瞬間に爆発有効化
                Projectile.friendly = true;

                Projectile.scale *= 1.05f;
                Projectile.alpha += 10;

                Explode();
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            }

            // 爆発後はすぐ消す
            if (Projectile.ai[1] > delay + 2)
                Projectile.Kill();

            for (int i = 0; i < 12; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.InfernoFork, 0f, 0f, 150, default, 1.2f);
                Main.dust[dust].velocity *= 1.8f;
                Main.dust[dust].noGravity = false;
            }

            int chainDepth = (int)Projectile.ai[0];

            float lightMultiplier = 1f - chainDepth * 0.15f;
            lightMultiplier = MathHelper.Clamp(lightMultiplier, 0.3f, 1f);

            Lighting.AddLight(
                Projectile.Center,
                1.2f * lightMultiplier,
                0.6f * lightMultiplier,
                0.1f * lightMultiplier
            );

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

            target.AddBuff(ModContent.BuffType<SimpleWhipDebuff2x>(), 240);
            // このヒットで死亡したか？
            bool died =
                hit.InstantKill ||
                target.life <= 0;

            if (!died)
                return;

            int chainDepth = (int)Projectile.ai[0] + 1;

            // 無限連鎖防止
            if (chainDepth >= 10)
                return;

            int newDamage = (int)(Projectile.damage * 0.85f);
            if (newDamage <= 0)
                return;



            // 基本縮小率（連鎖ごとに少し小さく）
            float baseScale = 1f * (1f - chainDepth * 0.03f);

            // ランダム補正（±8%）
            float randomFactor = Main.rand.NextFloat(0.92f, 1.02f);

            float finalScale = baseScale * randomFactor;

            int p = Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                target.Center,
                Vector2.Zero,
                ModContent.ProjectileType<DestructionChainTag>(),
                newDamage,
                0f,
                Projectile.owner,
                chainDepth
            );

            // 生成後にscale設定
            Main.projectile[p].scale = finalScale;


            SoundStyle style = SoundID.Item14 with
            {
                Pitch = chainDepth * 0.07f // 連鎖ごとに少し上げる
            };

            SoundEngine.PlaySound(style, Projectile.Center);

        }




        public void Explode()
        {
            int chainDepth = (int)Projectile.ai[0];

            // 最大5連鎖前提で補間係数を計算
            float t = MathHelper.Clamp(chainDepth / 10f, 0f, 1f);

            // 紫 → 赤へ
            Color startColor = Color.OrangeRed;
            Color endColor = Color.Red;
            Color finalColor = Color.Lerp(startColor, endColor, t);


            Particle explosion3 = new CustomPulse(Projectile.Center, Vector2.Zero, finalColor, "CalamityMod/Particles/ShatteredExplosion", Vector2.One * Projectile.scale, Main.rand.NextFloat(MathHelper.TwoPi), 0f, 0.135f, 30);
            GeneralParticleHandler.SpawnParticle(explosion3);
            Particle blastRing = new CustomPulse(Projectile.Center, Vector2.Zero, finalColor, "CalamityMod/Particles/BloomCircle", Vector2.One * Projectile.scale, Main.rand.NextFloat(MathHelper.TwoPi), 1f, 2.5f, 25, true);
            GeneralParticleHandler.SpawnParticle(blastRing);
            Particle blastRing2 = new CustomPulse(Projectile.Center, Vector2.Zero, finalColor, "CalamityMod/Particles/FlameExplosion", Vector2.One * Projectile.scale, Main.rand.NextFloat(MathHelper.TwoPi), 0, 0.135f, 25, true, 0.9f);
            GeneralParticleHandler.SpawnParticle(blastRing2);

            
        }
    }
}
