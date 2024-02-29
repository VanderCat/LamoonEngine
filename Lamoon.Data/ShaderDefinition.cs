using Serilog;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Lamoon.Data; 

public class ShaderDefinition : Definition {
    public string Type = "glsl";

    public string? Vertex;
    public string? Fragment;
    public string? Geometry;
    public string? Tesselation;
    public string? Compute;
}