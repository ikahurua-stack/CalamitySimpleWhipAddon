using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using CalamitySimpleWhipAddon.Content.Items.Weapons;
using CalamitySimpleWhipAddon.Content.Items.Accessories;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Deconstructors;

namespace CalamitySimpleWhipAddon.Content.Common
{
    public static class NPCLootRegistry
    {
        public static readonly Dictionary<int, List<IItemDropRule>> LootTable
            = new();

        static NPCLootRegistry()
        {
            // Possessed Armor
            Register(
                NPCID.PossessedArmor,
                ItemDropRule.ByCondition(
                    new Conditions.IsHardmode(),
                    ModContent.ItemType<KusariGama>(),
                    100
                )
            );

            Register(
                ModContent.NPCType<Burrower>(),
                ItemDropRule.ByCondition(
                    new Conditions.DownedPlantera(),
                    ModContent.ItemType<Loadout>(),
                    1
                )
            );

            Register(
                NPCID.LavaSlime,
                ItemDropRule.Common(
                    ModContent.ItemType<AncientBonds>(),
                    50
                )
            );
            // 今後ここにどんどん追加できる
            /*
            Register(
                NPCID.Skeleton,
                ItemDropRule.Common(
                    ModContent.ItemType<AnotherWeapon>(),
                    10
                )
            );
            */
        }

        private static void Register(int npcType, IItemDropRule rule)
        {
            if (!LootTable.TryGetValue(npcType, out var list))
            {
                list = new List<IItemDropRule>();
                LootTable[npcType] = list;
            }

            list.Add(rule);
        }
    }
}
