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
    }

    unsafe void Draw() {
        var gl = GraphicsReferences.OpenGl;
        Immedieate.Clear(ClearBufferMask.ColorBufferBit);
        gl.BindVertexArray(_mesh.VaoHandle);
        Immedieate.UseShader(_shader??Shader.Default);
        if (_tex is not null) Immedieate.BindTexture(_tex);
        
        gl.DrawElements(_mesh.PrimitiveType, (uint)_mesh.Indices.Length, DrawElementsType.UnsignedInt, (void*) 0);
    }
}