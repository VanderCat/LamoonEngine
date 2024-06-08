using Serilog;
using Serilog.Events;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Sdl.Android;

namespace AndroidClicker;

/// <summary>
/// Simple demo on how to use Silk on Android.
/// The code used is mostly identical to the one on OpenGL Tutorial 1.4 - Textures.
/// </summary>
[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : SilkActivity
{
    /// <summary>
    /// This is where the application starts.
    /// Note that when using net6-android, you do not need to have a main method.
    /// </summary>
    protected override void OnRun()
    {
        // Included assets should be loaded with the help of Android.Content.Res.AssetManager.
        // The included example shaders and texture have build action of "AndroidAsset".
        //FileManager.AssetManager = Assets;

        var options = ViewOptions.Default;
        // We need to tell Silk to use OpenGLES
        // Version 3.0 is supported by >90% of devices currently in use.
        // https://developer.android.com/about/dashboards#OpenGL
        options.API = new GraphicsAPI(ContextAPI.OpenGLES, ContextProfile.Compatability, ContextFlags.Default, new APIVersion(3, 0));
        var view = Silk.NET.Windowing.Window.GetView(options); // note also GetView, instead of Window.Create.
        
        var game = AndroidGame.Instance;
        game.View = view;
        game.WindowOptions = game.WindowOptions;
        game.BindCallbacks(view);
        
        view.Run();

        view.Dispose();
    }
}
    