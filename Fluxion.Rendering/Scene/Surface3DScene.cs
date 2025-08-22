// Fluxion.Rendering/Scene/Surface3DScene.cs
using Fluxion.Rendering.Camera;       // OrbitCamera
using Fluxion.Rendering.Draw;         // SurfaceRenderer, Axis3DRenderer
using Fluxion.Rendering.Scene;
using Fluxion.Rendering.Visualize3D;  // Mesh3D
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Fluxion.Rendering.SceneImpl
{
    public sealed class Surface3DScene : IScene
    {
        private readonly Mesh3D mesh;
        private readonly bool wireframe;

        private readonly OrbitCamera cam = new();
        private SurfaceRenderer? renderer;
        private Axis3DRenderer? axes;

        private Matrix4 proj, view, model;

        public Surface3DScene(Mesh3D mesh, bool wireframe)
        {
            this.mesh = mesh;
            this.wireframe = wireframe;
            proj = view = model = Matrix4.Identity;
        }

        public void OnLoad()
        {
            GL.Enable(EnableCap.DepthTest);
            renderer = new SurfaceRenderer();
            axes = new Axis3DRenderer();
            renderer.Upload(mesh, wireframeAsLines: wireframe && mesh.Indices.Length == 0);
            model = Matrix4.Identity;
        }

        public void OnResize(int width, int height)
        {
            proj = cam.GetProjectionMatrix(width / (float)height);
        }

        public void OnUpdate(double dt)
        {
            cam.Yaw += 0.15 * dt; // simple idle orbit
        }

        public void OnRender()
        {
            view = cam.GetViewMatrix();
            var mvp = model * view * proj;
            axes!.DrawAxes(mvp);
            renderer!.Draw(mvp, model, wireframe);
        }

        public void Dispose()
        {
            renderer?.Dispose();
            axes = null; // nothing to dispose
        }
    }
}
