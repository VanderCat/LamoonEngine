using System.Numerics;

namespace Lamoon;

public static partial class Graphics {
    private static Stack<Matrix4x4> _transformStack = new([new Matrix4x4()]);
    
    public static void PushTransform() => _transformStack.Push(_transformStack.Last());
    
    public static void PopTransform() {
        if (_transformStack.Count < 2) return;
        _transformStack.Pop();
    }

    public static void ApplyTransform(Transform transform) {
        _transformStack.Push(new Transform(_transformStack.Pop()).Apply(transform).Matrix);
    }

    public static Vector3 InverseTransformPoint(Vector3 point) =>
        new Transform(_transformStack.Peek()).InverseTransformPoint(point);

    public static Vector2 InverseTransformPoint(Vector2 point) {
        var vec = InverseTransformPoint(new Vector3(point, 0));
        return new Vector2(vec.X, vec.Y);
    }
}