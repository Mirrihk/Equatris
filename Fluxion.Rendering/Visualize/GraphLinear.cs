using System.Numerics;
using System.Runtime.InteropServices;
using Fluxion.Rendering.Draw;

namespace Fluxion.Rendering.Visualize
{
    public static class GraphLinear
    {
        public static void GraphEquation(IRenderer renderer, double a, double b, double c, double d,
                                         double solution, double range = 2.0, int samples = 200)
        {
            if (double.IsNaN(solution) || double.IsInfinity(solution)) return;

            var left = new System.Collections.Generic.List<Vector2>(samples + 1);
            var right = new System.Collections.Generic.List<Vector2>(samples + 1);

            double xMin = solution - range;
            double xMax = solution + range;
            double step = (xMax - xMin) / samples;

            // First pass: gather world points & compute y-extents
            float yMin = float.PositiveInfinity, yMax = float.NegativeInfinity;
            for (int i = 0; i <= samples; i++)
            {
                double x = xMin + i * step;
                float yL = (float)(a * x + b);
                float yR = (float)(c * x + d);
                var pL = new Vector2((float)x, yL);
                var pR = new Vector2((float)x, yR);
                left.Add(pL);
                right.Add(pR);
                if (yL < yMin) yMin = yL; if (yL > yMax) yMax = yL;
                if (yR < yMin) yMin = yR; if (yR > yMax) yMax = yR;
            }

            // Expand bounds a bit for padding
            float padY = (yMax - yMin) * 0.1f + 1e-3f;
            yMin -= padY; yMax += padY;

            // Map world → NDC
            static float Lerp(float a0, float a1, float t) => a0 + (a1 - a0) * t;
            static float InvLerp(float a0, float a1, float v) => (v - a0) / (a1 - a0);

            float xMinF = (float)xMin, xMaxF = (float)xMax;

            void NormalizeList(System.Collections.Generic.List<Vector2> pts)
            {
                for (int i = 0; i < pts.Count; i++)
                {
                    var p = pts[i];
                    float nx = Lerp(-1f, 1f, InvLerp(xMinF, xMaxF, p.X));
                    float ny = Lerp(-1f, 1f, InvLerp(yMin, yMax, p.Y));
                    pts[i] = new Vector2(nx, ny);
                }
            }

            NormalizeList(left);
            NormalizeList(right);

            // Draw lines
            renderer.DrawLines(CollectionsMarshal.AsSpan(left), 2f);
            renderer.DrawLines(CollectionsMarshal.AsSpan(right), 2f);

            // Draw solution point
            float sx = (float)solution;
            float sy = (float)(a * solution + b);
            float snx = Lerp(-1f, 1f, InvLerp(xMinF, xMaxF, sx));
            float sny = Lerp(-1f, 1f, InvLerp(yMin, yMax, sy));
            var dot = new Vector2(snx, sny);
            renderer.DrawPoints(new System.ReadOnlySpan<Vector2>(new[] { dot }), 8f);
        }
    }
}
