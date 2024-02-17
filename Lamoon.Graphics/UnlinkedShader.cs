using System.Reflection;
using Serilog;
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
            throw new Exception("Shader failed to compile: " + gl.GetShaderInfoLog(OpenGlHandle));

    }

    public bool CheckShader() {
        GraphicsReferences.OpenGl.GetShader(OpenGlHandle, ShaderParameterName.CompileStatus, out var vStatus);
        return vStatus == (int) GLEnum.True;
    }

    public static UnlinkedShader DefaultVertex {
        get {
            using var stream = Assembly.GetAssembly(typeof(UnlinkedShader))
                .GetManifestResourceStream("Lamoon.Graphics.Shaders.base.vert");
            using var streamReader = new StreamReader(stream);
            var code = streamReader.ReadToEnd();
            return new UnlinkedShader(ShaderType.VertexShader, code);
        }
    }
    public static UnlinkedShader DefaultFragment {
        get {
            using var stream = Assembly.GetAssembly(typeof(UnlinkedShader))
                .GetManifestResourceStream("Lamoon.Graphics.Shaders.base.frag");
            using var streamReader = new StreamReader(stream);
            var code = streamReader.ReadToEnd();
            return new UnlinkedShader(ShaderType.FragmentShader, code);
        }
    }

    public void Dispose() {
        Log.Verbose("Disposed shader {0}",OpenGlHandle);
        GraphicsReferences.OpenGl.DeleteShader(OpenGlHandle);
    }
}