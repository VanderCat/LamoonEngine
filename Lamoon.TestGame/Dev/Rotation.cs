using System.Numerics;
using NekoLib.Core;
using Lamoon.Engine;

namespace Lamoon.TestGame.Dev; 

public class Rotation : Behaviour{
    void Update() {
        Transform.LocalRotation 
            = Quaternion.CreateFromAxisAngle(
                new Vector3(1, 0, 0), 
                float.DegreesToRadians(Time.CurrentTimeF*16)
            );
    }
}