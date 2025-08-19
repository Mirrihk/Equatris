namespace Fluxion.Math.Algebra.Equations
{
    public class Linear
    {
        public double A { get; }
        public double B { get; }

        public Linear (double a, double b)
        {
            A = a;
            B = b;
        }

        public double Evaluate(double x) => A * x + B;
    }
}
