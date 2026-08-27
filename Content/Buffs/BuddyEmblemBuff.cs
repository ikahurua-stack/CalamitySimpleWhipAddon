using Terraria;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Common.Players;

namespace CalamitySimpleWhipAddon.Content.Buffs
{
    public class BuddyEmblemBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            var mp = Main.LocalPlayer.GetModPlayer<Common.Players.BuddyEmblemPlayer>();
            tip = $"Minion damage x{mp.BuddyEmblemBonus:0.00}\n" +
                "Solo minion bonus non-active";
        }
    }

    public class BuddyEmblemSoloBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            var mp = Main.LocalPlayer.GetModPlayer<Common.Players.BuddyEmblemPlayer>();

            tip =
                $"Minion damage x{mp.BuddyEmblemBonus:0.00}\n" +
                "Solo minion bonus active";
        }
    }

    public class BuddyEmblemSoloHalfBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            var mp = Main.LocalPlayer.GetModPlayer<Common.Players.BuddyEmblemPlayer>();

            tip =
                $"Minion damage x{mp.BuddyEmblemHalfBonus:0.00}\n" +
                "Solo minion half bonus active\n" +
                "The emblem's effect is being weakened by the summoned minions";
        }
    }


}
