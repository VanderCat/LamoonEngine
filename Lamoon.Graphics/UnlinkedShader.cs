using Silk.NET.OpenGL;

namespace Lamoon.Graphics; 

public class UnlinkedShader : IDisposable {
    public ShaderType Type;
    public uint OpenGlHandle;

    public UnlinkedShader(ShaderType type, string code) {
        var gl = GraphicsReferences.OpenGl;
        Type =  type;
        OpenGlHandle = gl.CreateShader(Type);
        gl.ShaderSource(OpenGlHandle, code);
        gl.CompileShader(OpenGlHandle);
        
        
        if (!CheckShader())
            throw new Exception("Vertex shader failed to compile: " + gl.GetShaderInfoLog(OpenGlHandle));

    }

    public bool CheckShader() {
        GraphicsReferences.OpenGl.GetShader(OpenGlHandle, ShaderParameterName.CompileStatus, out var vStatus);
        return vStatus == (int) GLEnum.True;
    }

    public void Dispose() {
        GraphicsReferences.OpenGl.DeleteShader(OpenGlHandle);
    }
}