using System.Drawing;
using System.Numerics;
using System.Reflection;
using Lamoon.Data;
using NekoLib.Filesystem;
using Serilog;

namespace Lamoon.Graphics; 

public class Material {
    public static Material Default = new(Texture.Missing, Shader.Default);
    
    public List<Texture> Textures;

    private Shader _shader;
    public Shader Shader {
        get {
            return _shader;
        }
        set {
            _shader = value;
            UnformValues.Clear();
        }
    }

    public Material(Texture texture, Shader shader) {
        Textures = new[] {texture}.ToList();
        _shader = shader;
    }

    private Dictionary<string, object> UnformValues = new();
    public Color Color;
    public bool DoubleSided;
    

    public void Bind() {
        for (int i = 0; i < Textures.Count; i++) {
            Immedieate.BindTexture(Textures[i], (uint)i);
        }

        foreach (var value in UnformValues) {
            var method = Shader.GetType().GetMethod("SetUniform", BindingFlags.Instance | BindingFlags.Public,
                new[] { typeof(string), value.Value.GetType() });
            method?.Invoke(Shader, new[] { value.Key, value.Value });
        }
    }

    public void SetUniform<T>(string name, T value) {
        if (!Shader.HasUniform(name))
            throw new ArgumentException("Specified Uniform has not been found");
        UnformValues[name] = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Material(Shader shader) {
        Textures = new List<Texture>();
        _shader = shader;
    }

    public static Material FromDefinition(MaterialDefinition materialDefinition) {
        var shader = Shader.FromFilesystem(materialDefinition.Shader);
        var material = new Material(shader);
        if (materialDefinition.Textures is not null)
            foreach (var texturePath in materialDefinition.Textures) {
                material.Textures.Add(Texture.FromFilesystem(texturePath));
            }
        
        return material;
    }

    public static Material FromFilesystem(string path) {
        if (!path.EndsWith(".lmat")) path += ".lmat";
        MaterialDefinition definition;
        if (Files.FileExists(path)) {
            using var stream = Files.GetFile(path).GetStream();
            definition = Definition.FromStream<MaterialDefinition>(stream);
        }
        else {
            Log.Error("{Material} does not exist!", path);
            return Default;
        }

        return FromDefinition(definition);
    }
}