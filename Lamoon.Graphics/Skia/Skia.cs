using Serilog;
using Silk.NET.Core.Contexts;
using SkiaSharp;

namespace Lamoon.Graphics.Skia; 

public class Skia {

    private static ILogger Log = Serilog.Log.Logger.ForContext("Name", "Skia");
    
    public static Skia? _instance;

    public static Skia Instance {
        get {
            _instance ??= new Skia();
            return _instance;
        }
    }

    public static IGLContext GlContext;

    private GRGlInterface _gpuInterface;
    public GRContext GrContext;
    
    private Skia() {
        Log.Debug("Loading Skia");
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
        GrContext = GRContext.CreateGl(_gpuInterface);
        Log.Debug("Skia loaded successfully!");
    }
}