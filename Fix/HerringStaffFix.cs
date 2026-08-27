using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Weapons.Summon;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;

public class HerringStaffFix : ModPlayer
{
    public override bool CanUseItem(Item item)
    {
        if (item.type != ModContent.ItemType<HerringStaff>())
            return true;

        float usedSlots = 0f;

        foreach (Projectile proj in Main.projectile)
        {
            if (!proj.active)
                continue;

            if (proj.owner != Player.whoAmI)
                continue;

            if (!proj.minion)
                continue;

            usedSlots += proj.minionSlots;
        }

        float freeSlots = Player.maxMinions - usedSlots;

        if (freeSlots > 0f)
            return true;

        return false;
    }
}

