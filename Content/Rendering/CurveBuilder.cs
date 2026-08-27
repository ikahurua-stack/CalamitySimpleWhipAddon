using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Collections.Generic;

namespace CalamitySimpleWhipAddon.Content.Rendering
{
    public struct CurvePoint
    {
        public Vector2 Position;
        public Vector2 Tangent;
        public Vector2 Normal;

        public float Distance;
        public float Progress;

        public Vector2 Velocity;

    }

    public sealed class CurveData
    {
        public readonly List<CurvePoint> Points = new();

        public readonly List<Vector2> PreviousPositions = new();

        public float TotalLength;

        public Vector2 AverageVelocity;
    }

    public static class CurveBuilder
    {
        // =========================================================
        // Public Build Entry
        // =========================================================

        public static CurveData Build(Projectile projectile, int samplesPerSegment = 6)
        {
            List<Vector2> controls = new();
            Projectile.FillWhipControlPoints(projectile, controls);
            return Build(controls, samplesPerSegment);
        }

        public static CurveData Build(List<Vector2> controls, int samplesPerSegment = 6)
        {
            CurveData data = new();

            if (controls == null || controls.Count < 2)
                return data;

            List<Vector2> smooth = BuildSmoothCurve(controls, samplesPerSegment);

            // ---------------------------------------------------------
            // Length calculation
            // ---------------------------------------------------------
            float totalLength = 0f;
            for (int i = 1; i < smooth.Count; i++)
                totalLength += Vector2.Distance(smooth[i - 1], smooth[i]);

            data.TotalLength = totalLength;

            float currentDistance = 0f;

            // ---------------------------------------------------------
            // Build curve points
            // ---------------------------------------------------------
            for (int i = 0; i < smooth.Count; i++)
            {
                Vector2 pos = smooth[i];

                Vector2 tangent;
                if (i == 0)
                    tangent = smooth[i + 1] - smooth[i];
                else if (i == smooth.Count - 1)
                    tangent = smooth[i] - smooth[i - 1];
                else
                    tangent = smooth[i + 1] - smooth[i - 1];

                if (tangent != Vector2.Zero)
                    tangent.Normalize();

                Vector2 normal = new(-tangent.Y, tangent.X);

                if (i > 0)
                    currentDistance += Vector2.Distance(smooth[i - 1], pos);

                float progress = totalLength > 0f ? currentDistance / totalLength : 0f;


                Vector2 velocity = Vector2.Zero;

                if (WhipMetaballRenderer.PreviousCurve != null)
                {
                    CurvePoint old =
                       Sample(
                           WhipMetaballRenderer.PreviousCurve,
                           progress *
                           WhipMetaballRenderer.PreviousCurve.TotalLength);

                    velocity = pos - old.Position;
                }
            

                data.Points.Add(new CurvePoint
                {
                    Position = pos,
                    Tangent = tangent,
                    Normal = normal,
                    Distance = currentDistance,
                    Progress = progress,
                    Velocity = velocity,
                });

                data.PreviousPositions.Add(pos);
            }

            return data;
        }

        // =========================================================
        // Catmull-Rom smoothing
        // =========================================================

        private static List<Vector2> BuildSmoothCurve(List<Vector2> controls, int steps)
        {
            List<Vector2> result = new();

            for (int i = 0; i < controls.Count - 1; i++)
            {
                Vector2 p0 = i == 0 ? controls[i] : controls[i - 1];
                Vector2 p1 = controls[i];
                Vector2 p2 = controls[i + 1];
                Vector2 p3 = i + 2 < controls.Count ? controls[i + 2] : controls[^1];

                for (int j = 0; j < steps; j++)
                {
                    float t = j / (float)steps;

                    result.Add(Vector2.CatmullRom(p0, p1, p2, p3, t));
                }
            }

            result.Add(controls[^1]);
            return result;
        }

        
        // =========================================================
        // Sampling (READ ONLY)
        // =========================================================

        public static CurvePoint Sample(CurveData data, float distance)
        {
            if (data == null || data.Points.Count == 0)
                return default;

            if (distance <= 0)
                return data.Points[0];

            if (distance >= data.TotalLength)
                return data.Points[^1];

            var points = data.Points;

            for (int i = 1; i < points.Count; i++)
            {
                var a = points[i - 1];
                var b = points[i];

                if (distance <= b.Distance)
                {
                    float t =
                        (distance - a.Distance) /
                        (b.Distance - a.Distance);

                    CurvePoint result = new();

                    result.Position = Vector2.Lerp(a.Position, b.Position, t);
                    result.Tangent = Vector2.Lerp(a.Tangent, b.Tangent, t);

                    if (result.Tangent != Vector2.Zero)
                        result.Tangent.Normalize();

                    result.Normal = new(-result.Tangent.Y, result.Tangent.X);

                    result.Distance = distance;
                    result.Progress = distance / data.TotalLength;

                    result.Velocity =
                        Vector2.Lerp(a.Velocity, b.Velocity, t);

                    return result;
                }
            }

            return data.Points[^1];
        }
    }
}