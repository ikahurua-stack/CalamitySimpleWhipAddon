using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Graphics.Metaballs;

namespace CalamitySimpleWhipAddon.Content.Rendering
{
    public sealed class ChorusofExecrationMetaball : BaseChargeMetaball
    {
        protected override ChargeTheme Theme =>
            ChargeTheme.chorusofExecration;

        public override IEnumerable<Texture2D> Layers
        {
            get
            {
                foreach (var layer in LayerExecration)
                    yield return layer.Value;
            }
        }

        public override Color EdgeColor =>
            new(61, 6, 2);

        public override void Load()
        {
            base.Load();

            if (Main.dedServ)
                return;

            LayerExecration = new Asset<Texture2D>[5];

            for (int i = 0; i < 5; i++)
            {
                LayerExecration[i] =
                    ModContent.Request<Texture2D>(
                        $"CalamityMod/Graphics/Metaballs/GruesomeEminence_Ghost_Layer{i + 1}",
                        AssetRequestMode.ImmediateLoad);
            }
        }

        public override Vector2 CalculateManualOffsetForLayer(int layerIndex)
        {
            switch (layerIndex)
            {
                case 0:
                    return Vector2.UnitX * Main.GlobalTimeWrappedHourly * 0.03f;

                case 1:
                    {
                        Vector2 offset =
                            Vector2.One *
                            (float)Math.Cos(Main.GlobalTimeWrappedHourly * 0.041f) * 2f;

                        offset = offset.RotatedBy(
                            (float)Math.Cos(Main.GlobalTimeWrappedHourly * 0.08f) * 0.97f);

                        return offset;
                    }

                case 2:
                    {
                        Vector2 offset =
                            (Main.GlobalTimeWrappedHourly * 2.02f).ToRotationVector2() * 0.036f;

                        offset.Y +=
                            (float)Math.Cos(Main.GlobalTimeWrappedHourly * 0.161f) * 0.5f + 0.5f;

                        return offset;
                    }

                case 3:
                    {
                        Vector2 offset =
                            Vector2.UnitX * Main.GlobalTimeWrappedHourly * -0.04f +
                            (Main.GlobalTimeWrappedHourly * 1.89f).ToRotationVector2() * 0.03f;

                        offset.Y +=
                            CalamityUtils.PerlinNoise2D(
                                Main.GlobalTimeWrappedHourly * 0.187f,
                                Main.GlobalTimeWrappedHourly * 0.193f,
                                2,
                                466920161) * 0.025f;

                        return offset;
                    }

                case 4:
                    {
                        Vector2 offset =
                            Vector2.UnitX * Main.GlobalTimeWrappedHourly * 0.037f +
                            (Main.GlobalTimeWrappedHourly * 1.77f).ToRotationVector2() * 0.04725f;

                        offset.Y +=
                            CalamityUtils.PerlinNoise2D(
                                Main.GlobalTimeWrappedHourly * 0.187f,
                                Main.GlobalTimeWrappedHourly * 0.193f,
                                2,
                                577215664) * 0.05f;

                        return offset;
                    }
            }

            return Vector2.Zero;
        }
    }
}