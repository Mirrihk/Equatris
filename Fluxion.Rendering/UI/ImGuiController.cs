// Fluxion.Rendering/UI/ImGuiController.cs
using System;
using System.Numerics;                 // for ImGui Vector2/Matrix types when needed
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;              // for OpenTK.Matrix4 (used in the GL uniform)

namespace Fluxion.Rendering.UI
{
    /// <summary>
    /// Minimal ImGui renderer for OpenTK (GL4).
    /// - Creates ImGui context
    /// - Uploads font atlas to a GL texture
    /// - Renders ImGui draw data into the current framebuffer
    /// </summary>
    public sealed class ImGuiController : IDisposable
    {
        private IntPtr _ctx = IntPtr.Zero;

        // GL resources
        private int _fontTexture;
        private int _shader;
        private int _vbo, _ebo, _vao;

        // Shader locations
        private int _locTex;
        private int _locProj;

        private int _locPos;
        private int _locUV;
        private int _locColor;

        // ImGui vertex layout (pos:vec2, uv:vec2, col:RGBA8)
        private const int VtxStride = 20;       // 4*2 + 4*2 + 4 = 20 bytes
        private const int VtxOffsetPos = 0;
        private const int VtxOffsetUV = 8;
        private const int VtxOffsetColor = 16;

        public ImGuiController()
        {
            // Create & set context
            _ctx = ImGui.CreateContext();
            ImGui.SetCurrentContext(_ctx);

            var io = ImGui.GetIO();
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
            io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors | ImGuiBackendFlags.RendererHasVtxOffset;

            // Add a default font so the atlas exists
            io.Fonts.AddFontDefault();

            CreateDeviceObjects();
        }

        /// <summary>Begin a new ImGui frame. Pass current framebuffer width/height.</summary>
        public void NewFrame(int fbWidth, int fbHeight)
        {
            ImGui.SetCurrentContext(_ctx);
            var io = ImGui.GetIO();
            io.DisplaySize = new System.Numerics.Vector2(fbWidth, fbHeight);  // fully-qualified to avoid ambiguity
            io.DisplayFramebufferScale = new System.Numerics.Vector2(1, 1);

            ImGui.NewFrame();
        }

        /// <summary>Ends the ImGui frame and renders draw data to the current framebuffer.</summary>
        public void Render()
        {
            ImGui.SetCurrentContext(_ctx);
            ImGui.Render();
            RenderDrawData(ImGui.GetDrawData());
        }

        private void CreateDeviceObjects()
        {
            // ---------- Shader ----------
            const string vs = @"#version 330 core
                uniform mat4 ProjMtx;
                layout (location = 0) in vec2 Position;
                layout (location = 1) in vec2 UV;
                layout (location = 2) in vec4 Color;
                out vec2 Frag_UV;
                out vec4 Frag_Color;
                void main()
                {
                    Frag_UV = UV;
                    Frag_Color = Color;
                    gl_Position = ProjMtx * vec4(Position.xy, 0, 1);
                }";

            const string fs = @"#version 330 core
                uniform sampler2D Texture;
                in vec2 Frag_UV;
                in vec4 Frag_Color;
                out vec4 Out_Color;
                void main()
                {
                    Out_Color = Frag_Color * texture(Texture, Frag_UV.st);
                }";

            int vert = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vert, vs);
            GL.CompileShader(vert);
            CheckShader(vert, "ImGui VS");

            int frag = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(frag, fs);
            GL.CompileShader(frag);
            CheckShader(frag, "ImGui FS");

            _shader = GL.CreateProgram();
            GL.AttachShader(_shader, vert);
            GL.AttachShader(_shader, frag);
            GL.LinkProgram(_shader);
            CheckProgram(_shader, "ImGui Program");

            GL.DeleteShader(vert);
            GL.DeleteShader(frag);

            _locTex = GL.GetUniformLocation(_shader, "Texture");
            _locProj = GL.GetUniformLocation(_shader, "ProjMtx");
            _locPos = 0; // layout(location=0)
            _locUV = 1; // layout(location=1)
            _locColor = 2; // layout(location=2)

            // ---------- Buffers/VAO ----------
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();
            _vao = GL.GenVertexArray();

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

            GL.EnableVertexAttribArray(_locPos);
            GL.EnableVertexAttribArray(_locUV);
            GL.EnableVertexAttribArray(_locColor);

            GL.VertexAttribPointer(_locPos, 2, VertexAttribPointerType.Float, false, VtxStride, VtxOffsetPos);
            GL.VertexAttribPointer(_locUV, 2, VertexAttribPointerType.Float, false, VtxStride, VtxOffsetUV);
            GL.VertexAttribPointer(_locColor, 4, VertexAttribPointerType.UnsignedByte, true, VtxStride, VtxOffsetColor);

