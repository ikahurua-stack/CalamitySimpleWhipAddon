using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent;
using System.Collections.Generic;
using System;
using System.IO;
using CalamityMod.NPCs;
using CalamitySimpleWhipAddon;
using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Systems;
using CalamitySimpleWhipAddon.Content.Items.Weapons;
using CalamitySimpleWhipAddon.Content.Projectiles;

namespace CalamitySimpleWhipAddon.Content.Systems
{
    public class MechanicalLightningSystem : ModSystem
    {
        public class LightningSegment
        {
            public List<Vector2> ThinPoints = new();

            public List<Vector2> ThinPoints2 = new();

            public Vector2 Direction;

            public int Timer;
        }

        public class ChainAttack
        {
            public List<NPC> Targets = new();

            public List<int> Damages = new();

            public int CurrentIndex;

            public int Delay;

            public int Owner;

            public Projectile SourceProjectile;
        }

        public static List<LightningSegment> ActiveLightnings = new();

        public static List<ChainAttack> ActiveChains = new();

        public static void StartChain(
            Projectile sourceProjectile,
            NPC firstTarget,
            int baseDamage)
        {
            ChainAttack chain = new();

            chain.Owner = sourceProjectile.owner;
            chain.SourceProjectile = sourceProjectile;

            float damageMultiplier = 1f;

            NPC current = firstTarget;

            chain.Targets.Add(current);
            chain.Damages.Add(baseDamage);

            while (true)
            {
                NPC nextTarget = null;
                float nearestDistance = 1000f;

                foreach (NPC npc in Main.npc)
                {
                    if (!npc.active)
                        continue;

                    if (npc.friendly)
                        continue;

                    if (npc.dontTakeDamage)
                        continue;

                    float dr =
                        npc.GetGlobalNPC<CalamityGlobalNPC>().DR;

                    if (dr >= 0.9f)
                        continue;

                    if (!npc.HasBuff(
                        ModContent.BuffType<SimpleWhipDebuffEx20>()))
                        continue;

                    if (chain.Targets.Contains(npc))
                        continue;

                    float distance =
                        Vector2.Distance(
                            current.Center,
                            npc.Center);

                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nextTarget = npc;
                    }
                }

                if (nextTarget == null)
                    break;

                current = nextTarget;

                chain.Targets.Add(current);

                damageMultiplier *= Main.rand.NextFloat(
                    0.75f,
                    0.85f);

                int damage =
                    Math.Max(
                        (int)(baseDamage * damageMultiplier),
                        1);

                if (damage < baseDamage * 0.05f)
                    break;

                chain.Damages.Add(damage);
            }

            ActiveChains.Add(chain);
        }

        public override void PostUpdateEverything()
        {
            for (int i = ActiveLightnings.Count - 1; i >= 0; i--)
            {
                ActiveLightnings[i].Timer--;

                if (ActiveLightnings[i].Timer <= 0)
                    ActiveLightnings.RemoveAt(i);
            }

            UpdateChains();
        }

