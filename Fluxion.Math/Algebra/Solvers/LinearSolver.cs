// Fluxion.Math/Algebra/Solvers/LinearSolver.cs
using System;

namespace Fluxion.Math.Algebra.Solvers
{
    public static class LinearSolver
    {
        /// <summary>Solves a1*x + a0 = 0. Returns NaN if no single real root.</summary>
        public static double Solve(double a1, double a0)
        {
            if (a1 == 0) return double.NaN; // no solution or infinite; caller decides
            return -a0 / a1;
        }
    }
}
