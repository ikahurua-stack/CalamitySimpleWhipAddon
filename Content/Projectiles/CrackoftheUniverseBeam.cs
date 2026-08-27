using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Systems;
using CalamityMod.Particles;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class CrackoftheUniverseBeam : ModProjectile
    {

        public bool canDamage => laserFX >= 1f;

        public const float laserLength = 4000f;

        public float laserFX = 0;

        public Color drawColor = Color.Magenta;

        public float sine = 0;

        private int timer;

        public float laserRot = 0;

        private Vector2 beamStart;

        private Vector2 beamDir;

        private bool initialized;

        private const float lifeTime = 40f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 10000;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;

            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.penetrate = -1;

            Projectile.timeLeft = 6000;

            Projectile.scale = 2f;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.ArmorPenetration = 9999;
        }

        public override void AI()
        {
            if (laserFX > 0)
                laserFX = MathHelper.Lerp(
                    laserFX,
                    0,
                    timer > 15 ? 0.07f : 0.01f);

            sine = (float)Math.Sin(timer * 4f / MathHelper.Pi);

            if (!initialized)
            {
                beamStart = Projectile.Center;

                beamDir = new Vector2(
                    Projectile.ai[0],
                    Projectile.ai[1]);

                beamDir = beamDir.SafeNormalize(Vector2.UnitX);

                laserRot = beamDir.ToRotation();

                Projectile.velocity = Vector2.Zero;

                laserFX = 2.5f;

                initialized = true;
            }

            timer++;


            if (timer == 1)
            {
                Vector2 start = beamStart + beamDir * 20f;

                Particle bolt2 = new CustomPulse(
                    start,
                    beamDir * 0.2f,
                    Main.rand.NextBool() ? Color.Fuchsia : Color.Cyan,
                    "CalamityMod/Particles/BloomRing",
                    new Vector2(0.5f, 1f),
                    beamDir.ToRotation(),
                    0f,
                    0.6f,
                    23);

                GeneralParticleHandler.SpawnParticle(bolt2);

                Particle spark = new CustomSpark(
                    start,
                    beamDir * 0.2f,
                    "CalamityMod/Particles/BloomCircle",
                    false,
                    28,
                    0.6f,
                    Main.rand.NextBool() ? Color.Fuchsia : Color.Cyan,
                    new Vector2(1f, 0.5f),
                    true,
                    true,
                    0,
                    false,
                    false);

                GeneralParticleHandler.SpawnParticle(spark);

                Particle spark2 = new CustomSpark(
                    start,
                    beamDir * 0.2f,
                    "CalamitySimpleWhipAddon/Content/Textures/Particles/BloomCircleBlack",
                    false,
                    28,
                    0.45f,
                    Main.rand.NextBool() ? Color.Fuchsia : Color.Cyan,
                    new Vector2(1f, 0.5f),
                    false,
                    true,
                    0,
                    false,
                    false);

                GeneralParticleHandler.SpawnParticle(spark2);

                CalamityGemBurstFX.Play(
                    start,
                    Color.Magenta,
                    Color.Cyan,
                    Color.Black,
                    DustID.GemAmethyst);

                for (int i = 0; i < 10; i++)
                {
                    float angleOffset =
                        Main.rand.NextFloat(-0.4f, 0.4f);

                    Vector2 velocity =
                        beamDir
                        .RotatedBy(angleOffset)
                        * Main.rand.NextFloat(10f, 18f);

                    Particle spark3 = new CustomSpark(
                        start + Main.rand.NextVector2Circular(6f, 6f),
                        velocity,
                        "CalamityMod/Particles/ProvidenceMarkParticle",
                        false,
                        17,
                        Main.rand.NextFloat(0.8f, 0.95f),
                        Main.rand.NextBool() ? Color.Fuchsia : Color.Cyan,
                        new Vector2(1.7f, 0.5f),
                        true,
                        false,
                        0,
                        false,
                        false,
                        Main.rand.NextFloat(0.4f, 0.5f));

                    GeneralParticleHandler.SpawnParticle(spark3);
                }

                SoundStyle attack =
                    new("CalamityMod/Sounds/Custom/DoGLaserWallLightAttack");

                if (Projectile.scale > 3)
                {
                    attack =
                        new("CalamityMod/Sounds/Custom/DoGLaserWallBigAttack");
                }

                SoundEngine.PlaySound(
                    attack with
                    {
                        Volume = 0.4f,
                        Pitch = 0,
                        MaxInstances = -1
                    },
                    beamStart);

                laserFX = 2.5f;

            }

            if (timer >= 50)
            {
                Projectile.Kill();
                return;
            }

        }

        public override bool? CanHitNPC(NPC target)
        {
            if (canDamage)
                return null;

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!canDamage)
                return false;

            float point = 0f;

            Vector2 start = beamStart;
            Vector2 end = beamStart + beamDir * laserLength * 2f;

            bool hitCheck =
                Collision.CheckAABBvLineCollision(
                    targetHitbox.TopLeft(),
                    targetHitbox.Size(),
                    start,
                    end,
                    30f * Projectile.scale,
                    ref point);

            return hitCheck;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (laserFX == 0)
                return false;

            Texture2D beam =
                ModContent.Request<Texture2D>(
                    "CalamitySimpleWhipAddon/Content/Projectiles/GlowBlade2").Value;

            Texture2D bBeam =
                ModContent.Request<Texture2D>(
                    "CalamitySimpleWhipAddon/Content/Projectiles/GlowBladeNoBloom2").Value;

            float opacity =
                0.65f *
                (float)Math.Pow(Math.Min(laserFX, 1), 2);


            Vector2 start = beamStart + beamDir * 20f;
            Vector2 dir = beamDir;

            for (int t = 0; t < 8; t++)
            {
                    bool black = (t > 0);

                    Texture2D usedTex =
                        black ? bBeam : beam;

                float timeFade =
                Utils.GetLerpValue(
                    0,
                    lifeTime,
                    timer,
                    true);

                float progress =
                    Utils.GetLerpValue(
                        0f,
                        lifeTime,
                        timer,
                        true);

                // 一気に太くなる
                float grow =
                    MathF.Pow(progress, 0.18f);

                // 一気に消える
                float shrink =
                    MathF.Pow(1f - progress, 3.2f);

                // 両方合成
                float thicknessFade =
                    MathF.Min(grow, shrink);

                float maxThickness = 0.05f;

                    float beamThickness =
                        maxThickness *
                        thicknessFade *
                        (black ? (0.8f - 0.15f * t) : 1f) *
                        (laserFX <= 1
                            ? MathF.Pow(Math.Min(laserFX, 1), 2)
                            : laserFX) *
                        Utils.Remap(
                            sine,
                            -1,
                            1,
                            0.8f,
                            1.1f);

                    float rot =
                        dir.ToRotation() + MathHelper.PiOver2;

                float colorProgress =
                    MathF.Pow(thicknessFade, 0.5f);

                Color beamColor =
                    Color.Lerp(
                        Color.Cyan,
                        Color.Magenta,
                        colorProgress);

                beamColor.A = 0;

                Main.EntitySpriteDraw(
                        usedTex,
                        start - Main.screenPosition,
                        null,
                        (black
                            ? Color.Black * opacity
                            : beamColor * opacity)
                        * (black ? (0.2f + 0.15f * t) : 1f),
                        rot,
                        new Vector2(
                            beam.Width / 2,
                            beam.Height),
                        new Vector2(
                            beamThickness * Projectile.scale,
                            laserLength / 975f *
                            (usedTex == beam ? 1f : 0.8277f)),
                        SpriteEffects.None);
            }
            

            return false;
        }
    }
}