using System.Numerics;
using Lamoon.Engine;
using NekoLib.Core;
using Serilog;

namespace FlappyBird; 

public class LogoAnim : Behaviour {
    public Vector2 pos;
    
    void Update() {
        Transform.LocalPosition = new Vector3(pos.X+MathF.Sin(Time.CurrentTimeF)*128,pos.Y+MathF.Cos(Time.CurrentTimeF*4)*48, Transform.LocalPosition.Z);
    }
}