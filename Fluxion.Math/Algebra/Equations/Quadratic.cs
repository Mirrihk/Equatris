// Fluxion.Math/Algebra/Quadratic.cs
namespace Fluxion.Math.Algebra
{
    /// <summary>y = a x^2 + b x + c</summary>
    public sealed class Quadratic
    {
        public double A { get; }
        public double B { get; }
        public double C { get; }

        private Quadratic(double a, double b, double c)
        { A = a; B = b; C = c; }

        public static Quadratic FromCoefficients(double a, double b, double c)
            => new Quadratic(a, b, c);

        public double Evaluate(double x) => A * x * x + B * x + C;
    }
}
