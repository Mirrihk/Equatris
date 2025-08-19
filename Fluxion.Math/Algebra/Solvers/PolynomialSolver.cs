// Fluxion.Math/Algebra/Solvers/PolynomialSolver.cs
using System;
using System.Collections.Generic;
using Fluxion.Math.Algebra;

namespace Fluxion.Math.Algebra.Solvers
{
    /// <summary>Solves real polynomials for real roots (degree ≤ 2 for now).</summary>
    public static class PolynomialSolver
    {
        /// <summary>
        /// Returns all real roots of the polynomial.
        /// For degree 1 and 2 only; throws NotSupportedException for higher degree.
        /// </summary>
        public static IReadOnlyList<double> Solve(Polynomial p)
        {
            var a = p.Coefficients; // ascending order: a0 + a1 x + a2 x^2 + ...
            int deg = p.Degree;

            switch (deg)
            {
                case 0:
                    // a0 = 0 → infinite solutions, else no solution. Return empty set for both.
                    return Array.Empty<double>();

                case 1:
                    {
                        // a0 + a1 x = 0 → x = -a0/a1  (a1 must != 0)
                        double a1 = a[1], a0 = a[0];
                        if (a1 == 0) return Array.Empty<double>(); // no solution or infinite; return empty
                        return new[] { -a0 / a1 };
                    }

                case 2:
                    {
                        // a0 + a1 x + a2 x^2 = 0 → QuadraticSolver with (a2, a1, a0)
                        double a2 = a[2], a1 = a[1], a0 = a[0];
                        if (a2 == 0)
                        {
                            // Degenerated to linear
                            if (a1 == 0) return Array.Empty<double>();
                            return new[] { -a0 / a1 };
                        }

                        var (x1, x2) = QuadraticSolver.Solve(a2, a1, a0);
                        var roots = new List<double>(2);
                        if (!double.IsNaN(x1)) roots.Add(x1);
                        if (!double.IsNaN(x2) && x2 != x1) roots.Add(x2);
                        return roots;
                    }

                default:
                    throw new NotSupportedException($"Polynomial degree {deg} not supported yet.");
            }
        }
    }
}
