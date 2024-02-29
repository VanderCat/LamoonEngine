using Lamoon.Graphics;
using NekoLib.Core;
using Silk.NET.OpenGL;

namespace Lamoon.Engine.Components; 

public class ModelRenderer : Behaviour {
    public Material? MaterialOverride;

    public Graphics.Model? RenderModel;

    unsafe void Render() {
        if (Camera.CurrentlyDrawing == null) return;
        var model = RenderModel ?? Graphics.Model.Error;
        var gl = GraphicsReferences.OpenGl;
        gl.Enable(EnableCap.DepthTest);
        foreach (var mesh in model.AllMeshes) {
            gl.BindVertexArray(mesh.VaoHandle);
            gl.BindBuffer(GLEnum.ArrayBuffer, mesh.VboHandle);
            var material = MaterialOverride??model.Materials[mesh.MaterialIndex];
            material.SetUniform("transform", mesh.GlobalMatrix*Transform.GlobalMatrix);
            material.SetUniform("view", Camera.CurrentlyDrawing.ViewMatrix);
            material.SetUniform("projection", Camera.CurrentlyDrawing.ProjectionMatrix);
            try {
                material.SetUniform("uTime", Time.CurrentTimeF);
            }
            catch (ArgumentException e) { }

            material.Bind();
            if (material.Textures.Count > 0)
                Immedieate.BindTexture(material.Textures[0]);

            //gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
            gl.DrawElements(mesh.PrimitiveType, (uint)mesh.Indices.Length, DrawElementsType.UnsignedInt, (void*) 0);
        }
        gl.BindVertexArray(0);
    }
}