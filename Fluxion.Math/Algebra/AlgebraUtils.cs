// Fluxion.Math/Algebra/AlgebraUtils.cs
using System;

namespace Fluxion.Math.Algebra
{
    /// <summary>
    /// Common helper methods for algebraic operations.
    /// </summary>
    public static class AlgebraUtils
    {
        /// <summary>
        /// Computes the discriminant of a quadratic equation ax² + bx + c.
        /// </summary>
        public static double Discriminant(double a, double b, double c)
        {
            return (b * b) - (4 * a * c);
        }

        /// <summary>
        /// Greatest common divisor (Euclidean algorithm).
        /// </summary>
        public static int Gcd(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return System.Math.Abs(a);
        }

        /// <summary>
        /// Least common multiple.
        /// </summary>
        public static int Lcm(int a, int b)
        {
            return System.Math.Abs(a * b) / Gcd(a, b);
        }

        public static bool NearlyEqual( double a, double b, double eps = 1e-9)
        {
            if (a.Equals(b)) return true;

            double diff = System.Math.Abs(a - b);
            double norm = System.Math.Min((System.Math.Abs(a) + System.Math.Abs(b)), double.MaxValue);

            return diff < System.Math.Max(eps, eps * norm);
        }
    }
}
