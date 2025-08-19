using System;

namespace Fluxion.Math.Algebra.Solvers
{
    /// <summary>
    /// Provides methods for solving quadratic equations of the form ax² + bx + c = 0.
    /// </summary>
    public static class QuadraticSolver
    {
        /// <summary>
        /// Solves ax² + bx + c = 0 using the quadratic formula.
        /// </summary>
        /// <param name="a">Coefficient of x² (must not be 0)</param>
        /// <param name="b">Coefficient of x</param>
        /// <param name="c">Constant term</param>
        /// <returns>
        /// A tuple with two roots (double.NaN if no real solution).
        /// </returns>
        /// <summary>
        /// Returns the two real roots if they exist; otherwise (NaN, NaN).
        /// Throws if a == 0 (not quadratic).
        /// </summary>
        public static (double x1, double x2) Solve(double a, double b, double c)
        {
            if (a == 0)
                throw new ArgumentException("Coefficient 'a' must not be zero for a quadratic equation.", nameof(a));

            double d = b * b - 4 * a * c;
            if (d < 0) return (double.NaN, double.NaN);

            double sqrtD = System.Math.Sqrt(d);
            double denom = 2 * a;

            return ((-b + sqrtD) / denom, (-b - sqrtD) / denom);
        }
    }
}
