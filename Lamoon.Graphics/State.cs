using System.Drawing;

namespace Lamoon;

public class GraphicsState {
    private GraphicsState? _prevState;

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
    public bool Wireframe { get; set; }
}

public static partial class Graphics {
    private static Stack<GraphicsState> _stateStack = new();
    public static GraphicsState State => _stateStack.Peek();
    
    public static void PushState() => _stateStack.Push(new GraphicsState(State));

    public static void PopState() {
        if (_stateStack.Count < 2) return;
        _stateStack.Pop();
    }
}