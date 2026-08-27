using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Common.Players
{
    public class WhipAccessoryPlayer : ModPlayer
    {

        // ムチ用アクセフラグ
        public bool silkGrip;
        public bool leatherGrip;
        public bool rubberGrip;
        public bool necromanticGrip;
        public bool emperorsGrip;
        public bool lightSpiritGrip;
        public bool airflowGrip;
        public bool commanderGrip;
        public bool magneticGrip;
        public bool whipMagnet;

        public override void ResetEffects()
        {
            silkGrip = false;
            leatherGrip = false;
            rubberGrip = false;
            necromanticGrip = false;
            emperorsGrip = false;
            lightSpiritGrip = false;
            airflowGrip = false;
            commanderGrip = false;
            magneticGrip = false;
            whipMagnet = false;
        }

        private readonly Dictionary<int, List<Vector2>> previousWhipPoints = new();
        private readonly Dictionary<int, int> hookedItems = new();

        public override void PostUpdate()
        {
            if (!whipMagnet)
                return;

            bool hasWhip = false;

            foreach (Projectile proj in Main.ActiveProjectiles)
            {
                if (proj.owner == Player.whoAmI &&
                    ProjectileID.Sets.IsAWhip[proj.type])
                {
                    hasWhip = true;
                    break;
                }
            }

            if (!hasWhip)
            {
                foreach (int itemIndex in hookedItems.Keys)
                {
                    Item item = Main.item[itemIndex];

                    if (!item.active)
                        continue;

                    item.Center = Player.Center;
                }

                hookedItems.Clear();
                previousWhipPoints.Clear();
            }


            AttractItemsToWhips();
        }

        private void AttractItemsToWhips()
        {
            foreach (Projectile proj in Main.ActiveProjectiles)
            {
                if (!proj.active)
                    continue;

                if (proj.owner != Player.whoAmI)
                    continue;

                if (!ProjectileID.Sets.IsAWhip[proj.type])
                    continue;

                ProcessWhip(proj);
            }
        }

        private void ProcessWhip(Projectile whip)
        {
            List<Vector2> currentPoints = new();
            Projectile.FillWhipControlPoints(whip, currentPoints);

            if (currentPoints.Count == 0)
                return;


            if (!previousWhipPoints.TryGetValue(
                    whip.whoAmI,
                    out List<Vector2> oldPoints))
            {
                previousWhipPoints[whip.whoAmI] =
                    new List<Vector2>(currentPoints);

                return;
            }

            foreach (Item item in Main.ActiveItems)
            {
                if (!item.active || item.IsAir)
                    continue;

                for (int i = 0; i < currentPoints.Count; i++)
                {
                    Vector2 start = oldPoints[i];
                    Vector2 end = currentPoints[i];

                    Vector2 closest =
                        Utils.ClosestPointOnLine(
                            start,
                            end,
                            item.Center);

                    float dist =
                        Vector2.Distance(
                            closest,
                            item.Center);

                    if (dist < 25f)
                    {
                        if (!hookedItems.ContainsKey(item.whoAmI))
                        {
                            hookedItems[item.whoAmI] = whip.whoAmI;
                        }
                    }

                }

            }

            foreach (var pair in hookedItems.ToList())
            {
                int itemIndex = pair.Key;
                int whipIndex = pair.Value;

                Item item = Main.item[itemIndex];

                if (!item.active)
                {
                    hookedItems.Remove(itemIndex);
                    continue;
                }

                Projectile whipProj = Main.projectile[whipIndex];

                if (!whipProj.active)
                {
                    hookedItems.Remove(itemIndex);
                    continue;
                }

                List<Vector2> whipPoints = new();
                Projectile.FillWhipControlPoints(whipProj, whipPoints);

                if (whipPoints.Count == 0)
                    continue;

                item.Center = whipPoints[^1];
                item.velocity = Vector2.Zero;

            }


            previousWhipPoints[whip.whoAmI] =
                new List<Vector2>(currentPoints);
        }
    }


    public class WhipHoldEffectPlayer : ModPlayer
    {
        public bool holdingNoKnockbackWhip;

        public override void ResetEffects()
        {
            if (holdingNoKnockbackWhip)
            {
                Player.noKnockback = true;
            }

            // 毎tickリセット（これが重要）
            holdingNoKnockbackWhip = false;
        }

    }
}