// Fluxion.Core/App/Program.cs
using System;

namespace Fluxion.Core.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0 && QuickSimulations.TryRun(args[0]))
                return;

            QuickSimulations.PrintMenu();
            // Or call one directly while developing:
             QuickSimulations.Sin2D();
            // Call any demo directly:
             QuickSimulations.Sin2D();
             QuickSimulations.SinRadialSurface3D();
             QuickSimulations.Sin3DLine();

            // With parameters:
             QuickSimulations.Line2D(m: 2.0, b: -1.0);
             QuickSimulations.Line3DOnXZ(m: 0.7, b: 0.0, offsetY: 2.0);
             QuickSimulations.Polyline3D_XY_Z0();
             QuickSimulations.SinCosSurface3D(wireframe: true);
             QuickSimulations.Helix3D();
        }
    }
}
