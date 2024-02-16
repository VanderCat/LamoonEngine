using System.Drawing;
using Silk.NET.OpenGL;

namespace Lamoon.Graphics; 

public class Renderbuffer : IDisposable {
    public uint OpenGlHandle;

    public Renderbuffer(Size size, InternalFormat format = InternalFormat.Depth24Stencil8) {
        OpenGlHandle = GraphicsReferences.OpenGl.CreateRenderbuffer();
        Bind();
        GraphicsReferences.OpenGl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, format, (uint)size.Width, (uint)size.Height);
        Unbind();
    }

    public void Bind() {
        GraphicsReferences.OpenGl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, OpenGlHandle);
    }

    public void Unbind() {
        GraphicsReferences.OpenGl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
    }

    public void Dispose() {
        GraphicsReferences.OpenGl.DeleteRenderbuffer(OpenGlHandle);
    }
}