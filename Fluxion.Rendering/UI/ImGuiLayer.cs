// Fluxion.Rendering/UI/ImGuiLayer.cs
using ImGuiNET;
using System;

namespace Fluxion.Rendering.UI
{
    public sealed class ImGuiLayer : IDisposable
    {
        private IntPtr _ctx;

        public ImGuiLayer()
        {
            _ctx = ImGui.CreateContext();
            ImGui.SetCurrentContext(_ctx);
            var io = ImGui.GetIO();
            io.Fonts.AddFontDefault(); // prevents null refs in font atlas accesses
        }

        public void NewFrame(int width, int height)
        {
            var io = ImGui.GetIO();
            io.DisplaySize = new System.Numerics.Vector2(width, height);
            ImGui.NewFrame();
        }

        public void RenderDrawData()
        {
            ImGui.Render();
            // NOTE: this is a stub; add a proper renderer later if you want visible ImGui.
        }

        public void Dispose()
        {
            if (_ctx != IntPtr.Zero)
            {
                ImGui.DestroyContext(_ctx);
                _ctx = IntPtr.Zero;
            }
        }
    }
}