        private void UpdateChains()
        {
            Vector2 start;
            Vector2 end;

            for (int i = ActiveChains.Count - 1; i >= 0; i--)
            {
                ChainAttack chain = ActiveChains[i];

                if (chain.Owner != Main.myPlayer)
                    continue;

                chain.Delay++;

                if (chain.Delay < 5)
                    continue;

                chain.Delay = 0;

                if (chain.CurrentIndex >= chain.Targets.Count)
                {
                    ActiveChains.RemoveAt(i);
                    continue;
                }

                NPC target =
                    chain.Targets[chain.CurrentIndex];

                if (target.active)
                {
                    Projectile.NewProjectile(
                        Projectile.GetSource_None(),
                        target.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<WulfrumLightning_Damage>(),
                        chain.Damages[chain.CurrentIndex],
                        0f,
                        chain.Owner,
                        target.whoAmI);


                    if (chain.CurrentIndex == 0)
                    {
                        start = target.Center;
                        end = target.Center;
                    }
                    else
                    {
                        start = chain.Targets[
                                chain.CurrentIndex - 1].Center;
                        end = target.Center;
                    }

                    if (Main.netMode != NetmodeID.SinglePlayer)
                    {
                        ModPacket packet = Mod.GetPacket();

                        packet.Write((byte)SimpleWhipPacketID.MechanicalLightningSystem1);

                        packet.WriteVector2(start);
                        packet.WriteVector2(end);

                        packet.Send();
                    }
                    else if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        AddLightning(start, end);
                    }
                }

                chain.CurrentIndex++;
            }
        }

        public static void AddLightning(Vector2 start, Vector2 end)
        {
            for (int i = 0; i < 12; i++)
            {
                Vector2 dir =
                    Main.rand.NextVector2Unit();

                int d = Dust.NewDust(
                    end,
                    2,
                    2,
                    DustID.PortalBoltTrail);

                Main.dust[d].velocity =
                    dir * Main.rand.NextFloat(1, 3);

                Main.dust[d].scale =
                        Main.rand.NextFloat(
                            0.3f,
                            0.5f);

                Main.dust[d].color =
                    Color.Orange;

                Main.dust[d].noGravity = false;
                Main.dust[d].fadeIn = 0.35f;
            }

            var lightning = new LightningSegment();

            GenerateLightningSegment(start, end, lightning.ThinPoints);
            GenerateLightningSegment2(start, end, lightning.ThinPoints2);

            lightning.Direction =
                (end - start).SafeNormalize(Vector2.UnitX);

            foreach (Vector2 point in lightning.ThinPoints)
            {
                if (Main.rand.NextBool(4))
                {
                    int dust = Dust.NewDust(
                        point - new Vector2(2),
                        4,
                        4,
                        DustID.PortalBoltTrail);

                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].color = Color.Orange;

                    Main.dust[dust].scale =
                        Main.rand.NextFloat(
                            0.3f,
                            0.5f);

                    Vector2 outward = -lightning.Direction;

                    Vector2 side =
                        outward.RotatedBy(MathHelper.PiOver2);

                    Main.dust[dust].velocity =
                        outward * Main.rand.NextFloat(3f, 5f)
                        + side * Main.rand.NextFloat(-2f, 2f);

                    Main.dust[dust].fadeIn = 0.5f;
                }
            }

            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(
                    end,
                    4,
                    4,
                    DustID.PortalBoltTrail);

                Main.dust[dust].noGravity = false;
                Main.dust[dust].color = Color.Orange;

                Main.dust[dust].scale =
                    Main.rand.NextFloat(0.3f, 0.5f);

                float angle =
                    Main.rand.NextFloat(
                        -MathHelper.ToRadians(60f),
                        MathHelper.ToRadians(60f));

                Main.dust[dust].velocity =
                    (-Vector2.UnitY)
                    .RotatedBy(angle)
                    * Main.rand.NextFloat(3f, 5f);

                Main.dust[dust].fadeIn = 0.5f;
            }

            SoundStyle fire = new("CalamityMod/Sounds/Item/ArcFlash");
            SoundEngine.PlaySound(fire with { Volume = 0.15f, Pitch = Main.rand.NextFloat(0.1f, 0.4f) }, end);

            lightning.Timer = 5;

            ActiveLightnings.Add(lightning);
        }

        public static void GenerateLightningSegment(
            Vector2 beamStart,
            Vector2 beamEnd,
            List<Vector2> points)
        {
            points.Clear();

            points.Add(beamStart);

            Vector2 dir = beamEnd - beamStart;

            Vector2 normal =
                dir.SafeNormalize(Vector2.UnitX)
                .RotatedBy(MathHelper.PiOver2);

            float distance = Vector2.Distance(
                beamStart,
                beamEnd);

            int segments =
                (int)(distance / 28f)
                + Main.rand.Next(-2, 3);

            segments = Utils.Clamp(
                segments,
                5,
                75);

            for (int i = 1; i < segments; i++)
            {
                float t = i / (float)segments;

                Vector2 pos =
                    Vector2.Lerp(
                        beamStart,
                        beamEnd,
                        t);

                float maxOffset =
                    distance * 0.08f;

                maxOffset =
                    MathHelper.Clamp(
                        maxOffset,
                        5f,
                        13f);

                float offset =
                    maxOffset *
                    (1f - Math.Abs(t - 0.5f));

                pos += normal *
                    Main.rand.NextFloat(
                        -offset,
                        offset);

                points.Add(pos);
            }

            points.Add(beamEnd);
        }

        public static void GenerateLightningSegment2(
            Vector2 beamStart,
            Vector2 beamEnd,
            List<Vector2> points)
        {
            points.Clear();

            points.Add(beamStart);

            Vector2 dir = beamEnd - beamStart;

            Vector2 normal =
                dir.SafeNormalize(Vector2.UnitX)
                .RotatedBy(MathHelper.PiOver2);

            float distance = Vector2.Distance(
                beamStart,
                beamEnd);

            int segments =
                (int)(distance / 28f)
                + Main.rand.Next(-2, 3);

            segments = Utils.Clamp(
                segments,
                5,
                75);

            for (int i = 1; i < segments; i++)
            {
                float t = i / (float)segments;

                Vector2 pos =
                    Vector2.Lerp(
                        beamStart,
                        beamEnd,
                        t);

                float maxOffset =
                    distance * 0.08f;

                maxOffset =
                    MathHelper.Clamp(
                        maxOffset,
                        10f,
                        25f);

                float offset =
                    maxOffset *
                    (1f - Math.Abs(t - 0.5f));

                pos += normal *
                    Main.rand.NextFloat(
                        -offset,
                        offset);

                points.Add(pos);
            }

            points.Add(beamEnd);
        }

        public override void PostDrawTiles()
        {
            if (ActiveLightnings == null || ActiveLightnings.Count == 0)
                return;

            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                Main.GameViewMatrix.TransformationMatrix);

            DrawLightnings();

            Main.spriteBatch.End();
        }

        

        private static void DrawSegment(
            Texture2D pixel,
            Vector2 start,
            Vector2 end,
            Color color,
            float width)
        {
            Vector2 diff = end - start;

            Main.EntitySpriteDraw(
                pixel,
                start - Main.screenPosition,
                new Rectangle(0, 0, 1, 1),
                color,
                diff.ToRotation(),
                new Vector2(0f, 0.5f),
                new Vector2(diff.Length(), width),
                SpriteEffects.None,
                0);
        }

        public static void DrawLightnings()
        {
            Texture2D pixel = TextureAssets.MagicPixel.Value;

            foreach (var lightning in ActiveLightnings)
            {
                float alpha = lightning.Timer / 5f;

                float pulse =
                    0.9f +
                    0.25f *
                    MathF.Sin(Main.GlobalTimeWrappedHourly * 80f);

                float pulse2 =
                    0.85f +
                    0.35f *
                    MathF.Sin(Main.GlobalTimeWrappedHourly * 110f + 1.7f);

                var thin1 = lightning.ThinPoints;
                var thin2 = lightning.ThinPoints2;


                float thickScale =
                    (alpha > 0.5f ? 1f : alpha * 2f) * pulse;

                float thinScale =
                    (alpha > 0.5f ? 1f : alpha * 2f) * pulse2;

                

                for (int i = 0; i < thin1.Count - 1; i++)
                {
                    Vector2 start = thin1[i];
                    Vector2 end = thin1[i + 1];

                    DrawSegment(pixel, start, end,
                        Color.Red * 0.2f * alpha,
                        17f * thinScale);

                    DrawSegment(pixel, start, end,
                        Color.Orange * 0.6f * alpha,
                        8f * thinScale);
                }

                for (int i = 0; i < thin2.Count - 1; i++)
                {
                    Vector2 start = thin2[i];
                    Vector2 end = thin2[i + 1];

                    DrawSegment(pixel, start, end,
                        Color.Red * 0.2f * alpha,
                        14f * thinScale);

                    DrawSegment(pixel, start, end,
                        Color.Orange * 0.6f * alpha,
                        6f * thinScale);
                }


                for (int i = 0; i < thin1.Count - 1; i++)
                {
                    Vector2 start = thin1[i];
                    Vector2 end = thin1[i + 1];

                    float whitePulse =
                        0.85f +
                        0.2f *
                        MathF.Sin(Main.GlobalTimeWrappedHourly * 180f + 0.8f);

                    DrawSegment(pixel, start, end,
                        Color.White * alpha,
                        6f * thinScale * whitePulse);
                }

                for (int i = 0; i < thin2.Count - 1; i++)
                {
                    Vector2 start = thin2[i];
                    Vector2 end = thin2[i + 1];

                    float whitePulse =
                        0.85f +
                        0.2f *
                        MathF.Sin(Main.GlobalTimeWrappedHourly * 180f + 0.8f);

                    DrawSegment(pixel, start, end,
                        Color.White * alpha,
                        4f * thinScale * whitePulse);
                }

            }
        }
    }
}