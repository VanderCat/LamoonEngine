using System.Drawing;
using System.Numerics;
using System.Reflection;

namespace Lamoon.Graphics; 

public class Material(Texture texture, Shader shader) {
    public static Material Default = new(Texture.Missing, Shader.Default);
    
    public List<Texture> Textures = new[]{texture}.ToList();

    public Shader Shader {
        get {
            return shader;
        }
        set {
            shader = value;
            UnformValues.Clear();
        }
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
    
}