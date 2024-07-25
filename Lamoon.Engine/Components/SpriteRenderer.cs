using System.Numerics;
using Lamoon.Graphics;
using NekoLib.Core;
using Silk.NET.OpenGL;

namespace Lamoon.Engine.Components; 

[ToolsIcon(MaterialIcons.Image)]
public class SpriteRenderer : Behaviour {
    public Sprite Sprite = Sprite.Default;
    unsafe void Render() {
        if (Camera.CurrentlyDrawing == null) return;
        var gl = GraphicsReferences.OpenGl;
        gl.Enable(EnableCap.DepthTest);
        gl.BindVertexArray(Mesh.Quad.VaoHandle);
        gl.BindBuffer(GLEnum.ArrayBuffer, Mesh.Quad.VboHandle);
        var shader = Sprite.Material.Shader;
        shader.SetMatrix4x4("transform",
            Matrix4x4.CreateBillboard(
                Transform.Position,
                Camera.CurrentlyDrawing.Transform.Position,
                Camera.CurrentlyDrawing.Transform.Up,
                Camera.CurrentlyDrawing.Transform.Forward)); 
        shader.SetMatrix4x4("view", Camera.CurrentlyDrawing.ViewMatrix);
        shader.SetMatrix4x4("projection", Camera.CurrentlyDrawing.ProjectionMatrix);
        try {
            shader.SetFloat("uTime", Time.CurrentTimeF);
        }
        catch (ArgumentException e) { }

        Immedieate.UseShader(shader);
        if (Sprite.Material.Textures.Count > 0)
            Immedieate.BindTexture(Sprite.Material.Textures[0]);
        
        //gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
        gl.DrawElements(Mesh.Quad.PrimitiveType, (uint)Mesh.Quad.Indices.Length, DrawElementsType.UnsignedInt, (void*) 0);
        gl.BindVertexArray(0);
    }
}