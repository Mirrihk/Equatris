namespace Fluxion.Rendering
{
    public interface IRenderable : IDisposable
    {
        void Render(float dt);
    }

}