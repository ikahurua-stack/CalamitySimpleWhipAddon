using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Rendering
{
    public enum ChargeTheme
    {
        None,
        massofWailing,
        chorusofExecration
    }

    public class ChargeParticle
    {
        public ChargeTheme Theme;

        public float Distance;

        public Vector2 Offset;

        public Vector2 Center;

        public Vector2 Velocity;

        public float Size;

        public int Life;

        public int MaxLife;

        public float Stick;
    }

    public class ChargeBody
    {
        public ChargeTheme Theme;

        public Vector2 Center;

        public float Size;

        public int Life;
    }

    public class TrailParticle
    {
        public ChargeTheme Theme;

        public Vector2 Center;
        public Vector2 Velocity;

        public float Size;

        public int Life;
        public int MaxLife;

        public float Rotation;
    }

    public class ExplosionParticle
    {
        public ChargeTheme Theme;

        public Vector2 Center;
        public Vector2 Velocity;

        public float Size;

        public int Life;
        public int MaxLife;

        public float Rotation;
    }

    public class ExplosionBody
    {
        public ChargeTheme Theme;

        public Vector2 Center;

        public float Size;

        public int Life;
    }

    public static class ChargeMetaballData
    {
        public static readonly List<ChargeParticle> Particles = new();

        public static readonly List<ChargeBody> Bodies = new();

        public static readonly List<TrailParticle> TrailParticles = new();

        public static readonly List<ExplosionParticle> ExplosionParticles = new();

        public static readonly List<ExplosionBody> ExplosionBodies = new();
    }

    public static class ChargeVisual
    {
        public static float DamageToMass(float damage)
        {
            return (float)Math.Pow(damage, 0.44f);
        }

        public static float MassToRadius(float mass)
        {
            float radius = 5f + mass * 2.45f;

            if (mass > 30f)
                radius -= (mass - 30f) * 0.5f;

            return MathHelper.Clamp(radius, 5f, 9000f);
        }

        public static float DamageToRadius(float damage)
        {
            return MassToRadius(DamageToMass(damage));
        }
    }

}
