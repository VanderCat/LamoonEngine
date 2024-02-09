using Serilog;
using Serilog.Events;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Sdl;
using Lamoon.Graphics;

namespace Lamoon.Engine;

public class Game {
    protected static Game? _instance;

    public static Game Instance {
        get {
            _instance ??= new Game();
            return _instance;
        }
    }
    
    private Graphics.Texture _tex;
    private Graphics.Shader _shader;
    private Graphics.Mesh _mesh;
    
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
        var gl = GraphicsReferences.OpenGl;
        _mesh.Draw(_tex);
    }

    public void Load() {
        GraphicsReferences.OpenGl = View.CreateOpenGL();
        _tex = Graphics.Texture.FromFile("test.png");
        _shader = Graphics.Shader.Default;
        _mesh = new Mesh(new[] {
                0.5f, 0.5f, 0.0f, 1.0f, 1.0f,
                0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
                -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
                -0.5f, 0.5f, 0.0f, 0.0f, 1.0f
            },
            new[] {
                0u, 1u, 3u,
                1u, 2u, 3u
            });
    }
    
    public void Closing() {
        
    }

    public void Update(double deltaTime) {
        Time.Delta = deltaTime;
    }

    public void FrameBufferResize(Vector2D<int> newSize) {
        
    }
}