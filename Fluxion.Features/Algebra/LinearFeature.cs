// Fluxion.Features/Algebra/LinearFeature.cs
using Fluxion.Math.Algebra.Equations;   // Linear
using Fluxion.Math.Algebra.Solvers;     // LinearSolver
using Fluxion.Rendering.Windowing;      // FluxWindow
using Fluxion.Rendering.Draw;           // OpenGLRenderer, IRenderer
using Fluxion.Rendering.Visualize;      // GraphLinear

namespace Fluxion.Features.Algebra
{
    /// Runnable helpers for Linear equations: Ax + B = 0  and  a*x + b = c*x + d (steps).
    /// Keeps Program.cs clean by exposing one-liners.
    public static class LinearFeature
    {
        /// Solve A*x + B = 0 and print the answer (+ quick check).
        public static void SolveAndPrint(double A, double B, int decimals = 4)
        {
            var eq = new Linear(A, B);
            double x = LinearSolver.Solve(eq);

            Console.WriteLine($"\nLinear: {A}*x + {B} = 0");
            if (double.IsNaN(x))
            {
                Console.WriteLine("Solution: No unique solution (A ≈ 0).");
                return;
            }

            Console.WriteLine($"Solution: x = {System.Math.Round(x, decimals)}");
            Console.WriteLine($"Check: A*x + B = {eq.Evaluate(x)} (should be ~0)");
        }

        /// Show  steps for A*x + B = 0 (single-side linear).
        public static void ShowSteps(double A, double B, int decimals = 4)
        {
            Console.WriteLine($"\nSolve {A}x + {B} = 0");
            if (System.Math.Abs(A) < 1e-12)
            {
                Console.WriteLine(System.Math.Abs(B) < 1e-12
                    ? "• Any x works (identity)."
                    : "• No solution (contradiction).");
                return;
            }

            Console.WriteLine($"• Subtract {B} from both sides → {A}x = {-B}");
            Console.WriteLine($"• Divide both sides by {A} → x = {-B}/{A}");
            double x = -B / A;
            Console.WriteLine($"• x ≈ {System.Math.Round(x, decimals)}");
        }

        /// Show steps for a two-sided linear equation a*x + b = c*x + d.
        /// Uses LinearSolver for narration and result.
        public static void ShowSteps(double a, double b, double c, double d, int decimals = 4)
        {
            Console.WriteLine($"\nSolve {a}x + {b} = {c}x + {d}");
            var res = LinearSolver.Solve(a, b, c, d);
            foreach (var s in res.Steps) Console.WriteLine($"• {s}");

            switch (res.Kind)
            {
                case LinearSolver.SolutionKind.Unique:
                    Console.WriteLine($"• Final: x = {res.Exact ?? System.Math.Round(res.Value!.Value, decimals).ToString()}");
                    break;
                case LinearSolver.SolutionKind.Infinite:
                    Console.WriteLine("• Final: Infinite solutions.");
                    break;
                case LinearSolver.SolutionKind.None:
                    Console.WriteLine("• Final: No solution.");
                    break;
            }
        }

        /// <summary>
        /// Graph y = A*x + B on [xMin, xMax].
        /// Renders with FluxWindow + OpenGLRenderer. Also overlays y = 0 for reference.
        /// </summary>
        public static void Graph(double A, double B, double xMin, double xMax)
        {
            // Choose a center/range for the view and an x to highlight.
            double center = 0.5 * (xMin + xMax);
            double range = 0.5 * (xMax - xMin);
            double xSolve = (System.Math.Abs(A) < 1e-12) ? center : (-B / A);   // if no unique root, center the view

            using var window = new FluxWindow(1000, 600, $"Fluxion – y = {A}x + {B}");
            OpenGLRenderer? gl = null;

            window.OnDraw = _ =>
            {
                gl ??= new OpenGLRenderer();
                gl.Clear(0.08f, 0.08f, 0.10f, 1f);

                // Graph the target line (left) vs y = 0 (right) and mark the root.
                // GraphLinear will normalize world coords to NDC and draw both lines.
                GraphLinear.GraphEquation(
                    renderer: gl,
                    a: A, b: B,       // left: y = A*x + B
                    c: 0, d: 0,       // right: y = 0 as a reference axis
                    solution: xSolve,
                    range: range,
                    samples: 300);
            };

            window.Run();
        }
    }
}
