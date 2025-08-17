// Fluxion.Simulations/Algebra/AlgebraScene.cs
using Fluxion.Rendering;
using Fluxion.Rendering.Draw;
using Fluxion.Rendering.UI;          // <-- use ImGuiController
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;       // <-- to read current framebuffer size

namespace Fluxion.Simulations.Algebra
{
    public sealed class AlgebraScene : IRenderable
    {
        private readonly ImGuiController _imgui = new(); // <-- real backend
        private readonly AxesRenderer _axes = new();

        // parabola params (editable in UI)
        private float _a = 1f, _b = 0f, _c = 0f;

        public AlgebraScene() { }

        public void Render(float dt)
        {
            // Query current framebuffer size (so ImGui has the right DisplaySize)
            int[] vp = new int[4];
            GL.GetInteger(GetPName.Viewport, vp);
            int fbW = vp[2], fbH = vp[3];

            _imgui.NewFrame(fbW, fbH);

            // --- simple algebra UI ---
            ImGui.Begin("Algebra");
            ImGui.Text("Hello ImGui!");
            ImGui.Separator();
            ImGui.Text("Quadratic: y = ax^2 + bx + c");
            ImGui.SliderFloat("a", ref _a, -3f, 3f);
            ImGui.SliderFloat("b", ref _b, -5f, 5f);
            ImGui.SliderFloat("c", ref _c, -5f, 5f);
            ImGui.End();

            // --- simple visualization (axes + parabola) ---
            var proj = Matrix4.CreateOrthographicOffCenter(-6, 6, -4, 4, -1, 1);
            var mvp = Matrix4.Identity * proj;
            _axes.DrawAxes(mvp);
            _axes.DrawParabola(mvp, _a, _b, _c, -5, 5, 512);

            // --- draw ImGui on top ---
            _imgui.Render();
        }

        public void Dispose()
        {
            _axes.Dispose();
            _imgui.Dispose();
        }
    }
}
