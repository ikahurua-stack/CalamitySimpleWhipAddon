using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamitySimpleWhipAddon.Content.Items.Accessories;

namespace CalamitySimpleWhipAddon.Content.Common.GlobalItems
{
    public class WhipGripGlobalItem : GlobalItem
    {
        public override bool CanEquipAccessory(Item item, Player player, int slot, bool visualOnly)
        {
            // 見た目スロットは常に許可
            if (visualOnly || slot >= 10 + player.extraAccessorySlots)
                return true;

            if (!IsWhipGrip(item))
                return true;

            for (int i = 3; i < 10 + player.extraAccessorySlots; i++)
            {
                // 今まさに入れ替え対象のスロットは無視
                if (i == slot)
                    continue;

                Item equipped = player.armor[i];
                if (equipped == null || equipped.IsAir)
                    continue;

                if (IsWhipGrip(equipped))
                    return false;
            }

            return true;
        }

        private bool IsWhipGrip(Item item)
        {
            return item.type == ModContent.ItemType<EmperorsGrip>()
                || item.type == ModContent.ItemType<LeatherGrip>()
                || item.type == ModContent.ItemType<NecromanticGrip>()
                || item.type == ModContent.ItemType<RubberGrip>()
                || item.type == ModContent.ItemType<SilkGrip>()
                || item.type == ModContent.ItemType<LightSpiritGrip>()
                || item.type == ModContent.ItemType<AirflowGrip>()
                || item.type == ModContent.ItemType<CommanderGrip>()
                || item.type == ModContent.ItemType<MagneticGrip>();

        }
    }
}

