using System.Numerics;
using Lamoon.Graphics;
using Lamoon.Graphics.Skia;
using NekoLib.Core;
using Silk.NET.OpenGL;
using SkiaSharp;
using Framebuffer = Lamoon.Graphics.Framebuffer;
using Renderbuffer = Lamoon.Graphics.Renderbuffer;
using Shader = Lamoon.Graphics.Shader;

namespace Lamoon.Engine.Components; 

[ToolsIcon(MaterialIcons.Photo_size_select_large)]
public class SkiaCanvas : Behaviour {
    private Framebuffer _fbo;
    private Renderbuffer _rbo;
    public static Graphics.Texture renderTexture;
    private GRBackendRenderTarget _grBackendRenderTarget;
    private SKSurface _skSurface;
    public SKCanvas Canvas;

    public SkiaCanvas() {
        RecreateCanvas();
    }

    void RecreateCanvas() {
        _fbo = new Framebuffer();
        var size = GraphicsReferences.ScreenSize;
        _rbo = new Renderbuffer(size);
        renderTexture = new Graphics.Texture(size);
        _fbo.SetRenderTexture(renderTexture);
        _fbo.SetRenderBuffer(_rbo);
        _fbo.Bind();
        var sampleCounts = 1;
        GraphicsReferences.OpenGl.GetFramebufferAttachmentParameter(
            FramebufferTarget.Framebuffer, 
            FramebufferAttachment.DepthStencilAttachment, 
            FramebufferAttachmentParameterName.StencilSize, 
            out var stencilBits);
        _fbo.Unbind();
        _grBackendRenderTarget = new GRBackendRenderTarget(size.Width, size.Height, 0, stencilBits, new GRGlFramebufferInfo(_fbo.OpenGlHandle, 0x8058) );
        _skSurface = SKSurface.Create(Skia.Instance.GrContext, _grBackendRenderTarget, GRSurfaceOrigin.BottomLeft,
            SKColorType.Rgba8888);
        Canvas = _skSurface.Canvas;
        /*
        var test = new GRBackendTexture(size.Width, size.Height, false,
            new GRGlTextureInfo((uint)renderTexture.Type, renderTexture.OpenGlHandle, (uint)renderTexture.Format));
        _skSurface = SKSurface.Create(Skia.Instance.GrContext, test, GRSurfaceOrigin.TopLeft, sampleCounts, 0x5085);*/
    } 
    
    unsafe void DrawGui() {
        var gl = GraphicsReferences.OpenGl;
        Skia.Instance.GrContext.ResetContext(GRGlBackendState.All);
        
        _fbo.Bind();
        var canvas = Canvas;
        
        canvas.Clear(new SKColor(0,0,0,0));
        Broadcast("SkiaDraw", canvas);
        canvas.Flush();
        _fbo.Unbind();

        
        gl.BindVertexArray(Mesh.Quad.VaoHandle);
        var shader = Shader.Default;
        shader.SetMatrix4x4("transform", Matrix4x4.Identity);
        shader.SetMatrix4x4("view", Matrix4x4.Identity);
        shader.SetMatrix4x4("projection", Matrix4x4.Identity);
        Immedieate.UseShader(shader);
        Immedieate.BindTexture(renderTexture);
        
        gl.DepthMask(false);
        gl.DepthFunc(DepthFunction.Always); 
        gl.DrawElements(Mesh.Quad.PrimitiveType, (uint)Mesh.Quad.Indices.Length, DrawElementsType.UnsignedInt, (void*) 0);
        gl.DepthMask(true);
        gl.DepthFunc(DepthFunction.Less);
        gl.BindVertexArray(0);
    }
}