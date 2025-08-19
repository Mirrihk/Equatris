// Fluxion.Rendering/Windowing/FluxWindow.cs
using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Fluxion.Rendering.Windowing
{
    /// <summary>
    /// Thin wrapper around OpenTK's GameWindow.
    /// Lets you plug in drawing code via OnDraw.
    /// </summary>
    public sealed class FluxWindow : IDisposable
    {
        private readonly GameWindow _gw;

        /// <summary>
        /// Called every frame with deltaTime (in seconds).
        /// Assign your rendering logic here.
        /// </summary>
        public Action<float>? OnDraw { get; set; }

        public FluxWindow(int width, int height, string title)
        {
            var native = new NativeWindowSettings
            {
                Title = title,
                ClientSize = new Vector2i(width, height),
                APIVersion = new Version(3, 3),
                StartVisible = true,
                StartFocused = true
            };

            _gw = new GameWindow(GameWindowSettings.Default, native);

            // Hooks
            _gw.Load += OnLoad;
            _gw.Unload += OnUnload;
            _gw.Resize += e => GL.Viewport(0, 0, _gw.ClientSize.X, _gw.ClientSize.Y);
            _gw.UpdateFrame += Update;
            _gw.RenderFrame += Render;

            // Start with VSync on
            _gw.VSync = VSyncMode.On;
        }

        private void OnLoad()
        {
            GL.ClearColor(0.10f, 0.10f, 0.12f, 1f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // Debug info
            try
            {
                Console.WriteLine($"[GL] Version : {GL.GetString(StringName.Version)}");
                Console.WriteLine($"[GL] Vendor  : {GL.GetString(StringName.Vendor)}");
                Console.WriteLine($"[GL] Renderer: {GL.GetString(StringName.Renderer)}");
            }
            catch { /* safe to ignore */ }
        }

        private void OnUnload()
        {
            // Clean up GL resources here later if needed
        }

        private void Update(FrameEventArgs e)
        {
            // Handle input/simulation here
        }

        private void Render(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Call user draw code
            OnDraw?.Invoke((float)e.Time);

            _gw.SwapBuffers();
        }

        /// <summary>Blocks until the window is closed.</summary>
        public void Run() => _gw.Run();

        public void Close() => _gw.Close();

        public void Dispose() => _gw.Dispose();
    }
}
