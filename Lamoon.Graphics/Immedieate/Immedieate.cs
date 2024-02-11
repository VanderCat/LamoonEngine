using Silk.NET.OpenGL;

namespace Lamoon.Graphics; 

public static partial class Immedieate {

    private static GL gl => GraphicsReferences.OpenGl;
    public static void Clear(ClearBufferMask mask) {
        gl.Clear(mask);
    }

    public static void UseShader(uint handle) {
        gl.UseProgram(handle);
    }

    public static void UseShader(Shader shader) {
        UseShader(shader.OpenGlHandle);
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
        Console.WriteLine(oglerror);
    }
}