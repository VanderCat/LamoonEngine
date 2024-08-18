using System.Reflection;
using System.Runtime.CompilerServices;
using Lamoon.Filesystem;
using NekoLib.Filesystem;
using Serilog;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Size = System.Drawing.Size;

namespace Lamoon.Graphics; 

public class Texture : IDisposable {
    
    static internal uint _lastBoundTexture = 0;

    static public void Bind(uint handle, TextureTarget target) {
        if (_lastBoundTexture == handle) return;
        _lastBoundTexture = handle;
        GraphicsReferences.OpenGl.BindTexture(target, handle);
    }

    static public void Bind(Texture texture) {
        Bind(texture.OpenGlHandle, texture.Type);
    }

    static public void Unbind(TextureTarget target) {
        Bind(0, target);
    }
    
    public uint OpenGlHandle;
    public Size Size;

    private TextureWrapMode _wrapModeX;
    public TextureWrapMode WrapModeX {
        set {
            Bind(this);
            GraphicsReferences.OpenGl.TextureParameter(OpenGlHandle, TextureParameterName.TextureWrapS, (int)value);
            _wrapModeX = value;
            Unbind(Type);
        }
        get => _wrapModeX;
    }

    private TextureWrapMode _wrapModeY;
    public TextureWrapMode WrapModeY {
        set {
            Bind(this);
            GraphicsReferences.OpenGl.TextureParameter(OpenGlHandle, TextureParameterName.TextureWrapT, (int)value);
            _wrapModeY = value;
            Unbind(Type);
        }
        get => _wrapModeY;
    }

    private TextureMinFilter _minFilter;
    public TextureMinFilter MinFilter {
        set {
            Bind(this);
            GraphicsReferences.OpenGl.TextureParameter(OpenGlHandle, TextureParameterName.TextureMinFilter, (int)value);
            _minFilter = value;
            Unbind(Type);
        }
        get => _minFilter;
    }

    private TextureMagFilter _magFilter;
    public TextureMagFilter MagFilter {
        set {
            Bind(this);
            GraphicsReferences.OpenGl.TextureParameter(OpenGlHandle, TextureParameterName.TextureMagFilter, (int)value);
            _magFilter = value;
            Unbind(Type);
        }
        get => _magFilter;
    }
    
    public TextureTarget Type { get; }
    public InternalFormat Format { get; }


    public Texture(
        Size size, 
        InternalFormat internalFormat = InternalFormat.Rgba8,
        TextureTarget type = TextureTarget.Texture2D
        ) {
        var gl = GraphicsReferences.OpenGl;
        OpenGlHandle = gl.GenTexture();
        Type = type;
        Format = internalFormat;
        Bind(this);
        unsafe {
            gl.TexImage2D(Type, 0, internalFormat, (uint)size.Width,
                (uint)size.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, (void*) 0);
        }

        WrapModeX = TextureWrapMode.Repeat;
        WrapModeY = TextureWrapMode.Repeat;

        MinFilter = TextureMinFilter.LinearMipmapLinear;
        MagFilter = TextureMagFilter.Linear;

        Size = size;
    }
    
    public Texture(
        Size size, 
        byte[] data, 
        InternalFormat internalFormat = InternalFormat.Rgba8, 
        PixelFormat dataFormat = PixelFormat.Rgba,
        TextureTarget type = TextureTarget.Texture2D
    ) {
        var gl = GraphicsReferences.OpenGl;
        OpenGlHandle = gl.GenTexture();
        Type = type;
        Format = internalFormat;
        Bind(this);
        unsafe {
            fixed (byte* ptr = data) {
                gl.TexImage2D(Type, 0, internalFormat, (uint)size.Width,
                    (uint)size.Height, 0, dataFormat, PixelType.UnsignedByte, ptr);
            }
        }

        WrapModeX = TextureWrapMode.Repeat;
        WrapModeY = TextureWrapMode.Repeat;

        MinFilter = TextureMinFilter.LinearMipmapLinear;
        MagFilter = TextureMagFilter.Linear;
        
        Bind(this);
        gl.GenerateMipmap(Type);
        Unbind(Type);
        
        Size = size;
    }

    public static Texture Missing = FromStream(Assembly.GetAssembly(typeof(Texture)).GetManifestResourceStream("Lamoon.Graphics.Textures._missing_texture.png"));

    [Obsolete("Please use Texture.FromFileSystem, this is left in case if you need raw filesystem access")]
    public static Texture FromFile(string path) {
        try {
            using var img = Image.Load<Bgra32>(path);
            img.Mutate(x => x.Flip(FlipMode.Vertical));
            var pixelBytes = new byte[img.Width * img.Height * Unsafe.SizeOf<Bgra32>()];
            img.CopyPixelDataTo(pixelBytes);

            return new Texture(new Size(img.Size.Width, img.Size.Height), pixelBytes, dataFormat: PixelFormat.Bgra);
        }
        catch (Exception e) {
            Log.Error("Texture failed to load!\n"+e);
            return Missing;
        }
    }

    public static Texture FromStream(Stream stream) {
        try {
            using var img = Image.Load<Bgra32>(stream);
            img.Mutate(x => x.Flip(FlipMode.Vertical));
            var pixelBytes = new byte[img.Width * img.Height * Unsafe.SizeOf<Bgra32>()];
            img.CopyPixelDataTo(pixelBytes);

            return new Texture(new Size(img.Size.Width, img.Size.Height), pixelBytes, dataFormat:PixelFormat.Bgra);
        }
        catch (Exception e) {
            Log.Error("Texture failed to load!\n"+e);
            return Missing;
        }
    }
    
    public static Texture FromFilesystem(string path) {
        try {
            using var stream = Files.GetFile(path).GetStream();
            return FromStream(stream);
        }
        catch (Exception e) {
            Log.Error("Texture failed to load!\n"+e);
            return Missing;
        }
    }

    public void Dispose() {
        GraphicsReferences.OpenGl.DeleteTexture(OpenGlHandle);
        if (OpenGlHandle == _lastBoundTexture)
            _lastBoundTexture = 0;
    }
}