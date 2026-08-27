using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamityMod.Particles;
using Terraria.DataStructures;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class WhipShieldChargeOrbProj : ModProjectile
    {
        private Rectangle frame;
        private Vector2 initialVelocity;
        private int timer = 0;

        public bool IsAttackOrb => Projectile.ai[0] == 1f;
        private NPC target;

        public int Tier => (int)Projectile.ai[1];

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ArmorPenetration = 9999;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.DamageType = DamageClass.Summon;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Rectangle[] frames =
            {
                new Rectangle(8, 0, 6, 6),
                new Rectangle(6, 8, 10, 6),
                new Rectangle(4, 16, 14, 8),
                new Rectangle(2, 26, 18, 10),
                new Rectangle(2, 38, 18, 8),
                new Rectangle(6, 48, 12, 12),
            };

            frame = frames[Main.rand.Next(frames.Length)];

            initialVelocity = Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(2f, 6f);
            Projectile.velocity = initialVelocity;

            for (int i = 0; i < 6; i++)
            {
                Vector2 vel = Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(2f, 6f);
                Color color = IsAttackOrb ? Color.Red : Color.Cyan;

                var p = new TechyHoloysquareParticle(
                    Projectile.Center,
                    vel,
                    Main.rand.NextFloat(1.2f, 2f),
                    color,
                    25
                );

                GeneralParticleHandler.SpawnParticle(p);
            }
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            timer++;


            if (Main.myPlayer == Projectile.owner)
            {
                if (!IsAttackOrb)
                {
                    if (timer < 30)
                    {
                        Projectile.velocity *= 0.92f;
                    }
                    else
                    {
                        Vector2 toPlayer = player.Center - Projectile.Center;
                        float speed = 24f;

                        Projectile.velocity = Vector2.Lerp(
                            Projectile.velocity,
                            toPlayer.SafeNormalize(Vector2.Zero) * speed,
                            0.08f
                        );
                    }

                    if (Vector2.Distance(player.Center, Projectile.Center) < 20f)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Vector2 vel = Main.rand.NextVector2Circular(4f, 4f);

                            var p = new TechyHoloysquareParticle(
                                player.Center,
                                vel,
                                1f,
                                Color.Cyan,
                                20
                            );

                            GeneralParticleHandler.SpawnParticle(p);
                        }

                        SoundEngine.PlaySound(
                            SoundID.DD2_WitherBeastCrystalImpact with
                            {
                                Volume = 0.7f,
                                Pitch = 0.2f,
                                PitchVariance = 0.25f
                            },
                            Projectile.Center
                        );

                        var modPlayer = player.GetModPlayer<WhipShieldPlayer>();

                        modPlayer.SetShieldTier(Tier);
                        modPlayer.AddCharge(1);
                        Projectile.Kill();
                        return;
                    }
                }
                else
                {
                    if (timer < 30)
                    {
                        Projectile.velocity *= 0.92f;
                    }
                    else
                    {
                        if (target == null || !target.active || target.friendly)
                        {
                            float dist = 600f;
                            foreach (NPC npc in Main.ActiveNPCs)
                            {
                                float d = Vector2.Distance(npc.Center, Projectile.Center);
                                if (d < dist && !npc.friendly && npc.CanBeChasedBy())
                                {
                                    dist = d;
                                    target = npc;
                                }
                            }
                        }

                        if (target != null)
                        {
                            Vector2 toTarget = target.Center - Projectile.Center;
                            float speed = 24f;

                            Projectile.velocity = Vector2.Lerp(
                                Projectile.velocity,
                                toTarget.SafeNormalize(Vector2.Zero) * speed,
                                0.1f
                            );
                        }
                    }
                }
                if (timer % 5 == 0) Projectile.netUpdate = true;
            }


            // 発光
            Color glowColor = IsAttackOrb ? Color.Red : Color.Cyan;
            Vector3 light = glowColor.ToVector3() * 0.6f;
            Lighting.AddLight(Projectile.Center, light);

        }

        public override bool? CanHitNPC(NPC target)
        {
            if (IsAttackOrb && timer < 30)
                return false;

            return null;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(
                SoundID.DD2_WitherBeastCrystalImpact with
                {
                    Volume = 0.7f,
                    Pitch = 0.2f,
                    PitchVariance = 0.25f
                },
                Projectile.Center
            );

            for (int i = 0; i < 2; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(4f, 4f);

                var p = new TechyHoloysquareParticle(
                    Projectile.Center,
                    vel,
                    1f,
                    Color.Red,
                    20
                );

                GeneralParticleHandler.SpawnParticle(p);
            }

            Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(
                "CalamityMod/Particles/TechyHolosquare"
            ).Value;

            Color baseColor = IsAttackOrb ? Color.Red : Color.Cyan;

            // ===== Additive開始 =====
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.Additive,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix
            );

            Vector2 pos = Projectile.Center - Main.screenPosition;
            float rot = Projectile.velocity.ToRotation();
            Vector2 origin = frame.Size() / 2f;

            bool glowing = timer >= 28;

            if (glowing)
            {
                // ===== 外側グロー =====
                Main.spriteBatch.Draw(
                    tex,
                    pos,
                    frame,
                    baseColor * 0.4f,
                    rot,
                    origin,
                    1.6f,
                    SpriteEffects.None,
                    0f
                );

                // ===== 中間グロー =====
                Main.spriteBatch.Draw(
                    tex,
                    pos,
                    frame,
                    baseColor * 0.45f,
                    rot,
                    origin,
                    1.3f,
                    SpriteEffects.None,
                    0f
                );
            }

            // ===== 本体 =====
            Main.spriteBatch.Draw(
                tex,
                pos,
                frame,
                baseColor * 0.8f,
                rot,
                origin,
                0.75f,
                SpriteEffects.None,
                0f
            );

            // ===== 元に戻す =====
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix
            );

            return false;
        }

    }
}
