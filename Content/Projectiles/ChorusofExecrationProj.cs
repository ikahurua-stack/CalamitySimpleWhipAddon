using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using CalamitySimpleWhipAddon.Content.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Graphics.Metaballs;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{


    public class ChorusofExecrationProj : ModProjectile
    {
        private CurveData cachedCurve;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 40;
            Projectile.WhipSettings.RangeMultiplier = InfernalEclipseCompatibility.IsEnabled ? 3.5f : 5.5f;
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
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuffEx22>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.98f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            List<Vector2> points = new();
            Projectile.FillWhipControlPoints(Projectile, points);

            // ★毎回生成しない
            cachedCurve = CurveBuilder.Build(points);

            WhipMetaballRenderer.Update(Projectile, cachedCurve);

            if (points.Count < 2)
                return;

            Vector2 tip = points[points.Count - 1];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 pos = points[i];
                Lighting.AddLight(pos, 0.4f, 0.03f, 0.03f);
            }
            Lighting.AddLight(tip, 0.6f, 0.05f, 0.05f);

        }

    }
}
