// Fluxion.Features/Algebra/LinearFeature.cs
using System;
using Fluxion.Math.Algebra.Equations;       // Linear
using Fluxion.Math.Algebra.Solvers;         // LinearSolver
using Fluxion.Rendering.Draw;               // AxesRenderer, CurveRenderer2D
using Fluxion.Rendering.Visualize;          // Plot2DFactory
using Fluxion.Rendering.Windowing;          // FluxWindow
using OpenTK.Mathematics;                   // Matrix4

namespace Fluxion.Features.Algebra
{
    /// <summary>
    /// Runnable helpers for Linear equations: Ax + B = 0
    /// Keep Program.cs clean by calling these one-liners.
    /// </summary>
    public static class LinearFeature
    {
        public static void SolveAndPrint(double A, double B)
        {
            var eq = new Linear(A, B);
            double x = LinearSolver.Solve(eq);

            Console.WriteLine($"\nLinear: {A}*x + {B} = 0");
            Console.WriteLine(double.IsNaN(x)
                ? "Solution: No unique solution (A≈0)."
                : $"Solution: x = {x}");
            Console.WriteLine($"Check: A*x + B = {eq.Evaluate(x)} (should be ~0)");
        }

        public static void Graph(double A, double B, double xMin, double xMax, int samples = 256)
        {
            using var window = new FluxWindow(1000, 600, "Fluxion – Linear f(x) = Ax + B");
            using var axes = new AxesRenderer();
            using var curve = new CurveRenderer2D();

            Func<double, double> f = x => A * x + B;
            var plot = Plot2DFactory.Function(f, xMin, xMax, samples);

            window.OnDraw = dt =>
            {
                var projection = Matrix4.CreateOrthographicOffCenter(
                    (float)xMin, (float)xMax, -10f, 10f, -1f, 1f);
                axes.DrawAxes(projection);
                curve.Draw(plot, projection);
            };

            window.Run();
        }
    }
}
