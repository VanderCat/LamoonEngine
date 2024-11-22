using Silk.NET.OpenGL;

namespace Lamoon;

public static unsafe partial class Graphics {
    public static Dictionary<PixelFormat, bool> GetPixelFormats()
        => throw new NotImplementedException();

    public static Capabilities GetCapabilities()
        => new(get_caps());
    

    //TODO: do not use bgfx types
    public static RendererType[] GetSupportedRenderers() {
        RendererType* type = null;
        var renderers = get_supported_renderers(0, type);
        var rendererSpan = new Span<RendererType>(type, sizeof(RendererType)*renderers);
        //possible memory leak? idk should i release span, or it will deal with memory by itself
        return rendererSpan.ToArray();
    }

    public static RenderStatistics GetStatistics()
        => throw new NotImplementedException();
}

//TODO: Do not use bgfx in public api
public unsafe class Capabilities {
    internal Capabilities(Caps* caps) {
        Type = caps->rendererType;
        Features = (CapsFlags)caps->supported;
        VendorId = caps->vendorId;
        DeviceId = caps->deviceId;
        HomogenousDepth = caps->homogeneousDepth == 1;
        GpuCount = caps->numGPUs;
        Formats = new Span<TextureFormat>(caps->formats, (int)TextureFormat.Count*sizeof(TextureFormat)).ToArray();
        Gpus = new Span<Caps.GPU>(caps->gpu, sizeof(Caps.GPU) * 4).ToArray();
        Limits = caps->limits;
    }
    public readonly RendererType Type;
    public readonly CapsFlags Features;
    public readonly string Version;
    public readonly uint VendorId;
    public readonly uint DeviceId;
    public readonly bool HomogenousDepth;
    public readonly uint GpuCount;
    public readonly TextureFormat[] Formats;
    public readonly Caps.GPU[] Gpus;
    public readonly Caps.Limits Limits;
}

public class RenderStatistics {
    public int Drawcalls;
    public int CanvasSwitches;
    public int TextureMemory;
    public int ImagesLoaded;
    public int CanvasesLoaded;
    public int FontsLoaded;
    public int ShaderSwitches;
    public int DrawcallsBatched;
}