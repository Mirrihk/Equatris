using System.Numerics;
using System.Collections.Generic;

namespace Fluxion.Rendering.Visualize
{
    public sealed record PlotStyle(bool Lines = true, bool Points = false, float Width = 2f, Vector3? Rgb = null);

    /// <summary>Device-agnostic 2D plot (polyline/points) + style.</summary>
    public sealed record Plot2D(IReadOnlyList<Vector2> Points, PlotStyle Style);

    public static class Plot2DFactory
    {
        /// <summary>Sample f(x) on [xMin, xMax] into a polyline.</summary>
        public static Plot2D Function(System.Func<double, double> f, double xMin, double xMax, int samples = 512, PlotStyle? style = null)
        {
            if (samples < 2) samples = 2;
            var pts = new Vector2[samples];
            double step = (xMax - xMin) / (samples - 1);

            for (int i = 0; i < samples; i++)
            {
                double x = xMin + i * step;
                pts[i] = new Vector2((float)x, (float)f(x));
            }

            return new Plot2D(pts, style ?? new PlotStyle(Lines: true, Points: false, Width: 2f, Rgb: new Vector3(0.2f, 0.8f, 0.3f)));
        }

        /// <summary>Build a plot from existing points.</summary>
        public static Plot2D Polyline(IEnumerable<Vector2> points, PlotStyle? style = null)
        {
            var list = points is List<Vector2> l ? l : new List<Vector2>(points);
            return new Plot2D(list, style ?? new PlotStyle());
        }
    }
}
