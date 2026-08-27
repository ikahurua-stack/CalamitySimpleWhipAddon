using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Particles;

namespace CalamitySimpleWhipAddon.Content.Systems
{
    public class AdditiveParticleSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            AdditiveTagDrawer.Update();
            CustomParticleHandler.UpdateAll();
        }

        public override void PostDrawTiles()
        {
            if (Main.dedServ) return;

            // ★【劇的軽量化】どちらも空なら、Beginすら呼ばずにここで終わらせる！
            if (CustomParticleHandler.IsEmpty && AdditiveTagDrawer.IsEmpty)
                return;

            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.Additive,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix
            );

            CustomParticleHandler.DrawAll(Main.spriteBatch);
            AdditiveTagDrawer.Draw(Main.spriteBatch);

            Main.spriteBatch.End();
        }

        public override void OnWorldUnload()
        {
            CustomParticleHandler.ClearAll();
        }
    }

}
