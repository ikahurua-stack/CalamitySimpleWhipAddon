using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
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


    public class Loadout_Laser : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 4;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = -1; // ★ 無限貫通
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 120; // 画面を横切るだけ
            Projectile.extraUpdates = 3;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;

            Projectile.DamageType = DamageClass.Summon;

            Projectile.ArmorPenetration = 9999;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(
                    SoundID.Item33 with { Volume = 1.3f },
                    Projectile.Center // ★ 飛んでくる方向
                );
                Projectile.localAI[0] = 1f;
            }

            Rectangle screen = new Rectangle(
                (int)Main.screenPosition.X - 400,
                (int)Main.screenPosition.Y - 400,
                Main.screenWidth + 800,
                Main.screenHeight + 800
            );

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (!screen.Contains(Projectile.Center.ToPoint()))
            {
                Projectile.Kill();
                return;
            }

            Lighting.AddLight(Projectile.Center, 0.8f, 0.2f, 0.2f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() / 2f;

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                Color.White,
                Projectile.rotation,
                origin,
                2f,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}