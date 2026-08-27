using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;

using CalamityMod;
using CalamityMod.Particles;
using CalamitySimpleWhipAddon.Content.Systems;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class SkybreakerCoilTag : ModProjectile
    {
        private const int HitFrames = 20;
        private const int VisualFrames = 60;
        private bool spawnedEffects = false; // ★ 一度だけ爆発させる用
        private bool visualRegistered = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = HitFrames;
        }

        public override void SetDefaults()
        {
            Projectile.width = 153;
            Projectile.height = 153;

            Projectile.friendly = true;

            Projectile.penetrate = -1;
            Projectile.timeLeft = HitFrames;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.knockBack = 0f;

            Projectile.DamageType = DamageClass.Summon;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;

            Projectile.scale = 1.6f;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            if (!spawnedEffects)
            {
                spawnedEffects = true;
                SpawnElectricDust();
                SpawnSparkParticles();
            }

            if (!visualRegistered)
            {
                visualRegistered = true;

                AdditiveTagDrawer.RegisterSkybreaker(
                    Projectile.Center,
                    Projectile.rotation,
                    Projectile.scale,
                    VisualFrames
                );
            }

            if (++Projectile.frame >= HitFrames)
                Projectile.Kill();
        }

        // ---------------- 電気ダスト ----------------
        private void SpawnElectricDust()
        {
            Color randomColor = Color.Lerp(
                new Color(120, 220, 255),
                Color.White,
                Main.rand.NextFloat()
            );

            for (int i = 0; i < 5; i++) // 数を増やすと密度が上がる
            {
                // 爆心からの初期位置（広がり具合）
                Vector2 offset = Main.rand.NextVector2Circular(25f, 40f);
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
                dust.color = randomColor;

                // サイズ（爆発感の要）
                dust.scale = Main.rand.NextFloat(0.6f, 1.7f);
                // ↑ 2.0f 以上にすると「爆発ダスト」らしくなる

            }
        }

        // ---------------- SparkParticle（Calamity） ----------------
        private void SpawnSparkParticles()
        {
            Color randomColor = Color.Lerp(
                new Color(120, 220, 255),
                Color.White,
                Main.rand.NextFloat()
            );

            for (int i = 0; i < 5; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(12f, 12f);
                Vector2 velOffset = Main.rand.NextVector2CircularEdge(1f, 1f);

                SparkParticle spark = new SparkParticle(
                    Projectile.Center + offset,
                    velOffset * Main.rand.NextFloat(15.5f, 25.5f),
                    true,
                    95,
                    Main.rand.NextFloat(0.3f, 1.1f),
                    Color.Lerp(Color.White, randomColor, 0.3f)
                );

                GeneralParticleHandler.SpawnParticle(spark);
            }
        }

        

        // ================= Additive 描画 =================
        public override bool PreDraw(ref Color lightColor)
        {
            return false; // 通常描画を殺す
        }

    }
}

