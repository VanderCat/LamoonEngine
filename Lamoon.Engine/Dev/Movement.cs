using System.Numerics;
using NekoLib.Core;

namespace Lamoon.Engine.Dev; 

public class Movement : Behaviour {
    public Vector3 _position;

    void Awake() {
        _position = Transform.LocalPosition;
    }
    
    void Update() {
        Transform.LocalPosition = _position with {Y = _position.Y + MathF.Abs(MathF.Sin(Time.CurrentTimeF*4))};
    }
}