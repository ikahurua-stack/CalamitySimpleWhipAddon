using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using CalamityMod.Items.Accessories;
using CalamitySimpleWhipAddon.Content.Items.Accessories;

namespace CalamitySimpleWhipAddon.Content.Common.GlobalItems
{
    public class AccessoryBleachedExclusion : GlobalItem
    {
        // バニラで除外するアクセ（同時装備OK）
        private static readonly HashSet<int> VanillaAllowed = new()
        {
            ItemID.PapyrusScarab,
            ItemID.PygmyNecklace,
            ItemID.SummonerEmblem
        };

        // Bleached → 排他対象（元＋素材ツリー）
        private static readonly Dictionary<int, HashSet<int>> BleachedExclusions = new()
        {
            {
                ModContent.ItemType<BleachedVoltaicJelly>(),
                new HashSet<int>
                {
                    ModContent.ItemType<JellyChargedBattery>(),
                    ModContent.ItemType<StarTaintedGenerator>(),
                    ModContent.ItemType<Nucleogenesis>(),
                    ModContent.ItemType<VoltaicJelly>()
                }
            },
            {
                ModContent.ItemType<BleachedJellyChargedBattery>(),
                new HashSet<int>
                {
                    ModContent.ItemType<StarTaintedGenerator>(),
                    ModContent.ItemType<Nucleogenesis>(),
                    ModContent.ItemType<JellyChargedBattery>(),
                    ModContent.ItemType<VoltaicJelly>(),
                    ModContent.ItemType<BleachedVoltaicJelly>()
                }
            },
            {
                ModContent.ItemType<BleachedNuclearFuelRod>(),
                new HashSet<int>
                {
                    ModContent.ItemType<StarTaintedGenerator>(),
                    ModContent.ItemType<Nucleogenesis>(),
                    ModContent.ItemType<NuclearFuelRod>()
                }
            },
            {
                ModContent.ItemType<BleachedStarTaintedGenerator>(),
                new HashSet<int>
                {
                    ModContent.ItemType<Nucleogenesis>(),
                    ModContent.ItemType<StarTaintedGenerator>(),
                    ModContent.ItemType<JellyChargedBattery>(),
                    ModContent.ItemType<NuclearFuelRod>(),
                    ModContent.ItemType<VoltaicJelly>(),
                    ModContent.ItemType<BleachedNuclearFuelRod>(),
                    ModContent.ItemType<BleachedJellyChargedBattery>(),
                    ModContent.ItemType<BleachedVoltaicJelly>()
                }
            },
            {
                ModContent.ItemType<BleachedTheFirstShadowflame>(),
                new HashSet<int>
                {
                    ModContent.ItemType<StatisCurse>(),
                    ModContent.ItemType<Nucleogenesis>(),
                    ModContent.ItemType<TheFirstShadowflame>()
                }
            },
            {
                ModContent.ItemType<BleachedStatisCurse>(),
                new HashSet<int>
                {
                    ModContent.ItemType<Nucleogenesis>(),
                    ModContent.ItemType<StatisCurse>(),
                    ModContent.ItemType<StatisBlessing>(),
                    ModContent.ItemType<TheFirstShadowflame>(),
                    ModContent.ItemType<BleachedTheFirstShadowflame>(),
                }
            },
            {
                ModContent.ItemType<BleachedNucleogenesis>(),
                new HashSet<int>
                {
                    ModContent.ItemType<Nucleogenesis>(),
                    ModContent.ItemType<StarTaintedGenerator>(),
                    ModContent.ItemType<StatisCurse>(),
                    ModContent.ItemType<StatisBlessing>(),
                    ModContent.ItemType<TheFirstShadowflame>(),
                    ModContent.ItemType<JellyChargedBattery>(),
                    ModContent.ItemType<NuclearFuelRod>(),
                    ModContent.ItemType<VoltaicJelly>(),
                    ModContent.ItemType<BleachedStatisCurse>(),
                    ModContent.ItemType<BleachedTheFirstShadowflame>(),
                    ModContent.ItemType<BleachedStarTaintedGenerator>(),
                    ModContent.ItemType<BleachedNuclearFuelRod>(),
                    ModContent.ItemType<BleachedJellyChargedBattery>(),
                    ModContent.ItemType<BleachedVoltaicJelly>()
                }
            }
        };

        public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded)
        {
            // バニラ除外
            if (VanillaAllowed.Contains(item.type))
                return true;

            foreach (Item equipped in player.armor)
            {
                if (equipped == null || equipped.IsAir)
                    continue;

                // 装備済みがバニラ除外対象なら無視
                if (VanillaAllowed.Contains(equipped.type))
                    continue;

                // item が Bleached
                if (BleachedExclusions.TryGetValue(item.type, out var blocked))
                {
                    if (blocked.Contains(equipped.type))
                        return false;
                }

                // 装備済みが Bleached
                if (BleachedExclusions.TryGetValue(equipped.type, out var reverseBlocked))
                {
                    if (reverseBlocked.Contains(item.type))
                        return false;
                }
            }

            return true;
        }
    }
}
