using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;


namespace CalamitySimpleWhipAddon.Content.Projectiles
{


    public class DestructionChainProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 39;
            Projectile.WhipSettings.RangeMultiplier = 3.2f;
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
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuff2x>(), 240);
            target.AddBuff(BuffID.OnFire3,120);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.96f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);

            SimpleWhipDrawer5.DrawSegments(Projectile, points, Timer);

            return false;
        }

        public override void AI()
        {
            base.AI();

            Player owner = Main.player[Projectile.owner];

            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            float swingProgress = Projectile.ai[0] / swingTime;

            if (Utils.GetLerpValue(0.1f, 0.7f, swingProgress, true) *
                Utils.GetLerpValue(0.9f, 0.7f, swingProgress, true) > 0.5f &&
                !Main.rand.NextBool(30))
            {
                List<Vector2> points = Projectile.WhipPointsForCollision;

                int pointIndex = Main.rand.Next(points.Count - 20, points.Count);
                Rectangle spawnArea = Utils.CenteredRectangle(points[pointIndex], new Vector2(30f, 30f));

				int dustType = Main.rand.NextBool(3) ? 16 : 174;

                Dust dust = Dust.NewDustDirect(spawnArea.TopLeft(), spawnArea.Width, spawnArea.Height,
                    dustType, 0f, 0f, 100, Color.White);

                dust.position = points[pointIndex];
                dust.fadeIn = 0.3f;
                dust.scale = Main.rand.NextFloat(0.5f, 1.2f);
                dust.noGravity = false;

                Vector2 spinningPoint = points[pointIndex] - points[pointIndex - 1];
                dust.velocity *= 1f;
                dust.velocity += spinningPoint.RotatedBy(owner.direction * ((float)Math.PI / 2f));
                dust.velocity *= 1f;


				foreach (var p in points)
                {
                    Lighting.AddLight(p, 0.6f, 0.1f, 0.1f);
                }
            }

			if (Main.rand.NextBool(10) && Main.netMode != NetmodeID.Server)
			{
				// ムチの制御点取得
				List<Vector2> points = Projectile.WhipPointsForCollision;

				// ムチ先端座標
				Vector2 tipPos = points[points.Count - 1];

				// ムチ先端の前の点（方向取得用）
				Vector2 prevPos = points[points.Count - 2];

				// 先端の移動方向
				Vector2 direction = (tipPos - prevPos).SafeNormalize(Vector2.Zero);

				// バニラの炎スモークゴア (375~377)
				int goreID = Main.rand.Next(375, 378);

				int gore = Gore.NewGore(
					Projectile.GetSource_FromAI(),
					tipPos,                     // ★ ここが重要：Projectile.Center ではなくムチの先端
					direction * 2f,             // ★ ムチの振り方向に飛ばす
					goreID,
					0.8f
				);

				Main.gore[gore].behindTiles = true;
			}


		}

    }
}