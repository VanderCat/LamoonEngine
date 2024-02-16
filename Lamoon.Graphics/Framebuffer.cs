using Silk.NET.OpenGL;

namespace Lamoon.Graphics; 

public class Framebuffer : IDisposable {
    public uint OpenGlHandle;
    private Renderbuffer? rb;
    private Texture? RenderTexture;

    public Framebuffer() {
        var gl = GraphicsReferences.OpenGl;
        OpenGlHandle = gl.GenFramebuffer();
    }

    public void Bind() {
        var gl = GraphicsReferences.OpenGl;
        gl.BindFramebuffer(FramebufferTarget.Framebuffer, OpenGlHandle);
    }

    public void Unbind() {
        var gl = GraphicsReferences.OpenGl;
        gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public void SetRenderBuffer(Renderbuffer renderbuffer) {
        rb = renderbuffer;
        var gl = GraphicsReferences.OpenGl;
        Bind();
        gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rb.OpenGlHandle);
        Unbind();
    }

    public void SetRenderTexture(Texture texture) {
        RenderTexture = texture;
        var gl = GraphicsReferences.OpenGl;
        Bind();
        gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texture.OpenGlHandle, 0);
        Unbind();
    }

    public void Dispose() {
        var gl = GraphicsReferences.OpenGl;
        gl.DeleteFramebuffers(1, OpenGlHandle);  
    }
}