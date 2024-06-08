using System.Numerics;
using System.Reflection;
using Lamoon.Data;
using Lamoon.Filesystem;
using Serilog;
using Silk.NET.OpenGL;

namespace Lamoon.Graphics; 

public class Shader {
    public uint OpenGlHandle;

    public static Shader Default;

    static Shader() {
        var assembly = Assembly.GetAssembly(typeof(Texture));
        using var streamFrag = Files.GetFile("Shaders/base.frag").GetStream();
        using var streamVert = Files.GetFile("Shaders/base.vert").GetStream();
        
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

    private Dictionary<string, int> UniformCache = new();

    public void ClearUniformCache() {
        UniformCache.Clear();
    }

    public int GetUniformLocation(string name) {
        if (!UniformCache.ContainsKey(name))
            UniformCache[name] = GraphicsReferences.OpenGl.GetUniformLocation(OpenGlHandle, name);
        var info = "";
        
        if (UniformCache[name] == -1)
            throw new ArgumentException($"{name} uniform was not found on shader.");
        return UniformCache[name];
    }

    public unsafe void SetMatrix4x4(string name, Matrix4x4 matrix) {
        GraphicsReferences.OpenGl.UseProgram(OpenGlHandle);
        var location = GetUniformLocation(name);
        GraphicsReferences.OpenGl.UniformMatrix4(location, 1, false, (float*) &matrix);
    }
    public void SetVector2(string name, Vector2 vector) {
        GraphicsReferences.OpenGl.UseProgram(OpenGlHandle);
        var location = GetUniformLocation(name);
        GraphicsReferences.OpenGl.Uniform2(location, vector);
    }
    public void SetVector3(string name, Vector3 vector) {
        GraphicsReferences.OpenGl.UseProgram(OpenGlHandle);
        var location = GetUniformLocation(name);
        GraphicsReferences.OpenGl.Uniform3(location, vector);
    }
    public void SetVector4(string name, Vector4 vector) {
        GraphicsReferences.OpenGl.UseProgram(OpenGlHandle);
        var location = GetUniformLocation(name);
        GraphicsReferences.OpenGl.Uniform4(location, vector);
    }
    public void SetFloat(string name, float value) {
        GraphicsReferences.OpenGl.UseProgram(OpenGlHandle);
        var location = GetUniformLocation(name);
        GraphicsReferences.OpenGl.Uniform1(location, value);
    }

    public void SetUniform(string name, float value) {
        SetFloat(name, value);
    }
    public void SetUniform(string name, Vector2 value) {
        SetVector2(name, value);
    }
    public void SetUniform(string name, Vector3 value) {
        SetVector3(name, value);
    }
    public void SetUniform(string name, Vector4 value) {
        SetVector4(name, value);
    }
    public void SetUniform(string name, Matrix4x4 value) {
        SetMatrix4x4(name, value);
    }

    public bool HasUniform(string name) {
        if (!UniformCache.ContainsKey(name))
            try {
                GetUniformLocation(name);
                return true;
            }
            catch (ArgumentException e) {
                return false;
            }

        return UniformCache[name] != -1;
    }

    public static Shader FromDefinition(ShaderDefinition shaderDefinition) {
        var vertexPath = shaderDefinition.Vertex is null || !Files.FileExists(shaderDefinition.Vertex)
            ? "Shaders/base.vert"
            : shaderDefinition.Vertex;
        
        var vertexCode = Files.GetFile(vertexPath).Read();
        using var vertex = new UnlinkedShader(ShaderType.VertexShader, vertexCode);
        
        var fragmentPath = shaderDefinition.Fragment is null || !Files.FileExists(shaderDefinition.Fragment)
            ? "Shaders/base.vert"
            : shaderDefinition.Fragment;
        
        var fragmentCode = Files.GetFile(fragmentPath).Read();
        using var fragment = new UnlinkedShader(ShaderType.FragmentShader, fragmentCode);
        
        if (shaderDefinition.Compute is not null) {
            Log.Warning("{Name} shaders are not supported yet!", nameof(shaderDefinition.Compute));
        }
        if (shaderDefinition.Geometry is not null) {
            Log.Warning("{Name} shaders are not supported yet!", nameof(shaderDefinition.Geometry));
        }
        if (shaderDefinition.Tesselation is not null) {
            Log.Warning("{Name} shaders are not supported yet!", nameof(shaderDefinition.Tesselation));
        }
        
        return new Shader(vertex, fragment);
    }

    public static Shader FromFilesystem(string path) {
        if (!path.EndsWith(".lshdr")) path += ".lshdr";
        ShaderDefinition definition;
        if (Files.FileExists(path)) {
            using var stream = Files.GetFile(path).GetStream();
            definition = Definition.FromStream<ShaderDefinition>(stream);
        }
        else {
            Log.Error("{Shader} does not exist!", path);
            definition = new ShaderDefinition();
        }

        return FromDefinition(definition);
    }
}