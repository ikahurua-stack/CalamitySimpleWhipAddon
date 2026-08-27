using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using System.Collections.Generic;
using CalamitySimpleWhipAddon.Content.Projectiles;

namespace CalamitySimpleWhipAddon.Content.Common
{
    public class WhipRangeCompatibility : GlobalProjectile
    {
        public override bool InstancePerEntity => true;


        private static readonly Dictionary<int, float> SOTSRangeBonus = new()
        {
            { ModContent.ProjectileType<DroptideProj>(), 1.4f },
            { ModContent.ProjectileType<KingsMajestyProj>(), 1.4f },
            { ModContent.ProjectileType<LayeredPainProj>(), 1.25f },
            { ModContent.ProjectileType<MandibleLashProj>(), 1.35f },
            { ModContent.ProjectileType<OpaqueOnyxProj>(), 1.16f },
            { ModContent.ProjectileType<ShieldConduitProj>(), 1.12f },
            { ModContent.ProjectileType<DarkMandibleLashProj>(), 1.3f },
            { ModContent.ProjectileType<BreezePiercerProj>(), 1.25f },
            { ModContent.ProjectileType<AncientBondsProj>(), 1.2f },
            { ModContent.ProjectileType<NightButterflyProj>(), 1.1f },
            { ModContent.ProjectileType<GelxyriboseProj>(), 1.15f },
            { ModContent.ProjectileType<GlitterGutterProj>(), 1.05f },
            { ModContent.ProjectileType<PrimalBondsProj>(), 1.05f },
            { ModContent.ProjectileType<ShieldConduitMkIIProj>(), 1.05f },
            { ModContent.ProjectileType<RapierWhipProj>(), 1.3f },
            { ModContent.ProjectileType<WulfrumArmProj>(), 1.2f },
            { ModContent.ProjectileType<WoodenWhipProj>(), 1.2f },
        };

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (!ModLoader.HasMod("SOTS"))
                return;

            if (!ProjectileID.Sets.IsAWhip[projectile.type])
                return;

            if (SOTSRangeBonus.TryGetValue(projectile.type, out float multiplier))
                projectile.WhipSettings.RangeMultiplier *= multiplier;

        }
    }
}