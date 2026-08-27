using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using CalamitySimpleWhipAddon.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Typeless;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class EntwinedBranchesProj : ModProjectile
    {
        // パーティクル専用タイマー
        private float particleTimer;
        private bool particleInitDone = false;

        private bool clearedOnSpawn = false;

        public Color InnerColor = Color.Green;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();
            Projectile.WhipSettings.Segments = 60;        // ムチの節の数。増やすとムチが長く滑らかに見える
            Projectile.WhipSettings.RangeMultiplier = 3.85f; // ムチの伸びる長さ。大きくすると遠くまで届く
        }

        // デバフは元の位置のまま
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SimpleWhipDebuff27>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.98f); // ヒットごとのダメージ減少率
        }

        public override void AI()
        {

            if (!clearedOnSpawn)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    CustomParticleHandler.ClearAll();
                }
                clearedOnSpawn = true;
            }

            Lighting.AddLight(Projectile.Center, InnerColor.ToVector3() * 0.2f);
            Player owner = Main.player[Projectile.owner];

            // 攻撃開始時のみパーティクルをクリア
            if (!particleInitDone)
            { // 既存のパーティクルを消す
                particleInitDone = true;
            }

            particleTimer++;

            // ムチ先端位置を取得
            List<Vector2> controlPoints = new();
            Projectile.FillWhipControlPoints(Projectile, controlPoints);

            if (controlPoints.Count >= 2)
            {
                Vector2 tip = controlPoints[^1];
                Vector2 prev = controlPoints[^2];

                Vector2 tipDir = Vector2.Normalize(tip - prev);



                // プレイヤー付近では生成しない
                if (Vector2.Distance(tip, owner.Center) > 70f) // 70f を大きくするとプレイヤー付近にはパーティクルが出にくくなる
                {
                    // パーティクル生成
                    if (Main.rand.NextBool(1))
                    {
                        SpawnParticle(tip);
                    }
                    if (Main.rand.NextBool(1))
                    {
                        SpawnParticle(tip);
                    }


                    // 白い少し大きめのダスト
                    for (int i = 0; i < 1; i++) // ループ回数を増やすと同時に出るダストが増える
                    {
                        int dustIndex = Dust.NewDust(
                            tip - new Vector2(4, 4), // 生成位置の微調整。数字を大きくするとムチ先端から離れる
                            8, 8,                     // 生成範囲。幅と高さ
                            DustID.Sluggy,         // Dustの種類。別のIDにすると違う見た目になる
                            Main.rand.NextFloat(-5f, 5f), // X速度範囲。数値を広げると飛ぶ速度が速くなる
                            Main.rand.NextFloat(-5f, 5f), // Y速度範囲
                            0,                        // Alpha（透明度）。0が不透明、255が完全透明
                            Color.Green,                 // 色
                            Main.rand.NextFloat(0.7f, 1.0f) // スケール。大きくすると大きなダストになる
                        );
                        Main.dust[dustIndex].noGravity = false; // trueで重力無効、falseで落ちる
                    }

                    // ライトグリーンの星型ダスト
                    for (int i = 0; i < 2; i++)
                    {
                        int dustIndex = Dust.NewDust(
                            tip - new Vector2(4, 4),
                            8, 8,
                            DustID.JunglePlants, // 星型の見た目。別IDにすると違う形状になる
                            Main.rand.NextFloat(-5f, 5f),
                            Main.rand.NextFloat(-5f, 5f),
                            0,
                            Color.Green,
                            Main.rand.NextFloat(0.5f, 1.0f)
                        );
                        Main.dust[dustIndex].noGravity = false;
                    }


                }

            }

            // パーティクル更新
            CustomParticleHandler.UpdateAll();
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                CustomParticleHandler.ClearAll();
            }
        }

        private void SpawnParticle(Vector2 tip)
        {
            // offsetの範囲を広げるとパーティクルの散らばりが広がる
            Vector2 offset = new Vector2(Main.rand.NextFloat(-12f, 12f), Main.rand.NextFloat(-12f, 12f));

            Texture2D tex = ModContent.Request<Texture2D>("CalamitySimpleWhipAddon/Content/Textures/Particles/EntwinedBranchesFire").Value;

            CustomParticle p = new CustomParticle(
                tip + offset,
                offset * 0.05f, // 速度倍率。大きくするとパーティクルが速く飛ぶ
                Main.rand.NextFloat(1.0f, 2f), // 最大スケール。大きくするとパーティクルが大きくなる
                Main.rand.NextFloat(30f, 60f),  // ライフタイム（寿命）。短くすると速く消える
                tex,
                Color.White
            );

            CustomParticleHandler.Spawn(p);
        }


        public override bool PreDraw(ref Color lightColor)
        {

            CustomParticleHandler.DrawAll(Main.spriteBatch);

            // ムチ本体描画
            List<Vector2> pts = new();
            Projectile.FillWhipControlPoints(Projectile, pts);

            // Projectile.ai[0] を timer として使用
            // timer が大きくなるほど先端が最大スケールになる描画タイミングに影響
            SimpleWhipDrawer7.DrawSegments(Projectile, pts, Projectile.ai[0]);
            return false;
        }
    }
}