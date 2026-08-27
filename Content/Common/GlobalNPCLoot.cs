using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using CalamitySimpleWhipAddon.Content.Common;

namespace CalamitySimpleWhipAddon.Content.Common
{
    public class GlobalNPCLoot : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (NPCLootRegistry.LootTable.TryGetValue(npc.type, out var rules))
            {
                foreach (IItemDropRule rule in rules)
                {
                    npcLoot.Add(rule);
                }
            }
        }
    }
}
