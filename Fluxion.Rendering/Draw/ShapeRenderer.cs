using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Fluxion.Rendering.Draw
{
    /// <summary>
    /// Minimal shape renderer for testing basic GL drawing.
    /// Manages VAO, VBO, and Shader program.
    /// </summary>
    public sealed class ShapeRenderer : IDisposable
    {
        // VAO = Vertex Array Object (remembers layout of vertex data)
        private readonly int _vao;

        // VBO = Vertex Buffer Object (holds raw vertex data in GPU memory)
        private readonly int _vbo;

        // Shader program (compiled GLSL for vertex + fragment stages)
        private readonly int _shader;

        private const string VertexSource = @"
            #version 330 core
            layout (location = 0) in vec2 aPos;
            uniform mat4 uTransform;
            void main()
            {
                gl_Position = uTransform * vec4(aPos, 0.0, 1.0);
            }
        ";

        private const string FragmentSource = @"
            #version 330 core
            out vec4 FragColor;
            uniform vec3 uColor;
            void main()
            {
                FragColor = vec4(uColor, 1.0);
            }
        ";

        public ShapeRenderer()
        {
            _shader = CompileShader();

            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

            // attribute 0 = vec2 aPos
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        }

        public void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Vector3 color)
        {
            var vertices = new float[]
            {
                p1.X, p1.Y,
                p2.X, p2.Y,
                p3.X, p3.Y
            };

            UseShader(color);

            Matrix4 identity = Matrix4.Identity;
            GL.UniformMatrix4(GL.GetUniformLocation(_shader, "uTransform"), false, ref identity);

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }

        public void DrawQuad(Vector2 center, float size, Vector3 color)
        {
            float h = size / 2f;
            var vertices = new float[]
            {
                center.X - h, center.Y - h,
                center.X + h, center.Y - h,
                center.X + h, center.Y + h,

                center.X - h, center.Y - h,
                center.X + h, center.Y + h,
                center.X - h, center.Y + h
            };

            UseShader(color);

            Matrix4 identity = Matrix4.Identity;
            GL.UniformMatrix4(GL.GetUniformLocation(_shader, "uTransform"), false, ref identity);

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }

        private void UseShader(Vector3 color)
        {
            GL.UseProgram(_shader);
            GL.Uniform3(GL.GetUniformLocation(_shader, "uColor"), color);
        }

        private int CompileShader()
        {
            int vs = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vs, VertexSource);
            GL.CompileShader(vs);

            int fs = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fs, FragmentSource);
            GL.CompileShader(fs);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vs);
            GL.AttachShader(program, fs);
            GL.LinkProgram(program);

            GL.DeleteShader(vs);
            GL.DeleteShader(fs);

            return program;
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbo);
            GL.DeleteProgram(_shader);
        }
    }
}
