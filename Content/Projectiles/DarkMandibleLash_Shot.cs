using CalamityMod.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class DarkMandibleLash_Shot : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = 1; // 再ヒット時に消える
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 0;

            Projectile.ArmorPenetration = 9999;

            Projectile.scale = 0.8f;

            Projectile.DamageType = DamageClass.Summon;
        }

        public override void OnSpawn(IEntitySource source)
        {
            // 真上に射出
            Projectile.velocity = new Vector2(
                Main.rand.NextFloat(-0.8f, 0.8f),
                -9f
            );

            // 回転はランダム開始
            Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }

        public override void AI()
        {
            // 常に回転
            Projectile.rotation += 0.35f;

            // 重力
            Projectile.velocity.Y += 0.4f;

            // 落下速度制限
            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;

            
                Vector2 spawnPos = Projectile.Bottom;

                Dust dust = Dust.NewDustPerfect(
                    spawnPos,
                    DustID.Ash,
                    new Vector2(
                        Main.rand.NextFloat(-1.0f, 1.0f),  // 横ブレ
                        Main.rand.NextFloat(1.5f, 3.5f)   // 下方向へ落ちる
                    )
                );

                dust.scale = Main.rand.NextFloat(0.8f, 1.2f);
                dust.noGravity = false;
            

        }


        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.velocity.Y < 0.5f)
                return false;

            return null;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SpawnSandBurst();
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SpawnSandBurst();

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_Death(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<DarkMandibleLash_Explosion>(),
                    (int)(Projectile.damage * 0.95f),
                    0f,
                    Projectile.owner
                );
            }

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 7; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Ash, 0f, 0f, Projectile.alpha, new Color(Math.Abs(Projectile.ai[2]), 0, 255, Projectile.alpha));
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity = Projectile.oldVelocity * 0.5f;
                Main.dust[dust].scale = Projectile.scale;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(
                Projectile,
                ProjectileID.Sets.TrailingMode[Type],
                lightColor,
                1
            );

            return false;
        }

        private void SpawnSandBurst()
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = new Vector2(
                    Main.rand.NextFloat(-6f, 6f),
                    Main.rand.NextFloat(-4f, 2f)
                );

                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Ash,
                    velocity
                );

                dust.scale = Main.rand.NextFloat(1.0f, 1.5f);
                dust.noGravity = false;
            }

            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
        }

        

    }
}
