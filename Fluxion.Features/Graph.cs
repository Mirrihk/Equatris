// Fluxion.Features/Graph.cs
using System;
using static System.Math;

namespace Fluxion.Features
{
    /// <summary>How to lift a 1D function f(x) into a 3D surface z = g(x, y).</summary>
    public enum Lift
    {
        /// z = f(x)  (extrude along y)
        Extrude,
        /// z = f(sqrt(x^2 + y^2))  (radial lift, great for ripples)
        Radial,
        /// z = f(x) * cos(y)  (nice for trig grids)
        ProductCos,
        /// z = f(x) * sin(y)
        ProductSin
    }

    public static class Graph
    {
        /// <summary>Start from a 1D function f(x).</summary>
        public static Graph1D F(Func<double, double> fx) => new(fx);

        /// <summary>Start from a 2D scalar field g(x,y).</summary>
        public static Graph2D F2(Func<double, double, double> gxy) => new(gxy);
    }

    public readonly struct Graph1D
    {
        private readonly Func<double, double> f;
        public Graph1D(Func<double, double> f) => this.f = f;

        /// <summary>Plot f(x) in 2D.</summary>
        public void TwoD(double xMin, double xMax, int samples = 1000)
        {
            // FIX: copy instance member to a local so we don't capture 'this' in any closures
            var fLocal = f;
            Graph2DFeature.Function(fLocal, xMin, xMax, samples);
        }

        /// <summary>Plot the same f(x) as a 3D surface via a lift.</summary>
        public void ThreeD(
            double xMin, double xMax,
            double yMin, double yMax,
            Lift lift = Lift.ProductCos,
            int resolution = 120, bool wireframe = false)
        {
            // FIX: copy instance member to a local so lambdas capture the local (not 'this')
            var fLocal = f;

            Func<double, double, double> g = lift switch
            {
                Lift.Extrude => (x, y) => fLocal(x),
                Lift.Radial => (x, y) => fLocal(Sqrt(x * x + y * y)),
                Lift.ProductCos => (x, y) => fLocal(x) * Cos(y),
                Lift.ProductSin => (x, y) => fLocal(x) * Sin(y),
                _ => (x, y) => fLocal(x)
            };

            Graph3DFeature.Surface(g, xMin, xMax, yMin, yMax, resolution, wireframe);
        }
    }

    public readonly struct Graph2D
    {
        private readonly Func<double, double, double> g;
        public Graph2D(Func<double, double, double> g) => this.g = g;

        /// <summary>Plot a 2D scalar field g(x,y) as a 3D surface.</summary>
        public void ThreeD(
            double xMin, double xMax,
            double yMin, double yMax,
            int resolution = 120, bool wireframe = false)
        {
            // (Not strictly necessary here, but future-proof if you add lambdas)
            var gLocal = g;
            Graph3DFeature.Surface(gLocal, xMin, xMax, yMin, yMax, resolution, wireframe);
        }
    }
}
