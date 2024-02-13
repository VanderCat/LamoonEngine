using System.Numerics;
using NekoLib.Core;

namespace Lamoon.Engine.Dev; 

public class Movement : Behaviour {
    private Vector3 _position;
    public float speed = 4f;
    public float scale = 1f;
    public float offset = 0f;

    void Awake() {
        _position = Transform.LocalPosition;
    }
    
    void Update() {
        Transform.LocalPosition = _position with {
            Z = _position.Z + MathF.Sin(Time.CurrentTimeF*speed+offset)*scale, 
            X = _position.X + MathF.Cos(Time.CurrentTimeF*speed+offset)*scale
        };
    }
}