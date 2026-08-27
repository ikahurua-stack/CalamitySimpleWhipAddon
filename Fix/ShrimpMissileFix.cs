using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using CalamityMod.Projectiles.DraedonsArsenal;


public class ShrimpMissileFix : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    private int lifeTimer;

    public override void PostAI(Projectile projectile)
    {
        if (projectile.ModProjectile?.GetType().Name != "ShrimpPlasmaMissile")
            return;

        lifeTimer++;

        if (lifeTimer > 800)
            projectile.Kill();
    }
}