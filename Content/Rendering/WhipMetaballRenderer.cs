using Microsoft.Xna.Framework;
using Terraria;
using System;
using CalamitySimpleWhipAddon.Content.Systems;

namespace CalamitySimpleWhipAddon.Content.Rendering
{
    public static class WhipMetaballRenderer
    {
        public static CurveData CurrentCurve;
        public static CurveData PreviousCurve;

        private static int tick;

        public static void Update(Projectile projectile, CurveData curve)
        {
            if (curve == null || curve.Points.Count < 2)
                return;

            CurrentCurve = curve;

            var body = ChargeMetaballData.Bodies;
            body.Clear();

            ChargeTheme theme =
                WhipChargeSystem.GetHeldTheme(Main.player[projectile.owner]);

            if (theme == ChargeTheme.None)
                return;

            tick++;

            var pts = curve.Points;
            float totalLen = curve.TotalLength;

            if (totalLen <= 0f)
                return;

            CurvePoint tipPoint = pts[^1];

            float tipSpeed =
            Utils.GetLerpValue(
            2f,
            20f,
            pts[^1].Velocity.Length(),
            true);



            for (int i = 1; i < pts.Count; i++)
            {
                var a = pts[i - 1];
                var b = pts[i];

                Vector2 seg = b.Position - a.Position;
                float len = seg.Length();
                if (len < 0.0001f)
                    continue;

                Vector2 dir = seg / len;
                Vector2 mid = (a.Position + b.Position) * 0.5f;
                Vector2 normal = new(-dir.Y, dir.X);

                // ★距離ベースprogress（ここ重要）
                float progress = b.Distance / totalLen;

                // =====================================================
                // メタボール本体
                // =====================================================
                float thickness =
                    MathHelper.Lerp(14f, 50f, progress);

                body.Add(new ChargeBody
                {
                    Theme = theme,

                    Center = mid + normal * 4f,
                    Size = thickness,
                    Life = 9999
                });

                // =====================================================
                // 発生
                // =====================================================

                Player owner = Main.player[projectile.owner];

                float distanceToPlayer = Vector2.Distance(a.Position, owner.MountedCenter);
                if (distanceToPlayer < 75f)
                    continue;

                float distance = MathHelper.Lerp(
                        a.Distance,
                        b.Distance,
                        Main.rand.NextFloat());

                Vector2 spawnPos =
                Vector2.Lerp(
                b.Position - b.Velocity * 3f,
                b.Position,
                Main.rand.NextFloat());

                float tip =
                Utils.GetLerpValue(
                0.15f,
                1f,
                progress,
                true);

                float tipFactor = MathF.Pow(tip, 10f);

                float amount =
                    tipFactor *
                    tipSpeed *
                    0.3f;

                int count = (int)amount;

                if (Main.rand.NextFloat() < amount - count)
                    count++;


                for (int j = 0; j < count; j++)
                {
                    SpawnLeak(theme, b, spawnPos, distance);
                }

            }

            PreviousCurve = CurrentCurve;
            CurrentCurve = curve;
        }

        private static void SpawnLeak(
            ChargeTheme theme,
            CurvePoint cp,
            Vector2 pos,
            float distance)
        {

            Vector2 moveDir = cp.Velocity;

            if (moveDir.LengthSquared() > 0.001f)
                moveDir.Normalize();
            else
                moveDir = cp.Tangent;

            if (moveDir != Vector2.Zero)
                moveDir.Normalize();
            else
                moveDir = cp.Tangent;

            Vector2 side =
                cp.Normal * Main.rand.NextFloat(-5f, 5f);

            Vector2 back =
                -moveDir * Main.rand.NextFloat(0f, 14f);

            Vector2 offset =
                side + back;

            float tip =
                cp.Progress;

            float power =
                MathHelper.Lerp(
                0.25f,
                1.0f,
                tip);

            Vector2 vel =
            -moveDir *
            Main.rand.NextFloat(3f, 12f) *
            power;

            vel +=
            cp.Normal *
            Main.rand.NextFloat(-2.4f, 2.4f) *
            power;

            float size =
                Main.rand.NextFloat(25f, 35f);

            int life = Main.rand.Next(15, 25);

            ChargeMetaballData.Particles.Add(new ChargeParticle
            {
                Theme = theme,

                Distance = distance,
                Center = pos,
                Offset = offset,

                Size = size,

                Life = life,
                MaxLife = life,

                Stick = 1f,

                Velocity = vel
            });
        }

        public static void Clear()
        {
            CurrentCurve = null;
            ChargeMetaballData.Bodies.Clear();
            ChargeMetaballData.Particles.Clear();
        }
    }
}
