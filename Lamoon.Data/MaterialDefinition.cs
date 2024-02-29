using System.Numerics;

namespace Lamoon.Data; 

public class MaterialDefinition : Definition {
    public string Shader = "Shaders/Default";
    public string[]? Textures;
    public Vector4? Color;
    public bool BackfaceCulling = true;

    public Dictionary<string, string> Uniform = new();
}