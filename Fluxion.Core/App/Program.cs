// Fluxion.Core/App/Program.cs
using System;
using Fluxion.Math.Algebra;              // Polynomial, AlgebraUtils, Quadratic (if you kept it)
using Fluxion.Math.Algebra.Solvers;      // QuadraticSolver, PolynomialSolver, SystemSolver
using Fluxion.Rendering.Draw;            // AxesRenderer, CurveRenderer2D
using Fluxion.Rendering.Visualize;       // Plot2DFactory
using Fluxion.Rendering.Windowing;       // FluxWindow
using OpenTK.Mathematics;                // Matrix4

namespace Fluxion.Core.App
{
    public static class Program
    {
        // Flip this to false when you want to run the graphics window instead.
        private const bool RUN_MATH_TESTS = true;

        public static void Main(string[] args)
        {
            using var window = new FluxWindow(1280, 720, "Fluxion Engine");
            
            
            var quadratic = Quadratic.FromCoefficients(1, 0, 0);               // y = x^2
            // ---------- (Optional) graphics demo if you set RUN_MATH_TESTS = false ----------

            var plot = Plot2DFactory.Function(quadratic.Evaluate, -2, 2);     // simple plot

            var axes = new AxesRenderer();
            var curve = new CurveRenderer2D();

            window.OnDraw = dt =>
            {
                var projection = Matrix4.CreateOrthographicOffCenter(-2f, 2f, -2f, 2f, -1f, 1f);
                axes.DrawAxes(projection);
                curve.Draw(plot, projection);
            };
            
            window.Run();

            if (RUN_MATH_TESTS)
            {
                RunMathSmokeTests();
                Console.WriteLine("\nDone. Press any key to exit...");
                Console.ReadKey();
                return;
            }


        }

        private static void RunMathSmokeTests()
        {
            Console.WriteLine("=== Fluxion Algebra Smoke Tests ===\n");

            // 1) Quadratic: x^2 - 3x + 2 = 0 -> roots {1, 2}
            //    NOTE: QuadraticSolver takes (a,b,c) in standard form.
            var (q1, q2) = QuadraticSolver.Solve(1, -3, 2);
            Console.WriteLine("Quadratic: x^2 - 3x + 2 = 0");
            Console.WriteLine($"  Roots: x1 = {q1}, x2 = {q2}");
            Console.WriteLine($"  Δ (via AlgebraUtils) = {AlgebraUtils.Discriminant(1, -3, 2)}\n");

            // 2) Polynomial: coefficients are ASCENDING order -> a0 + a1 x + a2 x^2
            //    For x^2 - 3x + 2, use new Polynomial(2, -3, 1)
            var p = new Polynomial(2, -3, 1);
            var roots = PolynomialSolver.Solve(p);
            Console.WriteLine("Polynomial: x^2 - 3x + 2");
            Console.WriteLine("  Roots: " + string.Join(", ", roots));
            Console.WriteLine("  Evaluate at x=2: " + p.Evaluate(2)); // should be 0
            Console.WriteLine();

            // 3) Linear system (2x2):
            //    2x + y = 5
            //    x  - y = 1
            var (x, y) = SystemSolver.Solve2x2(2, 1, 5, 1, -1, 1);
            Console.WriteLine("System (2x2):");
            Console.WriteLine("  2x + y = 5");
            Console.WriteLine("   x - y = 1");
            Console.WriteLine($"  Solution: x = {x}, y = {y}\n");

            // 4) Edge case: quadratic with no real roots (x^2 + 1 = 0)
            var (nr1, nr2) = QuadraticSolver.Solve(1, 0, 1);
            Console.WriteLine("Quadratic (no real roots): x^2 + 1 = 0");
            Console.WriteLine($"  Roots: x1 = {nr1}, x2 = {nr2}  (NaN indicates non‑real)");

        }

    }
}
