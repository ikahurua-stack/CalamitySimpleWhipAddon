using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Projectiles;

namespace CalamitySimpleWhipAddon.Content.Common
{
    public class EchoWhipGlobalProjectile : GlobalProjectile
    {
        public override void OnHitNPC(
            Projectile projectile,
            NPC target,
            NPC.HitInfo hit,
            int damageDone)
        {
            bool isInfernalEclipseWhip = false;
            if (InfernalEclipseCompatibility.IsEnabled &&
                ModLoader.TryGetMod(InfernalEclipseCompatibility.ModName, out Mod infernalEclipse) &&
                infernalEclipse.TryFind<ModProjectile>("SplitFirebrandFlailProjectile", out ModProjectile firebrandFlail))
            {
                isInfernalEclipseWhip = projectile.type == firebrandFlail.Type;
            }

            if (!ProjectileID.Sets.IsAWhip[projectile.type] && !isInfernalEclipseWhip)
                return;

            Player player = Main.player[projectile.owner];

            if (!player.GetModPlayer<EchoWhipPlayer>().echoAccessory)
                return;

            Projectile.NewProjectile(
                projectile.GetSource_OnHit(target),
                target.Center,
                Microsoft.Xna.Framework.Vector2.Zero,
                ModContent.ProjectileType<WhipEchoDamage>(),
                damageDone / 3,
                0f,
                player.whoAmI,
                target.whoAmI
            );
        }
    }
}
