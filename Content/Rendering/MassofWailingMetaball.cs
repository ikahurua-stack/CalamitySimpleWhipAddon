using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Graphics.Metaballs;

namespace CalamitySimpleWhipAddon.Content.Rendering
{
    public sealed class MassofWailingMetaball : BaseChargeMetaball
    {
        protected override ChargeTheme Theme =>
            ChargeTheme.massofWailing;

        public override IEnumerable<Texture2D> Layers
        {
            get
            {
                yield return LayerWailing.Value;
            }
        }

        public override Color EdgeColor =>
            Color.Red;

        public override void Load()
        {
            base.Load();

            if (Main.dedServ)
                return;

            LayerWailing = ModContent.Request<Texture2D>(
                "CalamityMod/Graphics/Metaballs/CalamitasLayer",
                AssetRequestMode.ImmediateLoad);
        }

        public override Vector2 CalculateManualOffsetForLayer(int layerIndex)
        {
            return Vector2.UnitX * Main.GlobalTimeWrappedHourly * 0.04f;
        }
    }
}