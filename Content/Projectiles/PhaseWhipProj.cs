using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{


	public class GreenPhaseWhipProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAWhip[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.DefaultToWhip();
			Projectile.WhipSettings.Segments = 45;
			Projectile.WhipSettings.RangeMultiplier = 1.09f;
            Projectile.ArmorPenetration = 9999;
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
			target.AddBuff(ModContent.BuffType<SimpleWhipDebuff08>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.75f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> points = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, points);


			SimpleWhipDrawer3.DrawSegments(Projectile, points, Timer);

			return false;
		}

		public override void AI()
		{
			base.AI();

			List<Vector2> points = Projectile.WhipPointsForCollision;
			Projectile.FillWhipControlPoints(Projectile, points);

			foreach (var p in points)
			{
				Lighting.AddLight(p, 0.0f, 0.8f, 0.5f);
			}
		}

	}

	public class BluePhaseWhipProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAWhip[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.DefaultToWhip();
			Projectile.WhipSettings.Segments = 45;
			Projectile.WhipSettings.RangeMultiplier = 1.09f;
            Projectile.ArmorPenetration = 9999;
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
			target.AddBuff(ModContent.BuffType<SimpleWhipDebuff08>(), 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.75f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> points = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, points);


			SimpleWhipDrawer3.DrawSegments(Projectile, points, Timer);

			return false;
		}

		public override void AI()
		{
			base.AI();

			List<Vector2> points = Projectile.WhipPointsForCollision;
			Projectile.FillWhipControlPoints(Projectile, points);

			foreach (var p in points)
			{
				Lighting.AddLight(p, 0.0f, 0.2f, 0.8f);
			}
		}

	}

	public class OrangePhaseWhipProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAWhip[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.DefaultToWhip();
			Projectile.WhipSettings.Segments = 45;
			Projectile.WhipSettings.RangeMultiplier = 1.09f;
            Projectile.ArmorPenetration = 9999;
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
			target.AddBuff(ModContent.BuffType<SimpleWhipDebuff08>(), 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.75f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> points = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, points);


			SimpleWhipDrawer3.DrawSegments(Projectile, points, Timer);

			return false;
		}

		public override void AI()
		{
			base.AI();

			List<Vector2> points = Projectile.WhipPointsForCollision;
			Projectile.FillWhipControlPoints(Projectile, points);

			foreach (var p in points)
			{
				Lighting.AddLight(p, 0.8f, 0.4f, 0.0f);
			}
		}

	}

	public class PurplePhaseWhipProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAWhip[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.DefaultToWhip();
			Projectile.WhipSettings.Segments = 45;
			Projectile.WhipSettings.RangeMultiplier = 1.09f;
            Projectile.ArmorPenetration = 9999;
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
			target.AddBuff(ModContent.BuffType<SimpleWhipDebuff08>(), 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.75f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> points = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, points);


			SimpleWhipDrawer3.DrawSegments(Projectile, points, Timer);

			return false;
		}

		public override void AI()
		{
			base.AI();

			List<Vector2> points = Projectile.WhipPointsForCollision;
			Projectile.FillWhipControlPoints(Projectile, points);

			foreach (var p in points)
			{
				Lighting.AddLight(p, 0.4f, 0.0f, 0.6f);
			}
		}

	}

	public class RedPhaseWhipProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAWhip[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.DefaultToWhip();
			Projectile.WhipSettings.Segments = 45;
			Projectile.WhipSettings.RangeMultiplier = 1.09f;
            Projectile.ArmorPenetration = 9999;
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
			target.AddBuff(ModContent.BuffType<SimpleWhipDebuff08>(), 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.75f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> points = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, points);


			SimpleWhipDrawer3.DrawSegments(Projectile, points, Timer);

			return false;
		}

		public override void AI()
		{
			base.AI();

			List<Vector2> points = Projectile.WhipPointsForCollision;
			Projectile.FillWhipControlPoints(Projectile, points);

			foreach (var p in points)
			{
				Lighting.AddLight(p, 0.8f, 0.2f, 0.2f);
			}
		}

	}

	public class WhitePhaseWhipProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAWhip[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.DefaultToWhip();
			Projectile.WhipSettings.Segments = 45;
			Projectile.WhipSettings.RangeMultiplier = 1.09f;
            Projectile.ArmorPenetration = 9999;
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
			target.AddBuff(ModContent.BuffType<SimpleWhipDebuff08>(), 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.75f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> points = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, points);


			SimpleWhipDrawer3.DrawSegments(Projectile, points, Timer);

			return false;
		}

		public override void AI()
		{
			base.AI();

			List<Vector2> points = Projectile.WhipPointsForCollision;
			Projectile.FillWhipControlPoints(Projectile, points);

			foreach (var p in points)
			{
				Lighting.AddLight(p, 0.9f, 0.9f, 0.9f);
			}
		}

	}

	public class YellowPhaseWhipProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAWhip[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.DefaultToWhip();
			Projectile.WhipSettings.Segments = 45;
			Projectile.WhipSettings.RangeMultiplier = 1.09f;
            Projectile.ArmorPenetration = 9999;
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
			target.AddBuff(ModContent.BuffType<SimpleWhipDebuff08>(), 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.75f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> points = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, points);


			SimpleWhipDrawer3.DrawSegments(Projectile, points, Timer);

			return false;
		}

		public override void AI()
		{
			base.AI();

			List<Vector2> points = Projectile.WhipPointsForCollision;
			Projectile.FillWhipControlPoints(Projectile, points);

			foreach (var p in points)
			{
				Lighting.AddLight(p, 0.7f, 0.6f, 0.1f);
			}
		}

	}
}