using System.Numerics;
using JoltPhysicsSharp;
using NekoLib.Core;

namespace Lamoon.Physics.Shapes; 

public class BoxCollision : Component, IShapeComponent {
    internal Shape Shape { get; set; }

    Shape IShapeComponent.Shape {
        get => Shape;
        set => Shape = value;
    }

    private BoxShapeSettings _settings;
    public Vector3 HalfExtent = Vector3.One;
    public float ConvexRadius = 0f;

    void Awake() {
        _settings = new BoxShapeSettings(Transform.LocalScale*HalfExtent, ConvexRadius);
        Shape = new BoxShape(_settings);
    }
}