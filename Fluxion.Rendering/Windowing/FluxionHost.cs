using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Fluxion.Rendering.Windowing
{
    public sealed class FluxionHost : GameWindow
    {
        private int _vao, _vbo, _shader;

        public FluxionHost(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws) { }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.08f, 0.08f, 0.12f, 1f);

            // A tiny triangle: 2D position (xy) + color (rgb)
            float[] vertices =
            {
                //  x,     y,     r,   g,   b
                -0.6f, -0.5f,   1f,  0f,  0f,
                 0.6f, -0.5f,   0f,  1f,  0f,
                 0.0f,  0.6f,   0f,  0f,  1f,
            };

            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            _shader = CreateProgram(VertexSrc, FragmentSrc);
            GL.UseProgram(_shader);

            // layout(location=0) vec2 aPos
            int aPos = GL.GetAttribLocation(_shader, "aPos");
            GL.EnableVertexAttribArray(aPos);
            GL.VertexAttribPointer(aPos, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            // layout(location=1) vec3 aColor
            int aColor = GL.GetAttribLocation(_shader, "aColor");
            GL.EnableVertexAttribArray(aColor);
            GL.VertexAttribPointer(aColor, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 2 * sizeof(float));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(_shader);
            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            if (_vbo != 0) GL.DeleteBuffer(_vbo);
            if (_vao != 0) GL.DeleteVertexArray(_vao);
            if (_shader != 0) GL.DeleteProgram(_shader);
        }

        // --- Minimal shaders (GL 3.3 core) ---
        private const string VertexSrc = @"
#version 330 core
layout(location = 0) in vec2 aPos;
layout(location = 1) in vec3 aColor;
out vec3 vColor;
void main()
{
    vColor = aColor;
    gl_Position = vec4(aPos, 0.0, 1.0);
}";
        private const string FragmentSrc = @"
#version 330 core
in vec3 vColor;
out vec4 FragColor;
void main()
{
    FragColor = vec4(vColor, 1.0);
}";

        // --- Helper: compile + link shader program ---
        private static int CreateProgram(string vs, string fs)
        {
            int v = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(v, vs);
            GL.CompileShader(v);
            GL.GetShader(v, ShaderParameter.CompileStatus, out int okV);
            if (okV == 0) throw new Exception("Vertex shader error: " + GL.GetShaderInfoLog(v));

            int f = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(f, fs);
            GL.CompileShader(f);
            GL.GetShader(f, ShaderParameter.CompileStatus, out int okF);
            if (okF == 0) throw new Exception("Fragment shader error: " + GL.GetShaderInfoLog(f));

            int p = GL.CreateProgram();
            GL.AttachShader(p, v);
            GL.AttachShader(p, f);
            GL.LinkProgram(p);
            GL.GetProgram(p, GetProgramParameterName.LinkStatus, out int okP);

            GL.DeleteShader(v);
            GL.DeleteShader(f);

            if (okP == 0) throw new Exception("Program link error: " + GL.GetProgramInfoLog(p));
            return p;
        }
    }
}
