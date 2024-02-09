using System.Reflection;
using Silk.NET.OpenGL;

namespace Lamoon.Graphics; 

public class Shader {
    public uint OpenGlHandle;

    public static Shader Default;

    static Shader() {
        var assembly = Assembly.GetAssembly(typeof(Texture));
        var streamFrag = assembly
            .GetManifestResourceStream("Lamoon.Graphics.Shaders.base.frag");
        var streamVert = assembly
            .GetManifestResourceStream("Lamoon.Graphics.Shaders.base.vert");
        var a = assembly.GetManifestResourceNames();
        using var streamReader = new StreamReader(streamFrag);
        var fragCode = streamReader.ReadToEnd();
        
        using var streamReader2 = new StreamReader(streamVert);
        var vertCode = streamReader2.ReadToEnd();

        Default = new Shader(new UnlinkedShader(ShaderType.VertexShader, vertCode),
            new UnlinkedShader(ShaderType.FragmentShader, fragCode));
    }
    public Shader(UnlinkedShader vertex, UnlinkedShader fragment) {
        var gl = GraphicsReferences.OpenGl;
        AssertType(vertex, ShaderType.VertexShader);
        AssertType(fragment, ShaderType.FragmentShader);
        OpenGlHandle = gl.CreateProgram();

        gl.AttachShader(OpenGlHandle, vertex.OpenGlHandle);
        gl.AttachShader(OpenGlHandle, fragment.OpenGlHandle);
        
        gl.LinkProgram(OpenGlHandle);
        if (!CheckShader())
            throw new Exception("Program failed to link: " + gl.GetProgramInfoLog(OpenGlHandle));
        
        gl.DetachShader(OpenGlHandle, vertex.OpenGlHandle);
        gl.DetachShader(OpenGlHandle, fragment.OpenGlHandle);
    }

    private void AssertType(UnlinkedShader shader, ShaderType desired) {
        if (shader.Type != desired)
            throw new ArgumentException($"Provided Unlinked Shader has type {shader.Type} instead of {desired}");
    }
    
    public bool CheckShader() {
        GraphicsReferences.OpenGl.GetProgram(OpenGlHandle, ProgramPropertyARB.LinkStatus, out var status);
        return status == (int) GLEnum.True;
    }
}