// Fluxion.Math/Algebra/Solvers/LinearSolver.cs
using System;
using Fluxion.Math.Algebra.Equations;

namespace Fluxion.Math.Algebra.Solvers
{
    public static class LinearSolver
    {
        /// <summary>
        /// Solve Ax + B = 0. Returns NaN if no unique solution.
        /// </summary>
        public static double Solve(Linear eq, double eps = 1e-12)
        {
            if (System.Math.Abs(eq.A) < eps)
                return double.NaN; // no solution or infinite solutions
            return -eq.B / eq.A;
        }
    }
}
