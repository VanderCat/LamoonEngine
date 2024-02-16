using System.Drawing;
using System.Numerics;
using Serilog;
using Silk.NET.OpenGL;

namespace Lamoon.Graphics; 

public static partial class Immedieate {

    private static GL gl => GraphicsReferences.OpenGl;
    public static void Clear(ClearBufferMask mask) {
        gl.Clear(mask);
    }

    public static Shader Shader { get; private set; } = Shader.Default;
    
    [Obsolete]
    public static void UseShader(uint handle) {
        gl.UseProgram(handle);
    }

    public static void UseShader(Shader shader) {
        UseShader(shader.OpenGlHandle);
        Shader = shader;
    }

    public static void UseShader() {
        UseShader(Shader.Default);
    }

    public static void BindTexture(uint handle, TextureTarget target, TextureUnit textureUnit = TextureUnit.Texture0) {
        gl.ActiveTexture(textureUnit);
        gl.BindTexture(target, handle);
        Texture._lastBoundTexture = handle;
    }

    public static void BindTexture(uint handle, TextureTarget target,
        uint textureUnit) {
        gl.ActiveTexture((GLEnum)((uint)TextureUnit.Texture0 + textureUnit));
        gl.BindTexture(target, handle);
        Texture._lastBoundTexture = handle;
    }

    public static void BindTexture(Texture texture, TextureUnit textureUnit = TextureUnit.Texture0) {
        BindTexture(texture.OpenGlHandle, texture.Type, textureUnit);
    }

    public static void BindTexture(Texture texture, uint textureUnit) {
        BindTexture(texture.OpenGlHandle, texture.Type, textureUnit);
    }

    public static void PrintError() {
        var oglerror = (ErrorCode)gl.GetError();
        if (oglerror != ErrorCode.NoError)
            Log.Error("OpenGL Error: {0}", oglerror);
    }

    private static Color _color;

    public static void UseColor(Color color) {
        Shader.SetVector4("color", new Vector4(_color.R, _color.G, _color.B, _color.A));
    }

    public unsafe static void DrawLine(Vector3 start, Vector3 end) {
        float[] vertices = {
            start.X, start.Y, start.Z,
            end.X, end.Y, end.Z
        };
        var VAO = gl.GenVertexArray();
        var VBO = gl.GenBuffer();
        
        gl.BindVertexArray(VAO);
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);
        fixed (float* buf = vertices)
            gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), buf,
                BufferUsageARB.StreamDraw);

        gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), (void*)0);
        gl.EnableVertexAttribArray(0);
        
        gl.DrawArrays(PrimitiveType.Lines, 0, 2);
        
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0); 
        gl.BindVertexArray(0);
        gl.DeleteBuffer(VBO);
        gl.DeleteVertexArray(VAO);
    }

    public static void DrawGrid(Vector3 position, Vector3 normal, float size, float density) {
        float[] vertices = new float[(int)((size/density)*4+4)*3];

        vertices[0] = position.X;
        vertices[1] = position.Y;
        vertices[2] = position.Z;
        for (var i = 0f; i < size; i+=density) {
            
        }
        
        gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
        
    }
}