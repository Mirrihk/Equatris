// Fluxion.Rendering/Draw/AxesRenderer.cs
using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Fluxion.Rendering.Draw
{
    /// <summary>Draws simple X/Y axes centered at the origin.</summary>
    public sealed class AxesRenderer : IDisposable
    {
        private readonly int _vao, _vbo, _shader;
        private readonly int _locMvp, _locColor;

        private const string VS = @"#version 330 core
            layout (location = 0) in vec2 aPos;
            uniform mat4 uMVP;
            void main(){ gl_Position = uMVP * vec4(aPos, 0.0, 1.0); }";

        private const string FS = @"#version 330 core
            uniform vec3 uColor;
            out vec4 FragColor;
            void main(){ FragColor = vec4(uColor, 1.0); }";

        public AxesRenderer()
        {
            _shader = Compile(VS, FS);
            _locMvp = GL.GetUniformLocation(_shader, "uMVP");
            _locColor = GL.GetUniformLocation(_shader, "uColor");

            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>Draw axes with tickless crosshairs in [-range, +range].</summary>
        public void DrawAxes(Matrix4 projection, float range = 10f)
        {
            float[] verts = {
                -range, 0f,  range, 0f,   // X axis
                 0f, -range, 0f,  range   // Y axis
            };

            GL.UseProgram(_shader);
            GL.UniformMatrix4(_locMvp, false, ref projection);
            GL.Uniform3(_locColor, 0.8f, 0.8f, 0.8f);

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, verts.Length * sizeof(float), verts, BufferUsageHint.DynamicDraw);

            GL.LineWidth(1.5f);
            GL.DrawArrays(PrimitiveType.Lines, 0, 4);

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }

        private static int Compile(string vs, string fs)
        {
            int v = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(v, vs);
            GL.CompileShader(v);

            int f = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(f, fs);
            GL.CompileShader(f);

            int p = GL.CreateProgram();
            GL.AttachShader(p, v);
            GL.AttachShader(p, f);
            GL.LinkProgram(p);

            GL.DeleteShader(v);
            GL.DeleteShader(f);
            return p;
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(_vao);
            GL.DeleteBuffer(_vbo);
            GL.DeleteProgram(_shader);
        }
    }
}
