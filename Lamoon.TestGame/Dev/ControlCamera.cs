using System.Numerics;
using Lamoon.Engine;
using NekoLib.Core;
using Serilog;
using Silk.NET.Input;
using Silk.NET.Input.Glfw;
using Silk.NET.Windowing.Glfw;

namespace Lamoon.TestGame.Dev; 

public class ControlCamera : Behaviour {
    public float mouseSensetivity = 1f;
    public float yaw;
    public float pitch;

    private IKeyboard kb;
    void Awake() {
        NekoGame.Instance.InputContext.Mice[0].MouseMove += OnMouseMove;
        kb = NekoGame.Instance.InputContext.Keyboards[0];
    }
    
    void Update() {
        if (Game.IsToolsOpened) return;
        mouseDelta = newMousePosition - mousePosition;
        mousePosition = newMousePosition;
        yaw += float.DegreesToRadians(-mouseDelta.X*mouseSensetivity);
        pitch += float.DegreesToRadians(-mouseDelta.Y*mouseSensetivity);
        pitch = Math.Clamp(pitch, -MathF.PI/2, MathF.PI/2);
        Transform.LocalRotation =
            Quaternion.CreateFromAxisAngle(Vector3.UnitY, yaw) *
            Quaternion.CreateFromAxisAngle(Vector3.UnitX, pitch);
        if (kb.IsKeyPressed(Key.W)) {
            Transform.LocalPosition += Transform.Forward*Time.DeltaF;
        }
        if (kb.IsKeyPressed(Key.S)) {
            Transform.LocalPosition += Transform.Backward*Time.DeltaF;
        }
        if (kb.IsKeyPressed(Key.D)) {
            Transform.LocalPosition += Transform.Right*Time.DeltaF;
        }
        if (kb.IsKeyPressed(Key.A)) {
            Transform.LocalPosition += Transform.Left*Time.DeltaF;
        }
    }

    public Vector2 mouseDelta = Vector2.Zero;
    public Vector2 newMousePosition = Vector2.Zero;
    public Vector2 mousePosition = Vector2.Zero;
    void OnMouseMove(IMouse mouse, Vector2 pos) {
        newMousePosition = pos;
    }
}