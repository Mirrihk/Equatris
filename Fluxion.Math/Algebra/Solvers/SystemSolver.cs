// Fluxion.Math/Algebra/Solvers/SystemSolver.cs
using System;

namespace Fluxion.Math.Algebra.Solvers
{
    /// <summary>
    /// Solves systems of linear equations.
    /// </summary>
    public static class SystemSolver
    {
        /// <summary>
        /// Solves a 2x2 system of linear equations:
        /// a1*x + b1*y = c1
        /// a2*x + b2*y = c2
        /// </summary>
        /// <returns>Tuple (x, y) with the solution</returns>
        public static (double x, double y) Solve2x2(
            double a1, double b1, double c1,
            double a2, double b2, double c2)
        {
            double det = a1 * b2 - a2 * b1;
            if (System.Math.Abs(det) < 1e-10)
                throw new InvalidOperationException("System has no unique solution (determinant is zero).");

            double x = (c1 * b2 - c2 * b1) / det;
            double y = (a1 * c2 - a2 * c1) / det;

            return (x, y);
        }
    }
}
