using Terraria;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Projectiles;
using Microsoft.Xna.Framework;

namespace CalamitySimpleWhipAddon.Content.Common.Players
{
    public class KingsMajestyPlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            if (Player.HeldItem.type != ModContent.ItemType<Items.Weapons.KingsMajesty>())
                return;

            bool found = false;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];

                if (p.active &&
                    p.owner == Player.whoAmI &&
                    p.type == ModContent.ProjectileType<Projectiles.KingsMajestyGem>())
                {
                    found = true;
                    break;
                }
            }

            if (!found && Player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(
                    Player.GetSource_Misc("KingsMajestyGem"),
                    Player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<Projectiles.KingsMajestyGem>(),
                    0,
                    0,
                    Player.whoAmI);
            }
        }
    }

    public class OpaqueOnyxPlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            if (Player.HeldItem.type != ModContent.ItemType<Items.Weapons.OpaqueOnyx>())
                return;

            bool found = false;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];

                if (p.active &&
                    p.owner == Player.whoAmI &&
                    p.type == ModContent.ProjectileType<Projectiles.OpaqueOnyxGem>())
                {
                    found = true;
                    break;
                }
            }

            if (!found && Player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(
                    Player.GetSource_Misc("OpaqueOnyxGem"),
                    Player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<Projectiles.OpaqueOnyxGem>(),
                    0,
                    0,
                    Player.whoAmI);
            }
        }
    }

    public class EmeraldSplashPlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            if (Player.HeldItem.type != ModContent.ItemType<Items.Weapons.EmeraldSplash>())
                return;

            bool found = false;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];

                if (p.active &&
                    p.owner == Player.whoAmI &&
                    p.type == ModContent.ProjectileType<Projectiles.EmeraldSplashGem>())
                {
                    found = true;
                    break;
                }
            }

            if (!found && Player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(
                    Player.GetSource_Misc("EmeraldSplashGem"),
                    Player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<Projectiles.EmeraldSplashGem>(),
                    0,
                    0,
                    Player.whoAmI);
            }
        }
    }

    public class RubellusPlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            if (Player.HeldItem.type != ModContent.ItemType<Items.Weapons.Rubellus>())
                return;

            bool found = false;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];

                if (p.active &&
                    p.owner == Player.whoAmI &&
                    p.type == ModContent.ProjectileType<Projectiles.RubellusGem>())
                {
                    found = true;
                    break;
                }
            }

            if (!found && Player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(
                    Player.GetSource_Misc("RubellusGem"),
                    Player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<Projectiles.RubellusGem>(),
                    0,
                    0,
                    Player.whoAmI);
            }
        }
    }

    public class CrackoftheUniversePlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            if (Player.HeldItem.type != ModContent.ItemType<Items.Weapons.CrackoftheUniverse>())
                return;

            bool found = false;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];

                if (p.active &&
                    p.owner == Player.whoAmI &&
                    p.type == ModContent.ProjectileType<Projectiles.CrackoftheUniverseGem>())
                {
                    found = true;
                    break;
                }
            }

            if (!found && Player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(
                    Player.GetSource_Misc("CrackoftheUniverseGem"),
                    Player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<Projectiles.CrackoftheUniverseGem>(),
                    0,
                    0,
                    Player.whoAmI);
            }
        }
    }
}