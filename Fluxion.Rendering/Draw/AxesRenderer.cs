// Fluxion.Rendering/Draw/AxesRenderer.cs
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Fluxion.Rendering.Draw
{
    public sealed class AxesRenderer : IDisposable
    {
        private readonly int _vao, _vbo, _shader;

        private const string VS = """
            #version 330 core
            layout (location = 0) in vec2 aPos;
            uniform mat4 uMVP;
            void main(){ gl_Position = uMVP * vec4(aPos, 0.0, 1.0); }
        """;

        private const string FS = """
            #version 330 core
            out vec4 FragColor;
            uniform vec3 uColor;
            void main(){ FragColor = vec4(uColor, 1.0); }
        """;

        public AxesRenderer()
        {
            _shader = Compile(VS, FS);
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        }

        public void DrawAxes(Matrix4 mvp)
        {
            // x- and y- axes from -10..10
            var verts = new float[]
            {
                -10, 0,  10, 0,   // X axis
                 0, -10, 0, 10    // Y axis
            };

            GL.UseProgram(_shader);
            GL.UniformMatrix4(GL.GetUniformLocation(_shader, "uMVP"), false, ref mvp);
            GL.Uniform3(GL.GetUniformLocation(_shader, "uColor"), new Vector3(0.8f, 0.8f, 0.8f));

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, verts.Length * sizeof(float), verts, BufferUsageHint.DynamicDraw);
            GL.DrawArrays(PrimitiveType.Lines, 0, 4);
        }

        public void DrawParabola(Matrix4 mvp, float a, float b, float c, float xMin = -5, float xMax = 5, int samples = 256)
        {
            var verts = new float[samples * 2];
            float step = (xMax - xMin) / (samples - 1);
            for (int i = 0; i < samples; i++)
            {
                float x = xMin + i * step;
                float y = a * x * x + b * x + c;
                verts[2 * i] = x; verts[2 * i + 1] = y;
            }

            GL.UseProgram(_shader);
            GL.UniformMatrix4(GL.GetUniformLocation(_shader, "uMVP"), false, ref mvp);
            GL.Uniform3(GL.GetUniformLocation(_shader, "uColor"), new Vector3(0.2f, 0.8f, 0.3f));

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, verts.Length * sizeof(float), verts, BufferUsageHint.DynamicDraw);
            GL.DrawArrays(PrimitiveType.LineStrip, 0, samples);
        }

        private static int Compile(string vs, string fs)
        {
            int v = GL.CreateShader(ShaderType.VertexShader); GL.ShaderSource(v, vs); GL.CompileShader(v);
            int f = GL.CreateShader(ShaderType.FragmentShader); GL.ShaderSource(f, fs); GL.CompileShader(f);
            int p = GL.CreateProgram(); GL.AttachShader(p, v); GL.AttachShader(p, f); GL.LinkProgram(p);
            GL.DeleteShader(v); GL.DeleteShader(f); return p;
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbo);
            GL.DeleteProgram(_shader);
        }
    }
}
