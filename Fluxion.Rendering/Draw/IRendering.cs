using System;
using System.Numerics;
using Fluxion.Rendering.Draw;

namespace Fluxion.Rendering.Draw
{
	public interface IRenderer
	{
		/// Clear the screen to a color
		void Clear(float r, float g, float b, float a = 1f);

		/// Draw points (2D)
		void DrawPoints(ReadOnlySpan<Vector2> pts, float size = 2f);

		/// Draw lines (2D)
		void DrawLines(ReadOnlySpan<Vector2> pts, float width = 1f);
	}
}