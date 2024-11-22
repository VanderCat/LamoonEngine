using System.Drawing;

namespace Lamoon;

public class GraphicsState {
    private GraphicsState? _prevState;
    private bool _wireframe;

    internal GraphicsState(GraphicsState? prevState = null) {
        _prevState = prevState;
    }
    
    public Color BackgroundColor { get; set; }
    public Color BlendMode { get; set; }
    public Canvas Canvas { get; set; }
    public Color Color { get; set; }
    public ColorMask ColorMask { get; set; }
    public Filter DefaultMinFilter { get; set; }
    public Filter DefaultMaxFilter { get; set; }
    public bool DepthEnabled { get; set; }
    public TestMode DepthMode { get; set; }
    public TestMode StencilMode { get; set; }
    public float StencilValue { get; set; }
    public Font Font { get; set; }
    public VertexWinding FrontFaceWinding { get; set; }
    public float LineWidth { get; set; }
    public CullMode MeshCullMode { get; set; }
    public RectangleF Scissor { get; set; }
    public Shader Shader { get; set; }

    public bool Wireframe {
        get => _wireframe;
        set {
            _wireframe = value;
            if (_wireframe) {
                DebugFlags |= DebugFlags.Wireframe;
            }
        }
    }

    private DebugFlags _debugFlags = DebugFlags.None;
    private DebugFlags DebugFlags {
        get => _debugFlags;
        set {
            _debugFlags = value;
            set_debug((uint)value);
        }
    }

    internal void Apply(GraphicsState prevState) {
        if (Wireframe != prevState.Wireframe) {
            _debugFlags |= DebugFlags.Wireframe;
        }
        set_debug((uint)_debugFlags);
    }
}

public static partial class Graphics {
    private static Stack<GraphicsState> _stateStack = new();
    private static GraphicsState State => _stateStack.Peek();

    public static Color BackgroundColor {
        get => State.BackgroundColor;
        set => State.BackgroundColor = value;
    }

    public static Color BlendMode {
        get => State.BlendMode;
        set => State.BlendMode = value;
    }

    public static Canvas Canvas {
        get => State.Canvas;
        set => State.Canvas = value;
    }

    public static Color Color {
        get => State.Color;
        set => State.Color = value;
    }

    public static ColorMask ColorMask {
        get => State.ColorMask;
        set => State.ColorMask = value;
    }

    public static Filter DefaultMinFilter {
        get => State.DefaultMinFilter;
        set => State.DefaultMinFilter = value;
    }

    public static Filter DefaultMaxFilter {
        get => State.DefaultMaxFilter;
        set => State.DefaultMaxFilter = value;
    }

    public static bool DepthEnabled {
        get => State.DepthEnabled;
        set => State.DepthEnabled = value;
    }

    public static TestMode DepthMode {
        get => State.DepthMode;
        set => State.DepthMode = value;
    }

    public static TestMode StencilMode {
        get => State.StencilMode;
        set => State.StencilMode = value;
    }

    public static float StencilValue {
        get => State.StencilValue;
        set => State.StencilValue = value;
    }

    public static Font Font {
        get => State.Font;
        set => State.Font = value;
    }

    public static VertexWinding FrontFaceWinding {
        get => State.FrontFaceWinding;
        set => State.FrontFaceWinding = value;
    }

    public static float LineWidth {
        get => State.LineWidth;
        set => State.LineWidth = value;
    }

    public static CullMode MeshCullMode {
        get => State.MeshCullMode;
        set => State.MeshCullMode = value;
    }

    public static RectangleF Scissor {
        get => State.Scissor;
        set => State.Scissor = value;
    }

    public static Shader Shader {
        get => State.Shader;
        set => State.Shader = value;
    }
    
    public static bool Wireframe {
        get => State.Wireframe;
        set => State.Wireframe = value;
    }


    public static void PushState() {
        var prevSate = _stateStack.Peek();
        var newState = new GraphicsState(State);
        newState.Apply(prevSate);
        _stateStack.Push(newState);
    }

    public static void PopState() {
        if (_stateStack.Count < 2) return;
        var prev = _stateStack.Pop();
        _stateStack.Peek().Apply(prev);
    }
}