using Fluxion.Rendering.Windowing;
using Fluxion.Simulations.Algebra;

using var window = new FluxWindow(1280, 720, "Fluxion Engine");
AlgebraScene? scene = null;

window.OnLoadCallback = () =>
{
    scene = new AlgebraScene(); // now context exists
};

window.OnDraw = dt => scene?.Render(dt);

window.Run();
scene?.Dispose();
