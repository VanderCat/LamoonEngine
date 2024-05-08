using Lamoon.Engine;
using NekoLib.Core;
using NekoLib.Scenes;
using Serilog;
using Silk.NET.Input;

namespace FlappyBird; 

public class BirdBrains : Behaviour {
    public float Acceleration = 32f;
    public float JumpForce = 10f;
    public float Speed = 0f;
    
    private IKeyboard kb;
    private bool jumped = false;

    void Awake() {
        kb = BirdGame.Instance.InputContext.Keyboards[0];
    }
    
    void Update() {
        ApplyGravity();
        if (kb.IsKeyPressed(Key.W)) {
            if (!jumped) {
                Speed = -JumpForce;
                Log.Error("Jump!");
            }
            jumped = true;
        }
        else {
            jumped = false;
        }
        if (Transform.LocalPosition.Y > 720-64) SceneManager.LoadScene(new MenuScene());
        if (Transform.LocalPosition.Y < 0) SceneManager.LoadScene(new MenuScene());
    }

    void ApplyGravity() {
        Speed += Acceleration*Time.DeltaF;
        Transform.LocalPosition = Transform.LocalPosition with { Y = Transform.LocalPosition.Y + Speed };
    }
}