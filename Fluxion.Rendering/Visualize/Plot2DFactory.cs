using System;
using System.Numerics;   // for Vector2
using Fluxion.Rendering.Visualize;

namespace Fluxion.Rendering.Visualize
{
    public static class Plot2DFactory
    {
        /// <summary>
        /// Generate a sampled plot of y = f(x) between [xMin, xMax].
        /// </summary>
        public static Plot2D Function(Func<double, double> f, double xMin, double xMax, int samples = 100, PlotStyle? style = null)
        {
            if (samples < 2) samples = 2;

            var plot = new Plot2D(style ?? new PlotStyle { Lines = true, Points = false });

            double step = (xMax - xMin) / (samples - 1);
            for (int i = 0; i < samples; i++)
            {
                double x = xMin + i * step;
                double y = f(x);
                plot.Points.Add(new Vector2((float)x, (float)y));
            }

            return plot;
        }
    }
}
