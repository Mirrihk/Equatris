// Fluxion.Rendering/Windowing/FluxWindow.cs
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Fluxion.Rendering.Windowing
{
    public sealed class FluxWindow : GameWindow
    {
        public Action? OnLoadCallback { get; set; }   // <-- NEW
        public Action<float>? OnDraw { get; set; }    // already used earlier

        public FluxWindow(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings
            {
                ClientSize = new Vector2i(width, height),
                Title = title
            })
        { }

        protected override void OnLoad()
        {
            base.OnLoad();
            VSync = VSyncMode.On;
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.08f, 0.09f, 0.10f, 1f);

            OnLoadCallback?.Invoke();                 // <-- SAFE INIT POINT
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            OnDraw?.Invoke((float)args.Time);         // <-- NULL-GUARDED

            SwapBuffers();
        }
    }
}
