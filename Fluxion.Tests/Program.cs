using OpenTK.Windowing.Desktop;
using Fluxion.Rendering.Windowing;

class Program
{
    static void Main(string[] args)
    {
        var native = new NativeWindowSettings
        {
            Title = "Fluxion Engine",
            Size = new OpenTK.Mathematics.Vector2i(1280, 720)
        };

        using var window = new FluxionHost(GameWindowSettings.Default, native);
        window.Run();
    }
}
