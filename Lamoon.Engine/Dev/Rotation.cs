using System.Numerics;
using NekoLib.Core;

namespace Lamoon.Engine.Dev; 

public class Rotation : Behaviour{
    void Update() {
        Transform.LocalRotation 
            = Quaternion.CreateFromAxisAngle(
                new Vector3(1, 0, 0), 
                float.DegreesToRadians(Time.CurrentTimeF*16)
            );
    }
}