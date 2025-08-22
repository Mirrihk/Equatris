using System;
using static System.Math;
using Fluxion.Features;

namespace Fluxion.Core.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // 2D
            Graph2DFeature.Function(System.Math.Sin, -2 * System.Math.PI, 2 * System.Math.PI);

            // 3D (same equation, lifted)
            Graph3DFeature.Surface((x, y) => System.Math.Sin(x) * System.Math.Cos(y),
                                   -2 * System.Math.PI, 2 * System.Math.PI, -2 * System.Math.PI, 2 * System.Math.PI,
                                   resolution: 180, wireframe: true);

        }
    }
}
