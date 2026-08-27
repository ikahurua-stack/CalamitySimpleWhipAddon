using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using CalamitySimpleWhipAddon.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using System;
using CalamityMod.Graphics.Primitives;
using static CalamityMod.CalamityUtils;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Typeless;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class MilkywayProj : ModProjectile
    {
        private readonly List<Vector2> tipHistory = new();
        private const int MaxTrailPoints = 20;

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

        private bool particleInitDone = false;
        private float particleTimer;
        private bool clearedOnSpawn = false;
        public Color InnerColor = new Color(150, 255, 255);

        private Vector2? lastTipPos = null; // フレーム補間用

        private static readonly Color[] LuminiteColors =
        {
            new Color(120, 200, 255),
            new Color(120, 200, 200)
        };

        private float WhipTrailWidth(float t, Vector2 position)
        {
            float tipBias = MathF.Pow(t, 1.5f);
            return Projectile.scale * MathHelper.Lerp(1.5f, 25f, tipBias);
        }

        private Color WhipTrailBloomColor(float t, Vector2 position)
        {
            Color baseColor = GetLuminiteGradient(t, position);
            return baseColor * Projectile.Opacity * 0.80f;
        }

        private float WhipTrailBloomWidth(float t, Vector2 position) => WhipTrailWidth(t, position) * 2f;

        private Color GetLuminiteGradient(float t, Vector2 position)
        {
            t = MathHelper.Clamp(t, 0f, 1f);
            float scaled = t * (LuminiteColors.Length - 1);
            int index = (int)scaled;
            int nextIndex = Math.Min(index + 1, LuminiteColors.Length - 1);
            float lerp = scaled - index;
            return Color.Lerp(LuminiteColors[index], LuminiteColors[nextIndex], lerp);
        }

        private Color WhipTrailColor(float t, Vector2 position)
        {
            float historyFactor = MathHelper.Clamp(tipHistory.Count / (float)MaxTrailPoints, 0f, 1f);
            float tipEmphasis = MathHelper.Lerp(0.1f, 1f, t);
            Color baseColor = GetLuminiteGradient(t, position);
            return baseColor * Projectile.Opacity * historyFactor * tipEmphasis * 0.7f;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;

            ProjectileID.Sets.TrailCacheLength[Type] = 0; 

        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 62;
            Projectile.WhipSettings.RangeMultiplier = 5.2f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuffC100x3>(), 240);
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuffT20St>(), 240);
            StackTagNPC2 tagNPC = target.GetGlobalNPC<StackTagNPC2>();
            tagNPC.AddStack(
                target,
                240,
                Projectile.owner,
                ModContent.ProjectileType<MilkywayMoth>(),
                ModContent.BuffType<SimpleWhipDebuffT20St>()
            );
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.9f);
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            float swingProgress = Projectile.ai[0] / swingTime;
            List<Vector2> controlPoints = new();
            Projectile.FillWhipControlPoints(Projectile, controlPoints);

            if (controlPoints.Count > 0)
            {
                Vector2 tip = controlPoints[^1];

                bool canTrail = swingProgress > 0.5f;

                if (!canTrail)
                    tipHistory.Clear();
                else
                {
                    if (tipHistory.Count == 0 || Vector2.Distance(tipHistory[^1], tip) > 4f)
                    {
                        tipHistory.Add(tip);
                        if (tipHistory.Count > MaxTrailPoints)
                            tipHistory.RemoveAt(0);
                    }
                }
            }

            if (Utils.GetLerpValue(0.1f, 0.7f, swingProgress, true) *
                Utils.GetLerpValue(0.9f, 0.7f, swingProgress, true) > 0.5f &&
                !Main.rand.NextBool(3))
            {
                Vector2 tip = controlPoints[^1];

                for (int i = 0; i < 2; i++)
                {
                    int dustIndex = Dust.NewDust(tip - new Vector2(4, 4), 8, 8, DustID.RainbowTorch,
                        Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f),
                        100, Color.LightPink, Main.rand.NextFloat(0.6f, 1.2f));
                    Main.dust[dustIndex].noGravity = true;
                }

                for (int i = 0; i < 2; i++)
                {
                    int dustIndex = Dust.NewDust(tip - new Vector2(4, 4), 8, 8, DustID.PortalBolt,
                        Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f),
                        100, Color.LightPink, Main.rand.NextFloat(0.6f, 1.2f));
                    Main.dust[dustIndex].noGravity = true;
                }

                foreach (Vector2 p in controlPoints)
                    Lighting.AddLight(p, 0.7f, 0.3f, 0.4f);
            }

            // --- パーティクル統合（フレーム補間版） ---
            if (!clearedOnSpawn && Projectile.owner == Main.myPlayer)
            {
                CustomParticleHandler.ClearAll();
                clearedOnSpawn = true;
            }

            Lighting.AddLight(Projectile.Center, InnerColor.ToVector3() * 0.2f);

            if (!particleInitDone) particleInitDone = true;
            particleTimer++;

            Projectile.FillWhipControlPoints(Projectile, controlPoints);

            if (controlPoints.Count >= 2)
            {
                Vector2 tip = controlPoints[^1];
                Vector2 prev = controlPoints[^2];

                if (Vector2.Distance(tip, owner.Center) < 150f)
                    return;

                // --- フレーム補間方式 ---
                if (lastTipPos.HasValue)
                {
                    Vector2 delta = tip - lastTipPos.Value;
                    float distance = delta.Length();
                    if (distance > 0.5f)
                    {
                        float spacing = 40f;
                        int count = (int)(distance / spacing);

                        for (int i = 0; i <= count; i++)
                        {
                            float t = i / (float)(count + 1);
                            Vector2 spawnPos = Vector2.Lerp(lastTipPos.Value, tip, t);
                            SpawnParticle(spawnPos);
                        }
                    }
                }
                lastTipPos = tip;

                // 追加ランダムパーティクル
                if (Vector2.Distance(tip, owner.Center) > 150f)
                {
                    if (Main.rand.NextBool(1)) SpawnParticle(tip);
                    if (Main.rand.NextBool(3)) SpawnParticle(tip);
                }
            }

            CustomParticleHandler.UpdateAll();
        }

        private void SpawnParticle(Vector2 tip)
        {
            Vector2 offset = new Vector2(Main.rand.NextFloat(-15f, 15f), Main.rand.NextFloat(-15f, 15f));
            Texture2D tex = ModContent.Request<Texture2D>("CalamitySimpleWhipAddon/Content/Textures/Particles/MilkywayFire").Value;

            float maxScale = Main.rand.NextFloat(0.4f, 0.8f);
            float lifeTime = Main.rand.NextFloat(20f, 40f);

            CustomParticle p = new CustomParticle(
                tip + offset,
                offset * 0.1f + new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)),
                maxScale,
                lifeTime,
                tex,
                Color.White
            );

            // 回転追加
            p.Rotation = Main.rand.NextFloat(0f, MathHelper.TwoPi);
            p.AngularVelocity = Main.rand.NextFloat(-0.1f, 0.1f);

            CustomParticleHandler.Spawn(p);
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
                CustomParticleHandler.ClearAll();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> points = new();
            Projectile.FillWhipControlPoints(Projectile, points);

            if (points.Count >= 2)
            {
                var shader = GameShaders.Misc["CalamityMod:Flame"];
                shader.SetShaderTexture(TextureAssets.Extra[197]);

                if (tipHistory.Count >= 2)
                {
                    PrimitiveRenderer.RenderTrail(
                        tipHistory,
                        new PrimitiveSettings(
                            WhipTrailBloomWidth,
                            WhipTrailBloomColor,
                            (t, pos) => Vector2.Zero,
                            shader: shader
                        ),
                        24
                    );

                    PrimitiveRenderer.RenderTrail(
                        tipHistory,
                        new PrimitiveSettings(
                            WhipTrailWidth,
                            WhipTrailColor,
                            (t, pos) => Vector2.Zero,
                            shader: shader
                        ),
                        24
                    );
                }

                RapierWhipDrawer4.DrawSegments(Projectile, points, Timer);
            }

            return false;
        }
    }
}
