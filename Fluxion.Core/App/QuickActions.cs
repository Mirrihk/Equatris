// Fluxion.Core/App/Quick_simulations.cs
using System;
using Fluxion.Features;

namespace Fluxion.Core.App
{

    /// <summary>Reusable demo calls you can invoke from Program.Main or the CLI.</summary>
    public static class QuickSimulations
    {
        // Add inside Fluxion.Core.App.QuickSimulations
        public static void RunAll(bool waitBetween = true)
        {
            var demos = new (string name, Action call)[]
            {
        ("sin2d",        Sin2D),
        ("sinradial3d",  SinRadialSurface3D),
        ("sin3dline",    Sin3DLine),
        ("line2d",       () => Line2D()),
        ("line3d",       () => Line3DOnXZ(offsetY: 1.0)),
        ("polyline3d",   Polyline3D_XY_Z0),
        ("sxcysurface",  () => SinCosSurface3D(wireframe: true)),
        ("helix",        Helix3D),
            };

            foreach (var (name, call) in demos)
            {
                Console.WriteLine($"\n=== {name} ===");
                try { call(); }
                catch (Exception ex) { Console.WriteLine($"[ERROR] {name}: {ex.Message}"); }
                if (waitBetween) Pause();
            }
        }

        private static void Pause()
        {
            Console.Write("Press Enter for next demo...");
            Console.ReadLine();
        }

        public static void Sin2D()
        {
            Graph.F(System.Math.Sin)
                 .TwoD(-2 * System.Math.PI, 2 * System.Math.PI, samples: 1200);
        }

        public static void SinRadialSurface3D()
        {
            Graph.F(System.Math.Sin)
                 .ThreeD(-8, 8, -8, 8, lift: Lift.Radial, resolution: 180, wireframe: false);
        }

        public static void Sin3DLine()
        {
            Graph.F(System.Math.Sin)
                 .ThreeDLine(-2 * System.Math.PI, 2 * System.Math.PI, plane: EmbedPlane.XY, offset: 0, samples: 1000);
        }

        public static void Line2D(double m = 1.2, double b = -0.5)
        {
            Graph.Line(m, b).TwoD(-10, 10, samples: 600);
        }

        public static void Line3DOnXZ(double m = 1.2, double b = -0.5, double offsetY = 1.0)
        {
            Graph.Line(m, b).ThreeD(-10, 10, plane: EmbedPlane.XZ, offset: offsetY, samples: 600);
        }

        public static void Polyline3D_XY_Z0()
        {
            Graph.Points(
                (-3.0, -1.2), (-2.0, -0.2), (-1.0, 0.8),
                (0.0, 0.0), (1.0, 1.1), (2.0, 1.7), (3.0, 2.2)
            ).ThreeDLine(plane: EmbedPlane.XY, offset: 0);
        }

        public static void SinCosSurface3D(bool wireframe = true)
        {
            Graph3DFeature.Surface(
                (x, y) => System.Math.Sin(x) * System.Math.Cos(y),
                -2 * System.Math.PI, 2 * System.Math.PI,
                -2 * System.Math.PI, 2 * System.Math.PI,
                resolution: 180, wireframe: wireframe);
        }

        public static void Helix3D()
        {
            Graph3DFeature.Parametric(
                t => System.Math.Cos(t),
                t => System.Math.Sin(t),
                t => 0.15 * t,
                tMin: 0, tMax: 12 * System.Math.PI, samples: 1200);
        }

        public static void PrintMenu()
        {
            Console.WriteLine("QuickSimulations (run with: dotnet run <name>):");
            Console.WriteLine("  sin2d | sinradial3d | sin3dline | line2d | line3d | polyline3d | sxcysurface | helix");
        }

        public static bool TryRun(string name)
        {
            switch (name.Trim().ToLowerInvariant())
            {
                case "sin2d": Sin2D(); return true;
                case "sinradial3d": SinRadialSurface3D(); return true;
                case "sin3dline": Sin3DLine(); return true;
                case "line2d": Line2D(); return true;
                case "line3d": Line3DOnXZ(offsetY: 1.0); return true;
                case "polyline3d": Polyline3D_XY_Z0(); return true;
                case "sxcysurface": SinCosSurface3D(wireframe: true); return true;
                case "helix": Helix3D(); return true;
                default: return false;
            }
        }
    }
}
