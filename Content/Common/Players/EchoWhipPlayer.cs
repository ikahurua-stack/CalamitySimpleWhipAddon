using Terraria;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Common.Players
{
    public class EchoWhipPlayer : ModPlayer
    {
        public bool echoAccessory;

        private bool lastEchoAccessory;

        public override void ResetEffects()
        {
            echoAccessory = false;
        }

        public override void PostUpdate()
        {
            if (echoAccessory && !lastEchoAccessory)
            {
                CleanupMinions();
            }

            lastEchoAccessory = echoAccessory;
        }

        public override void PostUpdateEquips()
        {
            if (!echoAccessory)
                return;

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
            {
                Player.GetDamage<SummonDamageClass>() += freeSlots * 0.1f;
            }
        }

        private void CleanupMinions()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (!proj.active)
                    continue;

                if (proj.owner != Player.whoAmI)
                    continue;

                if (proj.minion || proj.sentry)
                    proj.Kill();
            }
        }

        public override bool CanUseItem(Item item)
        {
            if (!echoAccessory)
                return true;

            if (item.DamageType != DamageClass.Summon)
                return true;

            if (item.buffType > 0)
                return false;

            return true;
        }
    }
}