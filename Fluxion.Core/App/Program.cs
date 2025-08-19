// Fluxion.Core/App/Program.cs
using Fluxion.Features.Algebra;

namespace Fluxion.Core.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Same as before
            //LinearFeature.SolveAndPrint(A: 5, B: -15);
           // LinearFeature.Graph(A: 5, B: -15, xMin: -2, xMax: 6);

            // NEW: show steps (single-side)
            // LinearFeature.ShowSteps(A: 5, B: -15);

            // NEW: show steps (two-sided): 12.6 + 4x = 9.6 + 8x
            LinearFeature.ShowSteps(a: 4, b: 12.6, c: 8, d: 9.6);
        }
    }
}
 