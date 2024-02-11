using System.Numerics;
using Lamoon.Graphics;
using NekoLib.Core;
using Serilog;
using Silk.NET.OpenGL;
using Texture = Lamoon.Graphics.Texture;
using Shader = Lamoon.Graphics.Shader;

namespace Lamoon.Engine.Dev; 

public class TestComponent : Behaviour {

    public Texture? _tex;
    public Shader? _shader;
    public Mesh _mesh;
    
    public event Action? onAwake;

    void Awake() {
        GraphicsReferences.OpenGl = Game.Instance.View.CreateOpenGL();
        _tex = Texture.FromFile("test.png");
        _shader = Shader.Default;
        _mesh = new Mesh(new[] {
                0.5f, 0.5f, 0.0f, 1.0f, 1.0f,
                0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
                -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
                -0.5f, 0.5f, 0.0f, 0.0f, 1.0f
            },
            new[] {
                0u, 1u, 3u,
                1u, 2u, 3u
            });
        Log.Information("Test");
        onAwake?.Invoke();
    }

    unsafe void Draw() {
        var gl = GraphicsReferences.OpenGl;
        gl.BindVertexArray(_mesh.VaoHandle);
        var shader = _shader ?? Shader.Default;
        shader.SetMatrix4x4("transform", Transform.GlobalMatrix);
        Immedieate.UseShader(shader);
        if (_tex is not null) Immedieate.BindTexture(_tex);
        
        gl.DrawElements(_mesh.PrimitiveType, (uint)_mesh.Indices.Length, DrawElementsType.UnsignedInt, (void*) 0);
    }
}