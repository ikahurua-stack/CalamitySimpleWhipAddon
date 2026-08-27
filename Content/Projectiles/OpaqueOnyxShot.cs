using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Drawing;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class OpaqueOnyxShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }


        public override void SetDefaults()
        {
            Projectile.netImportant = true;

            Projectile.width = 40;
            Projectile.height = 40;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = 6;
            Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 700;

            Projectile.timeLeft = 600;

            Projectile.extraUpdates = 4;

            Projectile.DamageType = DamageClass.Summon;

            Projectile.ArmorPenetration = 9999;
        }


        public override void AI()
        {
            NPC target = null;

            int who = (int)Projectile.ai[0];

            if (Projectile.ai[1] < 50)
            {

                if (who >= 0 && who < Main.maxNPCs)
                {
                    NPC npc = Main.npc[who];

                    if (npc.active && npc.CanBeChasedBy(this))
                        target = npc;
                    Projectile.netUpdate = true;
                }

                if (target != null)
                {
                    float current = Projectile.velocity.ToRotation();
                    float desired = (target.Center - Projectile.Center).ToRotation();

                    float newRot = current.AngleTowards(desired, 0.04f);

                    Projectile.velocity = newRot.ToRotationVector2() * Projectile.velocity.Length();
                    Projectile.netUpdate = true;
                }
            }

            Projectile.ai[1]++;

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.ai[1] == 50)
            {
                Projectile.velocity *= 5f;
                Projectile.netUpdate = true;
            }

            for (int i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.PurpleTorch);

                d.noGravity = true;
                d.scale = 1.2f;
                d.velocity *= 0.2f;
            }
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.NightsEdge, new ParticleOrchestraSettings()
            {
                PositionInWorld = target.Center,
                MovementVector = Vector2.One
            });

            SoundEngine.PlaySound(SoundID.Item105, Projectile.Center);

            Projectile.netUpdate = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Type].Value;

            Vector2 origin = tex.Size() * 0.5f;

            float progress = MathHelper.Clamp(Projectile.ai[1] / 50f, 0f, 1f);

            float xScale = MathHelper.Lerp(1.3f, 2.0f, progress);
            float yScale = MathHelper.Lerp(1f, 0.3f, progress);

            Vector2 scale = new Vector2(xScale, yScale);

            // トレイル
            for (int i = Projectile.oldPos.Length - 1; i >= 0; i--)
            {
                Vector2 drawPos =
                    Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;

                float alpha = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;

                Main.EntitySpriteDraw(
                    tex,
                    drawPos,
                    null,
                    new Color(255, 255, 255) * alpha * 0.5f,
                    Projectile.rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0);
            }

            // 本体
            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                new Color(255, 255, 255),
                Projectile.rotation,
                origin,
                scale,
                SpriteEffects.None,
                0);

            return false;
        }

    }

}