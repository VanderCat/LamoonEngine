using System.Numerics;

namespace Lamoon.Engine; 

public struct Ray {
    public Vector3 Direction;
    public Vector3 Origin;

    public Vector3 GetPoint(float distance) {
        return Origin + Vector3.Normalize(Direction) * distance;
    }

    public override string ToString() => $"Ray[{Direction} at {Origin}]";
}