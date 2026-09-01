using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Accessories;

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
            var IEoR = ModLoader.GetMod("InfernalEclipseAPI");
            int firebrandFlail = -1;

            if (IEoR != null)
            {
                firebrandFlail = IEoR.Find<ModProjectile>("SplitFirebrandFlailProjectile")?.Type ?? -1;
            }

            if (!ProjectileID.Sets.IsAWhip[projectile.type] && projectile.type != firebrandFlail)
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