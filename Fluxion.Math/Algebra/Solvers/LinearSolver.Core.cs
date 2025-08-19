// Fluxion.Math/Algebra/Solvers/LinearSolver.Core.cs
using System;
using System.Collections.Generic;
using SMath = System.Math;
namespace Fluxion.Math.Algebra.Solvers
{
    /// <summary>
    /// Core implementation of the linear solver and helpers.
    /// </summary>
    public static partial class LinearSolver
    {
        /// <summary>
        /// Implementation for solving a*x + b = c*x + d with formatting options.
        /// Produces a narrated step list and either a unique value, none, or infinite solutions.
        /// </summary>
        public static partial LinearSolveResult Solve(double a, double b, double c, double d, SolveFormatOptions fmt)
        {
            var steps = new List<string>();

            // Step 1: normalize to A*x + B = 0
            double A = a - c;
            double B = b - d;

            steps.Add($"Move all terms to left side: ({a} - {c})x + ({b} - {d}) = 0");
            steps.Add($"Simplify: {A}x + {B} = 0");

            if (global::System.Math.Abs(A) < 1e-12)
            {
                if (global::System.Math.Abs(B) < 1e-12)
                {
                    steps.Add("Result: Infinite solutions (identity).");
                    return LinearSolveResult.Infinite(steps);
                }
                else
                {
                    steps.Add("Result: No solution (contradiction).");
                    return LinearSolveResult.None(steps);
                }
            }

            // Step 2: Solve for x
            double value = -B / A;

            // Optional: "exact" display as a simple fraction string (not a true rational reduction)
            string? exact = null;
            if (fmt.UseFractionsInSteps)
            {
                // NOTE: This is a display convenience only.
                // If you want reduced rationals, add a proper Fraction type later.
                exact = $"{-B}/{A}";
            }

            steps.Add($"Divide both sides by {A}: x = {-B}/{A}");
            steps.Add($"x ≈ {System.Math.Round(value, fmt.DecimalPlaces)}");

            return LinearSolveResult.Unique(value, exact, steps);
        }

        /// <summary>
        /// Returns the narrated steps for solving a*x + b = c*x + d (no result formatting needed).
        /// </summary>
        public static IReadOnlyList<string> Explain(double a, double b, double c, double d)
        {
            var result = Solve(a, b, c, d, new SolveFormatOptions());
            return result.Steps;
        }

        /// <summary>
        /// Solve many equations of the form a*x + b = c*x + d.
        /// </summary>
        public static IEnumerable<LinearSolveResult> SolveMany(IEnumerable<(double a, double b, double c, double d)> items)
        {
            if (items is null) yield break;

            var fmt = new SolveFormatOptions();
            foreach (var (a, b, c, d) in items)
                yield return Solve(a, b, c, d, fmt);
        }
    }
}
