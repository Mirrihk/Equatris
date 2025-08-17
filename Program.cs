// Fluxion.Core/App/Program.cs
using Fluxion.Rendering.Draw;

namespace Fluxion.Core.App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using var window = new FluxWindow(1280, 720, "Fluxion Engine");
            window.Run();
        }
    }
}
