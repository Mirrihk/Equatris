// Fluxion.Core/App/Program.cs
using Fluxion.Features;

namespace Fluxion.Core.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // 1) 2D function: y = sin(x)
            Graph.F(global::System.Math.Sin)
                 .TwoD(-2 * global::System.Math.PI, 2 * global::System.Math.PI, samples: 1200);

            // 2) 3D surface from the SAME function via a lift (radial ripples)
            Graph.F(global::System.Math.Sin)
                 .ThreeD(-8, 8, -8, 8, lift: Lift.Radial, resolution: 180, wireframe: false);

            // 3) 3D line from ANY function: y = sin(x) on the XY plane (Z = 0)
            Graph.F(global::System.Math.Sin)
                 .ThreeDLine(-2 * global::System.Math.PI, 2 * global::System.Math.PI,
                             plane: EmbedPlane.XY, offset: 0, samples: 1000);

            // 4) 2D straight line: y = m x + b
            Graph.Line(m: 1.2, b: -0.5)
                 .TwoD(-10, 10, samples: 600);

            // 5) 3D straight line on the XZ plane, Y fixed to 1.0
            Graph.Line(1.2, -0.5)
                 .ThreeD(-10, 10, plane: EmbedPlane.XZ, offset: 1.0, samples: 600);

            // 6) 3D polyline from raw points (no equation), on XY plane at Z=0
            Graph.Points(
                (-3.0, -1.2), (-2.0, -0.2), (-1.0, 0.8),
                (0.0, 0.0), (1.0, 1.1), (2.0, 1.7), (3.0, 2.2)
            ).ThreeDLine(plane: EmbedPlane.XY, offset: 0);

            // 7) Direct 3D surface: z = sin(x) * cos(y)
            Graph3DFeature.Surface(
                (x, y) => global::System.Math.Sin(x) * global::System.Math.Cos(y),
                -2 * global::System.Math.PI, 2 * global::System.Math.PI,
                -2 * global::System.Math.PI, 2 * global::System.Math.PI,
                resolution: 180, wireframe: true);

            // 8) 3D parametric curve: helix
            Graph3DFeature.Parametric(
                t => global::System.Math.Cos(t),
                t => global::System.Math.Sin(t),
                t => 0.15 * t,
                tMin: 0, tMax: 12 * global::System.Math.PI, samples: 1200);
        }
    }
}
