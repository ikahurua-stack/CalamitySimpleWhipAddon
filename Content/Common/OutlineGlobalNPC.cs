using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader;
using System;
using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon;
using CalamitySimpleWhipAddon.Content.Systems;
using CalamitySimpleWhipAddon.Content.Particles;

namespace CalamitySimpleWhipAddon.Content.Common
{
    public class OutlineGlobalNPC : GlobalNPC
    {

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx19>()) || 
                npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx18>()) ||
                npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx20>()))
            {
                if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx19>()))
                {
                    drawColor = Color.GreenYellow;

                    Lighting.AddLight(npc.Center, 0.375f, 0.5f, 0.1f);
                }
                else if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx18>()))
                {
                    drawColor = Color.HotPink;

                    Lighting.AddLight(npc.Center, 0.7f, 0.3f, 0.2f);
                }
                else if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx20>()))
                {
                    drawColor = Color.Orange;

                    Lighting.AddLight(npc.Center, 0.6f, 0.35f, 0.1f);
                }
            }
        }


        public override void PostAI(NPC npc)
        {
            if (!npc.active)
                return;

            if (!npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx19>()) &&
                !npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx18>()) &&
                !npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx20>()))
                return;

            if (Main.rand.NextBool(5))
            {
                float radius =
                    MathF.Max(npc.width, npc.height) * 0.75f;

                float size =
                    MathHelper.Clamp(radius / 60f, 0f, 1f);

                float r =
                    radius * (float)Math.Sqrt(Main.rand.NextFloat());

                Vector2 start =
                    npc.Center +
                    Main.rand.NextVector2Circular(r, r);

                float t =
                    Vector2.Distance(start, npc.Center) / radius;

                float distance =
                    MathHelper.Lerp(radius * 1.0f,
                                    radius * 0.2f,
                                    t);

                Vector2 dir =
                    Main.rand.NextVector2Unit();

                Vector2 end =
                    start +
                    dir * distance;

                Vector2 offset = end - npc.Center;

                if (offset.Length() > radius)
                {
                    offset.Normalize();
                    end = npc.Center + offset * radius;
                }

                if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx19>()))
                {
                    ElectricArcSystem.Spawn(start, end, size, Color.GreenYellow, Color.LawnGreen, Color.White);
                }
                else if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx18>()))
                {
                    ElectricArcSystem.Spawn(start, end, size, Color.Red, Color.Pink, Color.HotPink);
                }
                else if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx20>()))
                {
                    ElectricArcSystem.Spawn(start, end, size, Color.Red, Color.Orange, Color.White);
                }


                int d = Dust.NewDust(
                    start,
                    1,
                    1,
                    DustID.PortalBoltTrail);

                Main.dust[d].velocity =
                    Main.rand.NextVector2Circular(2f, 2f);

                Main.dust[d].scale =
                    Main.rand.NextFloat(0.2f * size, 0.4f * size);

                Main.dust[d].fadeIn = 0.2f;

                if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx19>()))
                {
                    Main.dust[d].color =
                    Color.GreenYellow;
                }
                else if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx18>()))
                {
                    Main.dust[d].color =
                    Color.Pink;
                }
                else if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx20>()))
                {
                    Main.dust[d].color =
                    Color.Orange;
                }

                Main.dust[d].noGravity = false;

                int d2 = Dust.NewDust(
                    end,
                    1,
                    1,
                    DustID.PortalBoltTrail);

                Main.dust[d2].velocity =
                    Main.rand.NextVector2Circular(2f, 2f);

                Main.dust[d2].scale =
                    Main.rand.NextFloat(0.2f * size, 0.4f * size);

                Main.dust[d2].fadeIn = 0.2f;

                if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx19>()))
                {
                    Main.dust[d2].color =
                    Color.GreenYellow;
                }
                else if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx18>()))
                {
                    Main.dust[d2].color =
                    Color.Pink;
                }
                else if (npc.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx20>()))
                {
                    Main.dust[d2].color =
                    Color.Orange;
                }

                Main.dust[d2].noGravity = false;
            }
        }



    }
}