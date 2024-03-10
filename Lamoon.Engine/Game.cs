using System.Diagnostics;
using System.Drawing;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices;
using Lamoon.Engine.Console;
using Lamoon.Filesystem;
using Serilog;
using Serilog.Events;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Lamoon.Graphics;
using Lamoon.Graphics.Skia;
using NekoLib.Scenes;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing.Glfw;
using Texture = Lamoon.Graphics.Texture;

namespace Lamoon.Engine;

public class Game {
    protected static Game? _instance;
    private Stopwatch sw;
    public IInputContext InputContext;

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
        GlfwWindowing.Use();
        
        const string outputTemplate = "{Timestamp:HH:mm:ss} [{Level}] {Name}: {Message}{Exception}{NewLine}";
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.Console(LogEventLevel.Verbose, outputTemplate)
            .WriteTo.File($"logs/lamoon{DateTime.Now:yy.MM.dd-hh.MM.ss}.log", LogEventLevel.Verbose, outputTemplate)
            .CreateLogger()
            .ForContext("Name", "LamoonEngine");
    }

    public IWindow Window;
    public IView View;
    
    public void InitializeWindow() {
        Glfw.GetApi().WindowHint(WindowHintBool.OpenGLDebugContext, true);
        Glfw.GetApi().WindowHint(WindowHintContextApi.ContextCreationApi, ContextApi.EglContextApi);
        Window = Silk.NET.Windowing.Window.Create(WindowOptions.Default with {
            Size = new(1280, 720),
            WindowBorder = WindowBorder.Fixed,
            PreferredStencilBufferBits = 8,
            PreferredBitDepth = new Vector4D<int>(8, 8, 8, 8)
        });
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

    public void RunWindow() {
        try { Window.Run(); }
        catch (Exception e) {
            Log.Fatal(e.ToString());
            Closing();
            Environment.Exit(-1);
        }
    }

    public virtual void FocusChanged(bool isFocused) {
        SceneManager.InvokeScene("FocusChanged", isFocused);
    }
    
    public virtual void Draw(double deltaTime) {
        ImguiController.Update((float) deltaTime);
        Immedieate.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        SceneManager.Draw();
        ImguiController.Render();
    }

    public ImGuiController ImguiController;
    public virtual void Load() {
        var assemblyGraphicsFs = new AssemblyFilesystem(typeof(Texture).Assembly);
        assemblyGraphicsFs.Mount();
        
        var assemblyFs = new AssemblyFilesystem(Assembly.GetExecutingAssembly());
        assemblyFs.Mount();

        sw = Stopwatch.StartNew();
        var gl = View.CreateOpenGL();
        Skia.GlContext = View.GLContext;

        GraphicsReferences.OpenGl = gl;
        GraphicsReferences.ScreenSize = new Size(View.FramebufferSize.X, View.FramebufferSize.Y);
        InputContext = View.CreateInput();
        #if DEBUG
        gl.Enable( EnableCap.DebugOutput );
        _openGlLogger = Log.Logger.ForContext("Name", "OpenGL");
        ImguiController = new ImGuiController(
            gl,
            View,
            InputContext
        );
       Console.Console.RegisterType<DefaultConsoleCommands>();
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
                    if (logLevel == LogEventLevel.Verbose  || logLevel == LogEventLevel.Debug)
                        return; // this is too spammy when using IMGUI on nvidia card
                    _openGlLogger.Write(logLevel, Marshal.PtrToStringAnsi(message)+"\n"+new StackTrace(1));
                },
                null);
        }
        #endif
    }
    
    public virtual void Closing() {
        sw.Stop();
    }

    public virtual void Update(double deltaTime) {
        Time.Delta = deltaTime;
        Time.CurrentTime = sw.Elapsed.TotalSeconds;

        Time.FixedAccumulator += Time.DeltaF;
        
        while ( Time.FixedAccumulator >= Time.FixedDelta )
        {
            SceneManager.InvokeScene("FixedUpdate");
            Time.FixedAccumulator -= Time.FixedDelta;
        }
        SceneManager.Update();
    }

    public virtual void FrameBufferResize(Vector2D<int> newSize) {
        GraphicsReferences.OpenGl.Viewport(Vector2D<int>.Zero, newSize);
        GraphicsReferences.ScreenSize = new Size(newSize.X, newSize.Y);
        SceneManager.InvokeScene("Resize", new Size(newSize.X, newSize.Y));
    }
}