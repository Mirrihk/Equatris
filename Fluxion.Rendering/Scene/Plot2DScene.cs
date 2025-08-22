// Fluxion.Rendering/Scene/Plot2DScene.cs
using Fluxion.Rendering.Draw;        // AxesRenderer, CurveRenderer2D
using Fluxion.Rendering.Scene;
using Fluxion.Rendering.Visualize;   // Plot2D
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Fluxion.Rendering.SceneImpl
{
    public sealed class Plot2DScene : IScene
    {
        private readonly Plot2D plot;
        private readonly double xMin, xMax, yMin, yMax;

        private AxesRenderer? axes;
        private CurveRenderer2D? curve;
        private Matrix4 proj;

        public Plot2DScene(Plot2D plot, double xMin, double xMax, double yMin, double yMax)
        {
            this.plot = plot;
            this.xMin = xMin; this.xMax = xMax;
            this.yMin = yMin; this.yMax = yMax;
            proj = Matrix4.Identity;
        }

        public void OnLoad()
        {
            // Context is current here—safe to create GL objects
            GL.Disable(EnableCap.DepthTest);
            axes = new AxesRenderer();
            curve = new CurveRenderer2D();
        }

        public void OnResize(int width, int height)
        {
            proj = Matrix4.CreateOrthographicOffCenter(
                (float)xMin, (float)xMax,
                (float)yMin, (float)yMax,
                -1f, 1f);
        }

        public void OnUpdate(double dt) { /* no-op for now */ }

        public void OnRender()
        {
            axes!.DrawAxes(proj);
            curve!.Draw(plot, proj);
        }

        public void Dispose()
        {
            (axes as System.IDisposable)?.Dispose();
            (curve as System.IDisposable)?.Dispose();
        }
    }
}
