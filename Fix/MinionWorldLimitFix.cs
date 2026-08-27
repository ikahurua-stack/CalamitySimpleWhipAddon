using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

public class MinionWorldLimitFix : GlobalProjectile
{
	public override bool InstancePerEntity => true;

	public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
	{
		return entity.minion;
	}

	public override void PostAI(Projectile projectile)
	{
		float margin = 250f;

		float left = margin;
		float right = Main.maxTilesX * 16 - margin;
		float top = margin;
		float bottom = Main.maxTilesY * 16 - margin;

		Vector2 nextCenter = projectile.Center + projectile.velocity;

		float halfW = projectile.width * 0.5f;
		float halfH = projectile.height * 0.5f;

		float nextLeft = nextCenter.X - halfW;
		float nextRight = nextCenter.X + halfW;
		float nextTop = nextCenter.Y - halfH;
		float nextBottom = nextCenter.Y + halfH;

		if (nextLeft < left)
		{
			projectile.Center = new Vector2(left + halfW, projectile.Center.Y);
			if (projectile.velocity.X < 0)
				projectile.velocity.X = 0;
		}

		if (nextRight > right)
		{
			projectile.Center = new Vector2(right - halfW, projectile.Center.Y);
			if (projectile.velocity.X > 0)
				projectile.velocity.X = 0;
		}

		if (nextTop < top)
		{
			projectile.Center = new Vector2(projectile.Center.X, top + halfH);
			if (projectile.velocity.Y < 0)
				projectile.velocity.Y = 0;
		}

		if (nextBottom > bottom)
		{
			projectile.Center = new Vector2(projectile.Center.X, bottom - halfH);
			if (projectile.velocity.Y > 0)
				projectile.velocity.Y = 0;
		}
	}
}