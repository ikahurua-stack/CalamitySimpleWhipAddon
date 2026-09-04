using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Common
{
    /// <summary>Updates balance numbers in localized tooltips when Infernal Eclipse is present.</summary>
    public sealed class InfernalEclipseTooltipGlobalItem : GlobalItem
    {
        private static readonly Dictionary<string, (string From, string To)[]> Replacements = new()
        {
            ["WoodenWhip"] = new[] { ("1.07", "1.05") },
            ["RapierWhip"] = new[] { ("8%", "6%") },
            ["Ectopia"] = new[] { ("22%", "13%") },
            ["EntwinedBranches"] = new[] { ("1.27", "1.13") },
            ["Necropsia"] = new[] { ("27%", "18%") },
            ["ResonantVoid"] = new[] { ("1.12", "1.1"), ("12%", "10%") },
            ["Milkyway"] = new[] { ("4倍", "3倍"), ("4x", "3x") },
            ["ButterflyEffect"] = new[] { ("+6", "+5"), ("6 召喚", "5 召喚"), ("6 summon", "5 summon") },
            ["GoldRush"] = new[] { ("100%", "40%") },
            ["GildedReliquary"] = new[] { ("120%", "80%") },
            ["AurelianSanctum"] = new[] { ("80 召喚", "40 召喚"), ("80 summon", "40 summon"), ("80 召唤", "40 召唤"), ("200%", "100%"), ("30 summon", "40 summon"), ("30 урона", "40 урона") },
            ["EmeraldSplash"] = new[] { ("15%", "10%") },
            ["CrackoftheUniverse"] = new[] { ("20 召喚", "8 召喚"), ("20 summon", "8 summon"), ("20 召唤", "8 召唤"), ("10%", "6%"), ("20%", "10%") },
            ["OpaqueOnyx"] = new[] { ("8%", "4%"), ("40%", "10%") },
            ["ChorusofExecration"] = new[] { ("30%", "10%"), ("25%", "20%") },
        };

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!InfernalEclipseCompatibility.IsEnabled || item.ModItem?.Mod.Name != "CalamitySimpleWhipAddon" ||
                !Replacements.TryGetValue(item.ModItem.Name, out var replacements))
            {
                return;
            }

            foreach (TooltipLine line in tooltips)
            {
                if (!line.Name.StartsWith("Tooltip"))
                    continue;

                foreach ((string from, string to) in replacements)
                    line.Text = line.Text.Replace(from, to);
            }
        }
    }
}
