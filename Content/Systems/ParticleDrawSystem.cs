using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Particles;
using CalamitySimpleWhipAddon.Content.Projectiles;

namespace CalamitySimpleWhipAddon.Content.Systems
{
    public class ParticleDrawSystem : ModSystem
    {
        // 毎フレームのメモリ確保（ゴミ）を防ぐため、再利用可能なリストを定義
        private static readonly List<IAdditiveProjectile> _additiveCache = new();
        private static readonly List<IAlphaBlendProjectile> _alphaBlendCache = new();

        public override void PostDrawTiles()
        {
            if (Main.dedServ) return;

            // 前フレームのキャッシュをクリア
            _additiveCache.Clear();
            _alphaBlendCache.Clear();

            // 【改善】ループは1回だけ（1000回）。ここで描画対象を一気に集める
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (!p.active) continue; // アクティブでないなら即スキップ（超軽量）

                // 型チェックをして、対象ならリストに登録
                if (p.ModProjectile is IAdditiveProjectile additiveProj)
                {
                    _additiveCache.Add(additiveProj);
                }
                else if (p.ModProjectile is IAlphaBlendProjectile alphaBlendProj)
                {
                    _alphaBlendCache.Add(alphaBlendProj);
                }
            }

            // ==================== 1. 加算（Additive）描画 ====================
            // 描画するものが「ある時だけ」Beginする
            if (_additiveCache.Count > 0 || ElectricArcSystem.Arcs.Count > 0)
            {
                Main.spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.Additive,
                    SamplerState.LinearClamp,
                    DepthStencilState.None,
                    RasterizerState.CullNone,
                    null,
                    Main.GameViewMatrix.ZoomMatrix
                );

                foreach (var proj in _additiveCache)
                {
                    proj.DrawAdditive(Main.spriteBatch);
                }

                ElectricArcSystem.UpdateAndDraw(Main.spriteBatch);


                Main.spriteBatch.End();
            }

            // ==================== 2. 通常（AlphaBlend）描画 ====================
            // 描画するものが「ある時だけ」Beginする
            if (_alphaBlendCache.Count > 0)
            {
                Main.spriteBatch.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.LinearClamp,
                    DepthStencilState.None,
                    RasterizerState.CullNone,
                    null,
                    Main.GameViewMatrix.ZoomMatrix
                );

                foreach (var proj in _alphaBlendCache)
                {
                    proj.DrawAlphaBlend(Main.spriteBatch);
                }

                Main.spriteBatch.End();
            }
        }
    }


}
