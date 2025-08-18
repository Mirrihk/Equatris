// Fluxion.Core/App/Program.cs
using Fluxion.Math.Algebra;
using Fluxion.Rendering.Draw;
using Fluxion.Rendering.Visualize;
using Fluxion.Rendering.Windowing;
using OpenTK.Mathematics;

namespace Fluxion.Core.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using var window = new FluxWindow(1280, 720, "Fluxion Engine");

            // Example: plot y = ax^2 + bx + c
            var quadratic = Quadratic.FromCoefficients(1, 0, 0);
            var plot = Plot2DFactory.Function(quadratic.Evaluate, -2, 2);

            var axes = new AxesRenderer();
            var curve = new CurveRenderer2D();

            window.OnDraw = dt =>
            {
                var projection = Matrix4.CreateOrthographicOffCenter(-2, 2, -2, 2, -1, 1);

                axes.DrawAxes(projection);
                curve.Draw(plot, projection);
            };

            window.Run();
        }
    }
}
