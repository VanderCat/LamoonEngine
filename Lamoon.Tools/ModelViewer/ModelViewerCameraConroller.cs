using System.Numerics;
using NekoLib.Core;

namespace Lamoon.Tools.ModelViewer; 

internal class ModelViewerCameraConroller : Behaviour {
    public Transform FocusPoint;
    public float Distance = 5f;
    public Vector2 OrbitAngles = new(0,0);
    public float MouseSensetivity = 1f;

    void LateUpdate() {
        var io = ImGui.GetIO();
        if (io.MouseDown[2]) {
            OrbitAngles.X += float.DegreesToRadians(-io.MouseDelta.X * MouseSensetivity);
            OrbitAngles.Y += float.DegreesToRadians(-io.MouseDelta.Y * MouseSensetivity);
        }

        if (io.MouseDown[1]) {
            Distance += float.DegreesToRadians(io.MouseDelta.Y * MouseSensetivity);
        }
        var lookRotation =
            Quaternion.CreateFromAxisAngle(Vector3.UnitY, OrbitAngles.X) *
            Quaternion.CreateFromAxisAngle(Vector3.UnitX, OrbitAngles.Y);
        Vector3 lookDirection = Vector3.Transform(-Vector3.UnitZ, lookRotation);
        Vector3 lookPosition = FocusPoint.Position - lookDirection * Distance;
        Transform.LocalPosition = lookPosition;
        Transform.LocalRotation = lookRotation;
    }
}