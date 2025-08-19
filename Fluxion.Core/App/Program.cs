// Fluxion.Core/App/Program.cs
using Fluxion.Features.Algebra;

namespace Fluxion.Core.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Linear equation: 5x - 15 = 0 → x = 3
            LinearFeature.SolveAndPrint(A: 5, B: -15);

            // Simple graph between -2 and 6
            LinearFeature.Graph(A: 5, B: -15, xMin: -2, xMax: 6);
        }
    }
}
