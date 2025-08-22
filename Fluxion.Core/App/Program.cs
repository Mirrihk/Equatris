// Fluxion.Core/App/Program.cs
using Fluxion.Features.Algebra;
using Fluxion.Math.Algebra.Solvers;
using Fluxion.Rendering.Draw;
using Fluxion.Rendering.Visualize;

namespace Fluxion.Core.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ///Same as before
            //LinearFeature.SolveAndPrint(A: 5, B: -15);
            LinearFeature.Graph(A: 78, B: 20, xMin: -2, xMax: 6);

            // NEW: show steps (single-side)
            //LinearFeature.ShowSteps(A: 5, B: -15);

            // NEW: show steps (two-sided): 12.6 + 4x = 9.6 + 8x
            //LinearFeature.ShowSteps(a: 7, b: 0.4, c: 6, d: 1.4);


            // Create an instance of a renderer

        }
    }
}