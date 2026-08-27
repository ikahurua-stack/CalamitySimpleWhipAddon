using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using CalamitySimpleWhipAddon.Content.Common.GlobalNPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using System;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{


    public class EmeraldSplashProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 75;
            Projectile.WhipSettings.RangeMultiplier = 1.97f;
        }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private float ChargeTime
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }



        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuffEx16>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.76f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);

            SimpleWhipDrawer14.DrawSegments(Projectile, points, Timer);

            return false;
        }

        public override void AI()
        {
            base.AI();

            Player owner = Main.player[Projectile.owner];

            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            float swingProgress = Projectile.ai[0] / swingTime;

            if (Utils.GetLerpValue(0.1f, 0.7f, swingProgress, true) *
                Utils.GetLerpValue(0.9f, 0.7f, swingProgress, true) > 0.5f
                && Main.rand.NextBool(1))
            {
                List<Vector2> points = Projectile.WhipPointsForCollision;

                if (points.Count < 2)
                    return;

                int spawnCount = Main.rand.Next(1, 2);

                for (int i = 0; i < spawnCount; i++)
                {
                    int pointIndex = Main.rand.Next(points.Count - 10, points.Count);

                    Vector2 p1 = points[pointIndex];
                    Vector2 p2 = points[pointIndex - 1];

                    // 鞭上のランダム位置
                    Vector2 spawnPos = Vector2.Lerp(p1, p2, Main.rand.NextFloat());

                    // 少し拡散
                    spawnPos += new Vector2(
                        Main.rand.NextFloat(-12f, 12f),
                        Main.rand.NextFloat(-12f, 12f)
                    );

                    if (Vector2.Distance(spawnPos, owner.Center) < 60f)
                        continue;

                    Vector2 spinningPoint = p1 - p2;
                    Vector2 whipDir = spinningPoint.SafeNormalize(Vector2.UnitX);

                    Vector2 toMouse = Main.MouseWorld - spawnPos;
                    toMouse.Normalize();

                    Vector2 finalDir = Vector2.Lerp(whipDir, toMouse, 0.4f);

                    Vector2 vel = finalDir;
                    vel *= Main.rand.NextFloat(1.8f, 3f);

                    ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.TerraBlade, new ParticleOrchestraSettings()
                    {
                        PositionInWorld = spawnPos,
                        MovementVector = vel
                    });



                }
            }
        }

    }
}