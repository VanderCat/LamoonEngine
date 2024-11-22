using System.Runtime.CompilerServices;
using Serilog;
using Silk.NET.Core.Contexts;
using SkiaSharp;

namespace Lamoon;

public unsafe static partial class Graphics {
    public static IGLContext GlContext;
    private static GRGlInterface _gpuInterface;
    private static GRContext _grContext;
    
    private static void InitializeSkia() {
        _gpuInterface =  GRGlInterface.CreateOpenGl(name => {
            Log.Verbose("Tring to get opengl with name: {name}", name);
            IntPtr result;
            try {
                result = GlContext.GetProcAddress(name);
            }
            catch (Exception e) {
                Log.Verbose("{name} was not found!", name);
                result = IntPtr.Zero;
            }
            
            return result;
        });
        if (!_gpuInterface.Validate()) {
            throw new Exception("Skia could not create an GRGlInterface");
        }
        Log.Verbose("Created gpu interface");
        _grContext = GRContext.CreateGl(_gpuInterface);
        Log.Debug("Skia loaded successfully!");
    }
    
    public static void Initialize(bool debug, bool profile, nint windowHandle = 0, nint context = 0) {
        var bgfxInit = new Init();
        init_ctor(&bgfxInit);
        bgfxInit.type = RendererType.OpenGL; //TODO: allow different renderer types

        bgfxInit.debug = (byte)(debug ? 1 : 0);
        bgfxInit.profile = (byte)(profile ? 1 : 0);
        bgfxInit.platformData.nwh = (void*)windowHandle;
        bgfxInit.platformData.context = (void*)context;
        init(&bgfxInit);
    }

    internal static void Reset(uint width, uint height, ResetFlags resetFlags = ResetFlags.None) {
        reset(width, height, (uint)resetFlags, TextureFormat.Count);
    }
}