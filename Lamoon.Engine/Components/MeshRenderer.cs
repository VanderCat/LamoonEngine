using System.Numerics;
using Lamoon.Graphics;
using NekoLib.Core;
using Serilog;
using Silk.NET.OpenGL;
using Shader = Lamoon.Graphics.Shader;

namespace Lamoon.Engine.Components; 

public class MeshRenderer : Behaviour {
    public Material Material = Material.Default;
    public Mesh Mesh;
    
    unsafe void Render() {
        if (Camera.CurrentlyDrawing == null) return;
        var gl = GraphicsReferences.OpenGl;
        gl.Enable(EnableCap.DepthTest);
        /*gl.BindVertexArray(Mesh.VaoHandle);
        Material.Bind();
        //Material.SetUniform("model", Transform.GlobalMatrix);
        //Material.SetUniform("view", Camera.CurrentlyDrawing.Transform.GlobalMatrix);
        //Material.SetUniform("projection", Camera.CurrentlyDrawing.ProjectionMatrix);
        Immedieate.PrintError();
        gl.DrawElements(Mesh.PrimitiveType, (uint)Mesh.Indices.Length, DrawElementsType.UnsignedInt, (void*) 0);*/
        gl.BindVertexArray(Mesh.VaoHandle);
        gl.BindBuffer(GLEnum.ArrayBuffer, Mesh.VboHandle);
        var shader = Material.Shader;
        shader.SetMatrix4x4("transform", Transform.GlobalMatrix); 
        shader.SetMatrix4x4("view", Camera.CurrentlyDrawing.ViewMatrix);
        shader.SetMatrix4x4("projection", Camera.CurrentlyDrawing.ProjectionMatrix);
        Immedieate.UseShader(shader);
        Immedieate.BindTexture(Material.Textures[0]);
        
        //gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
        gl.DrawElements(Mesh.PrimitiveType, (uint)Mesh.Indices.Length, DrawElementsType.UnsignedInt, (void*) 0);
        gl.BindVertexArray(0);
    }
}