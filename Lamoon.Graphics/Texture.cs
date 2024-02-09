using System.Runtime.CompilerServices;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SkiaSharp;
using Size = System.Drawing.Size;

namespace Lamoon.Graphics; 

public class Texture {
    
    public uint OpenGlHandle;

    public TextureWrapMode WrapModeX {
        set => GraphicsReferences.OpenGl.TextureParameter(OpenGlHandle, TextureParameterName.TextureWrapS, (int) value);
    }
    public TextureWrapMode WrapModeY {
        set => GraphicsReferences.OpenGl.TextureParameter(OpenGlHandle, TextureParameterName.TextureWrapT, (int) value);
    }
    
    public TextureMinFilter MinFilter {
        set => GraphicsReferences.OpenGl.TextureParameter(OpenGlHandle, TextureParameterName.TextureMinFilter, (int) value);
    }
    public TextureMagFilter MagFilter {
        set => GraphicsReferences.OpenGl.TextureParameter(OpenGlHandle, TextureParameterName.TextureMagFilter, (int) value);
    }

    public Texture(Size size, byte[] data) {
        var gl = GraphicsReferences.OpenGl;
        OpenGlHandle = gl.GenTexture();
        gl.BindTexture(TextureTarget.Texture2D, OpenGlHandle);
        unsafe {
            fixed (byte* ptr = data) {
                gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)size.Width,
                    (uint)size.Height, 0, PixelFormat.Rgba, PixelType.Byte, ptr);
            }
        }

        WrapModeX = TextureWrapMode.Repeat;
        WrapModeY = TextureWrapMode.Repeat;

        MinFilter = TextureMinFilter.LinearMipmapLinear;
        MagFilter = TextureMagFilter.Linear;
        
        gl.GenerateMipmap(TextureTarget.Texture2D);
        gl.BindTexture(TextureTarget.Texture2D, 0);
    }

    public static Texture FromFile(string path) {
        using var img = Image.Load<Rgba32>(path);
        byte[] pixelBytes = new byte[img.Width * img.Height * Unsafe.SizeOf<Rgba32>()];
        img.CopyPixelDataTo(pixelBytes);

        return new Texture(new Size(img.Size.Width, img.Size.Height), pixelBytes);
    }
}