using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Fluxion.Rendering.Draw
{
    public sealed class Axis3DRenderer
    {
        public void DrawAxes(Matrix4 mvp)
        {
            GL.Disable(EnableCap.DepthTest);

            // Use modern OpenGL buffer-based rendering instead of deprecated immediate mode
            var vertices = new float[]
            {
                // X axis (red-ish)
                -100f, 0f, 0f, 100f, 0f, 0f,
                // Y axis (green-ish)
                0f, -100f, 0f, 0f, 100f, 0f,
                // Z axis (blue-ish)
                0f, 0f, -100f, 0f, 0f, 100f
            };

            var colors = new float[]
            {
                // X axis (red-ish)
                0.9f, 0.2f, 0.2f, 0.9f, 0.2f, 0.2f,
                // Y axis (green-ish)
                0.2f, 0.9f, 0.2f, 0.2f, 0.9f, 0.2f,
                // Z axis (blue-ish)
                0.2f, 0.5f, 0.9f, 0.2f, 0.5f, 0.9f
            };

            // Create and bind a Vertex Array Object (VAO)
            int vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            // Create and bind a Vertex Buffer Object (VBO) for vertices
            int vboVertices = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboVertices);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            // Create and bind a VBO for colors
            int vboColors = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboColors);
            GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * sizeof(float), colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            // Draw the axes
            GL.DrawArrays(PrimitiveType.Lines, 0, vertices.Length / 3);

            // Cleanup
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.DeleteBuffer(vboVertices);
            GL.DeleteBuffer(vboColors);
            GL.DeleteVertexArray(vao);

            GL.Enable(EnableCap.DepthTest);
        }
    }
}
