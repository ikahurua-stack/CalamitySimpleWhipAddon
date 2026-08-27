using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using CalamitySimpleWhipAddon.Content.Common.GlobalNPCs;
using CalamitySimpleWhipAddon.Content.Systems;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{


    public class RubellusProj : ModProjectile
    {
        private Vector2 oldTip;
        private bool tipInitialized = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 65;
            Projectile.WhipSettings.RangeMultiplier = 2.6f;
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
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuffEx18>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.85f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);

            SimpleWhipDrawer17.DrawSegments(Projectile, points, Timer);

            return false;
        }

        public override void AI()
        {
            base.AI();

            Player owner = Main.player[Projectile.owner];

            // ===== ムチの先端取得 =====
            List<Vector2> points = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, points);

            if (points.Count < 2)
                return;

            Vector2 tip = points[points.Count - 1];
            Vector2 prev = points[points.Count - 2];

            // ===== 初期化 =====
            if (!tipInitialized)
            {
                oldTip = tip;
                tipInitialized = true;
            }

            // ===== 先端方向 =====
            Vector2 dir = tip - prev;
            if (dir != Vector2.Zero)
                dir.Normalize();
            else
                dir = Vector2.UnitX;

            // ===== プレイヤー付近では出さない =====
            float distanceToPlayer = Vector2.Distance(tip, owner.MountedCenter);
            if (distanceToPlayer < 50f)
            {
                oldTip = tip;
                return;
            }


            // ===== プレイヤー距離 =====
            float dist = Vector2.Distance(tip, owner.MountedCenter);

            // 調整用距離
            float minDist = 50f;
            float maxDist = 350f;

            // 0～1に正規化
            float tDist = Utils.GetLerpValue(minDist, maxDist, dist, true);

            // ===== フレーム補間 =====
            float moveDist = Vector2.Distance(oldTip, tip);

            Vector2 start;
            Vector2 end;

            start = oldTip;
            end = tip;


            // ★サーバーのみ送信
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = Mod.GetPacket();

                packet.Write((byte)SimpleWhipPacketID.LightningSystem1);
                packet.Write(Projectile.whoAmI);
                packet.WriteVector2(start);
                packet.WriteVector2(end);
                packet.Send();
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
            {
                LightningSystem.AddLightning(start, end);
            }

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 pos = points[i];
                Lighting.AddLight(pos, 0.7f, 0.2f, 0.2f);
            }
            Lighting.AddLight(tip, 1f, 0.3f, 0.3f);

            // ===== 次フレーム用 =====
            oldTip = tip;
        }

        

    }
}