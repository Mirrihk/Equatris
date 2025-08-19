using System;

namespace Fluxion.Math.Algebra
{
    /// <summary>
    /// Represents a polynomial with real coefficients.
    /// Coefficients are stored in ascending order: 
    /// coeffs[0] = constant, coeffs[1] = x, coeffs[2] = x², etc.
    /// </summary>
    public class Polynomial
    {
        /// <summary>Coefficients of the polynomial (lowest degree first).</summary>
        public double[] Coefficients { get; }

        /// <summary>Create polynomial with given coefficients (ascending order).</summary>
        public Polynomial(params double[] coeffs)
        {
            if (coeffs == null || coeffs.Length == 0)
                throw new ArgumentException("Polynomial must have at least one coefficient.");
            Coefficients = coeffs;
        }

        /// <summary>
        /// Evaluate polynomial at x using Horner’s method.
        /// </summary>
        public double Evaluate(double x)
        {
            double result = 0;
            for (int i = Coefficients.Length - 1; i >= 0; i--)
            {
                result = result * x + Coefficients[i];
            }
            return result;
        }

        /// <summary>Degree of polynomial (highest non-zero coefficient index).</summary>
        public int Degree => Coefficients.Length - 1;

        public override string ToString()
        {
            return string.Join(" + ",
                Coefficients.Select((c, i) => $"{c}x^{i}"));
        }
    }
}
