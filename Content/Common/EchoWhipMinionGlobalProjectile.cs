using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamitySimpleWhipAddon.Content.Common.Players;
using Terraria.DataStructures;
using System;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;

namespace CalamitySimpleWhipAddon.Content.Common
{
    public class EchoWhipMinionGlobalProjectile : GlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
            if (projectile.owner < 0)
                return;

            if (!projectile.minion)
                return;

            Player player = Main.player[projectile.owner];

            if (!player.GetModPlayer<EchoWhipPlayer>().echoAccessory)
                return;

            projectile.damage = 0;
            projectile.originalDamage = 0;

            projectile.Center = new Vector2(-1000000f, -1000000f);
        }

        public override void OnSpawn(
        Projectile projectile,
        IEntitySource source)
        {
            if (projectile.owner < 0)
                return;

            Player player = Main.player[projectile.owner];

            if (!player.GetModPlayer<EchoWhipPlayer>().echoAccessory)
                return;

            if (projectile.type == ModContent.ProjectileType<WhipEchoDamage>())
                return;

            if (projectile.sentry)
            {
                projectile.Kill();
            }

            if (projectile.minion)
            {

                projectile.damage = 0;
                projectile.originalDamage = 0;

                if (projectile.minionSlots <= 0f)
                    return;
                projectile.Kill();
                
            }

            if (ProjectileID.Sets.MinionShot[projectile.type])
            {
                projectile.Center = new Vector2(-1000000f, -1000000f);

                projectile.Kill();
                return;
            }

            if (source is EntitySource_Parent parent &&
                parent.Entity is Projectile parentProj)
            {
                if (parentProj.minion || parentProj.sentry)
                {
                    projectile.Center = new Vector2(-1000000f, -1000000f);
                    projectile.damage = 0;
                    projectile.originalDamage = 0;

                    projectile.Kill();
                    return;
                }

                if (ProjectileID.Sets.MinionShot[parentProj.type])
                {
                    projectile.Center = new Vector2(-1000000f, -1000000f);
                    projectile.damage = 0;
                    projectile.originalDamage = 0;

                    projectile.Kill();
                    return;
                }

            }
        }

    }
}