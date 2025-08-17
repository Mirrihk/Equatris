using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

public sealed class FluxionWindow : GameWindow
{
    public FluxionWindow(GameWindowSettings gws, NativeWindowSettings nws)
        : base(gws, nws)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(Color4.Black); // background color
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        // Here you’ll update physics / math each frame
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // Example: draw something simple (we’ll add more later)

        SwapBuffers(); // show frame
    }
}
