using OpenTK.Windowing.Desktop;

class Program
{
    static void Main(string[] args)
    {
        var native = new NativeWindowSettings()
        {
            Title = "Fluxion Engine",
            Size = new OpenTK.Mathematics.Vector2i(1280, 720)
        };

        using var window = new FluxionWindow(GameWindowSettings.Default, native);
        window.Run();
    }
}
