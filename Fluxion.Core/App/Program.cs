// Fluxion.Core/App/Program.cs
using Fluxion.Features;                 // for Graph3DFeature
// Option A: fully qualify System.Math inside the lambda (most robust)
namespace Fluxion.Core.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Graph3DFeature.Surface(
                (x, y) => global::System.Math.Sin(x) * global::System.Math.Cos(y),
                xMin: -6, xMax: 6, yMin: -6, yMax: 6,
                resolution: 120, wireframe: true);
        }
    }
}
