using System.Runtime.InteropServices;
using Lamoon.Engine.Dev;
using Serilog;
using Serilog.Events;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Sdl;
using Lamoon.Graphics;
using NekoLib.Scenes;
using Silk.NET.SDL;

namespace Lamoon.Engine;

public class Game {
    protected static Game? _instance;

    public static Game Instance {
        get {
            _instance ??= new Game();
            return _instance;
        }
    }

    private ILogger _openGlLogger;
    
    protected Game() {
    }
    
    static Game() {
        SdlWindowing.Use();
        
        const string outputTemplate = "{Timestamp:HH:mm:ss} [{Level}] {Name}: {Message}{Exception}{NewLine}";
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(LogEventLevel.Verbose, outputTemplate)
            .WriteTo.File($"logs/lamoon{DateTime.Now:yy.MM.dd-hh.MM.ss}.log", LogEventLevel.Debug, outputTemplate)
            .CreateLogger()
            .ForContext("Name", "LamoonEngine");
    }

    public IWindow Window;
    public IView View;
    
    public void InitializeWindow() {
        Window = Silk.NET.Windowing.Window.Create(WindowOptions.Default);
        SdlWindowing.GetExistingApi(Window).GLSetAttribute(GLattr.ContextFlags, (int)GLcontextFlag.DebugFlag);
    }

    public void BindCallbacks(IView view) {
        View = view;
        view.FramebufferResize += FrameBufferResize;
        view.Update += Update;
        view.Closing += Closing;
        view.Load += Load;
        view.Render += Draw;
        view.FocusChanged += FocusChanged;
    }

    public void RunWindow() => Window.Run();

    public void FocusChanged(bool isFocused) {
        
    }
    
    public void Draw(double deltaTime) {
        SceneManager.Draw();
    }

    public void Load() {
        var gl = View.CreateOpenGL();
        GraphicsReferences.OpenGl = gl;
        #if DEBUG
        _openGlLogger = Log.Logger.ForContext("Name", "OpenGL");
        unsafe {
            gl.DebugMessageCallback(
                (GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userparam) => {
                    var logLevel = ((DebugSeverity)severity) switch {
                        DebugSeverity.DontCare => LogEventLevel.Verbose,
                        DebugSeverity.DebugSeverityNotification => LogEventLevel.Debug,
                        DebugSeverity.DebugSeverityHigh => LogEventLevel.Error,
                        DebugSeverity.DebugSeverityMedium => LogEventLevel.Warning,
                        DebugSeverity.DebugSeverityLow => LogEventLevel.Information,
                        _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
                    };
                    _openGlLogger.Write(logLevel, "{type}/{id}: "+Marshal.PtrToStringAnsi(message), type.ToString().Substring(9), id);
                },
                null);
        }
        #endif
        SceneManager.LoadScene(new TestScene());
    }
    
    public void Closing() {
        
    }

    public void Update(double deltaTime) {
        Time.Delta = deltaTime;
        SceneManager.Update();
    }

    public void FrameBufferResize(Vector2D<int> newSize) {
        
    }
}