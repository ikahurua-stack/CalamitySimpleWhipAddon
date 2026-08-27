using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using CalamitySimpleWhipAddon.Content.Common;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.DraedonsArsenal;

namespace CalamitySimpleWhipAddon.Content.Systems
{
	public class BetterHornetStingerMinionFix : ModSystem
	{
		public override void PostSetupContent()
		{
			int type = ModContent.ProjectileType<BetterHornetStinger>();

			ProjectileID.Sets.MinionShot[type] = true;
		}
	}

    public class ShrimpPlasmaMissileMinionFix : ModSystem
    {
        public override void PostSetupContent()
        {
            int type = ModContent.ProjectileType<ShrimpPlasmaMissile>();

            ProjectileID.Sets.MinionShot[type] = true;
        }
    }

    public class OtherModsMinionFix : ModSystem
    {
        public override void PostSetupContent()
        {

            foreach (int type in ExternalProjectileRegistry.GetRegisteredTypes())
            {
                if (type > 0) 
                {
                    ProjectileID.Sets.MinionShot[type] = true;
                }
            }
        }
    }


}