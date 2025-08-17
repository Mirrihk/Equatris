// Fluxion.Rendering/Draw/FluxWindow.cs
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;


namespace Fluxion.Rendering.Draw
{
    public sealed class FluxWindow : GameWindow
    {
        private readonly Stopwatch _fpsTimer = new();
        private int _framesPerSecond = 0;

        public FluxWindow
            (
                int width = 1280,
                int height = 720,
                string title = "Fluxion Engine"
            ) : base(GameWindowSettings.Default,
                new NativeWindowSettings
                {
                    Size = new Vector2i(width, height),
                    Title = title
                })
        { }

        protected override void OnLoad()
        {
            base.OnLoad();
            VSync = VSyncMode.On;
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.08f, 0.09f, 0.10f, 1f);
            _fpsTimer.Start();
        }
         protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (KeyboardState.IsKeyDown(Keys.F1))
            {
                VSync = VSync == VSyncMode.On ? VSyncMode.Off : VSyncMode.On;
            }

        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SwapBuffers();

            _framesPerSecond++;
            if(_fpsTimer.ElapsedMilliseconds >= 1000)
            {
                var fps = _framesPerSecond;
                var ms = 1000 / Math.Max(fps, 1);
                Title = $"Fluxion Engine | FPS: {fps} | {ms:0.0} ms | VSync: {VSync}";
                _framesPerSecond = 0;
                _fpsTimer.Restart();
            }
        }
    }
}