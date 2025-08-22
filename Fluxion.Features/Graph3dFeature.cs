using System;
using Fluxion.Math.Functions3D;
using Fluxion.Rendering.Camera;
using Fluxion.Rendering.Draw;
using Fluxion.Rendering.Visualize3D;
using OpenTK.Mathematics;

namespace Fluxion.Features
{
    public static class Graph3DFeature
    {
        /// <summary>Graph a surface z = f(x,y).</summary>
        public static void Surface(Func<double, double, double> f,
            double xMin, double xMax, double yMin, double yMax,
            int resolution = 100, bool wireframe = false)
        {
            var field = new DelegateScalarField(f);
            var mesh = SurfaceFactory.BuildSurface(field, xMin, xMax, yMin, yMax, resolution);
            Run3DWindow(mesh, wireframe, title: "Fluxion Surface");
        }

        /// <summary>Graph a parametric curve r(t) = (x(t), y(t), z(t)).</summary>
        public static void Parametric(Func<double, double> x, Func<double, double> y, Func<double, double> z,
                                      double tMin, double tMax, int samples = 1000)
        {
            OpenTK.Mathematics.Vector3d r(double t) => new(x(t), y(t), z(t));
            var mesh = SurfaceFactory.BuildPolyline(r, tMin, tMax, samples);
            Run3DWindow(mesh, wireframe: true, title: "Fluxion Curve");
        }

        private static void Run3DWindow(Mesh3D mesh, bool wireframe, string title)
        {
            // Reuse your existing FluxWindow if it supports custom draw hooks.
            using var window = new FluxWindow3D(title, 1280, 800, mesh, wireframe);
            window.Run();
        }
    }

    /// <summary>Thin wrapper around your GameWindow to host 3D content.</summary>
    // inside Fluxion.Features namespace...

    internal sealed class FluxWindow3D : OpenTK.Windowing.Desktop.GameWindow
    {
        private readonly Mesh3D mesh;
        private readonly bool wireframe;

        private readonly OrbitCamera cam = new();
        private SurfaceRenderer? renderer;      // was: new SurfaceRenderer()
        private Axis3DRenderer? axes;           // was: new Axis3DRenderer()

        private OpenTK.Mathematics.Matrix4 proj, view, model;

        public FluxWindow3D(string title, int w, int h, Mesh3D mesh, bool wireframe)
            : base(OpenTK.Windowing.Desktop.GameWindowSettings.Default,
                   new OpenTK.Windowing.Desktop.NativeWindowSettings { Title = title, ClientSize = new(w, h) })
        {
            this.mesh = mesh;
            this.wireframe = wireframe;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // (Context is current now)
            OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DepthTest);

            // Instantiate AFTER the context exists
            renderer = new SurfaceRenderer();
            axes = new Axis3DRenderer();

            // Upload AFTER renderer is created (and context is alive)
            renderer.Upload(mesh, wireframeAsLines: wireframe && mesh.Indices.Length == 0);

            model = OpenTK.Mathematics.Matrix4.Identity;
            proj = cam.GetProjectionMatrix(Size.X / (float)Size.Y);
        }

        protected override void OnResize(OpenTK.Windowing.Common.ResizeEventArgs e)
        {
            base.OnResize(e);
            OpenTK.Graphics.OpenGL4.GL.Viewport(0, 0, Size.X, Size.Y);
            proj = cam.GetProjectionMatrix(Size.X / (float)Size.Y);
        }

        protected override void OnUpdateFrame(OpenTK.Windowing.Common.FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            cam.Yaw += 0.15 * args.Time;
        }

        protected override void OnRenderFrame(OpenTK.Windowing.Common.FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            OpenTK.Graphics.OpenGL4.GL.ClearColor(0.05f, 0.07f, 0.1f, 1f);
            OpenTK.Graphics.OpenGL4.GL.Clear(
                OpenTK.Graphics.OpenGL4.ClearBufferMask.ColorBufferBit |
                OpenTK.Graphics.OpenGL4.ClearBufferMask.DepthBufferBit);

            view = cam.GetViewMatrix();
            var mvp = model * view * proj;

            axes!.DrawAxes(mvp);
            renderer!.Draw(mvp, model, wireframe);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            renderer?.Dispose();
        }
    }

}