            // ---------- Font atlas ----------
            RecreateFontDeviceTexture();

            // Unbind
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void RecreateFontDeviceTexture()
        {
            ImGui.SetCurrentContext(_ctx);
            var io = ImGui.GetIO();

            io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out _);

            _fontTexture = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _fontTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.PixelStore(PixelStoreParameter.UnpackRowLength, 0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                          PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            io.Fonts.SetTexID((IntPtr)_fontTexture);
            io.Fonts.ClearTexData(); // CPU-side pixels can be freed now
        }

        private void RenderDrawData(ImDrawDataPtr drawData)
        {
            if (drawData.CmdListsCount == 0) return;

            // ----- GL state: blend, scissor; no depth/cull -----
            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.ScissorTest);

            // Viewport & orthographic projection
            GL.Viewport(0, 0, (int)drawData.DisplaySize.X, (int)drawData.DisplaySize.Y);

            float L = drawData.DisplayPos.X;
            float R = drawData.DisplayPos.X + drawData.DisplaySize.X;
            float T = drawData.DisplayPos.Y;
            float B = drawData.DisplayPos.Y + drawData.DisplaySize.Y;

            // OpenTK.Matrix4 (column-major) matching ImGui’s ortho
            Matrix4 ortho = new Matrix4(
                2.0f / (R - L), 0, 0, 0,
                0, 2.0f / (T - B), 0, 0,
                0, 0, -1, 0,
                (R + L) / (L - R), (T + B) / (B - T), 0, 1
            );

            GL.UseProgram(_shader);
            GL.Uniform1(_locTex, 0); // Texture unit 0
            GL.UniformMatrix4(_locProj, false, ref ortho);

            GL.BindVertexArray(_vao);

            // Iterate command lists (use CmdLists, not CmdListsRange)
            for (int n = 0; n < drawData.CmdListsCount; n++)
            {
                ImDrawListPtr cmdList = drawData.CmdLists[n];

                // Upload vertex/index buffers
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, cmdList.VtxBuffer.Size * VtxStride, IntPtr.Zero, BufferUsageHint.StreamDraw);
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, cmdList.VtxBuffer.Size * VtxStride, cmdList.VtxBuffer.Data);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, cmdList.IdxBuffer.Size * sizeof(ushort), IntPtr.Zero, BufferUsageHint.StreamDraw);
                GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, cmdList.IdxBuffer.Size * sizeof(ushort), cmdList.IdxBuffer.Data);

                int idxOffset = 0;
                for (int cmdi = 0; cmdi < cmdList.CmdBuffer.Size; cmdi++)
                {
                    var pcmd = cmdList.CmdBuffer[cmdi];

                    // Bind texture
                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.Texture2D, (int)pcmd.TextureId);

                    // Scissor (flip Y)
                    var clip = pcmd.ClipRect;
                    GL.Scissor(
                        (int)clip.X,
                        (int)(drawData.DisplaySize.Y - clip.W),
                        (int)(clip.Z - clip.X),
                        (int)(clip.W - clip.Y)
                    );

                    // NOTE: OpenTK 4 expects PrimitiveType here (NOT BeginMode)
                    GL.DrawElementsBaseVertex(
                        PrimitiveType.Triangles,
                        (int)pcmd.ElemCount,
                        DrawElementsType.UnsignedShort,
                        (IntPtr)(idxOffset * sizeof(ushort)),
                        0
                    );

                    idxOffset += (int)pcmd.ElemCount;
                }
            }

            // Restore modest state
            GL.Disable(EnableCap.ScissorTest);
        }

        private static void CheckShader(int shader, string label)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int ok);
            if (ok == 0)
            {
                string log = GL.GetShaderInfoLog(shader);
                throw new Exception($"{label} compile error: {log}");
            }
        }

        private static void CheckProgram(int program, string label)
        {
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int ok);
            if (ok == 0)
            {
                string log = GL.GetProgramInfoLog(program);
                throw new Exception($"{label} link error: {log}");
            }
        }

        public void Dispose()
        {
            if (_ctx != IntPtr.Zero)
            {
                GL.DeleteTexture(_fontTexture);
                GL.DeleteBuffer(_vbo);
                GL.DeleteBuffer(_ebo);
                GL.DeleteVertexArray(_vao);
                GL.DeleteProgram(_shader);

                ImGui.DestroyContext(_ctx);
                _ctx = IntPtr.Zero;
            }
        }
    }
}
