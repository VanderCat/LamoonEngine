using JoltPhysicsSharp;

namespace Lamoon.Physics.Shapes; 

public interface IShapeComponent {
    internal Shape Shape { get; set; }
}