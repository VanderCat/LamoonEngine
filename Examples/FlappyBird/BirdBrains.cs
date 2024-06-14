using Lamoon.Audio;
using Lamoon.Engine;
using Lamoon.Engine.Components;
using Lamoon.Filesystem;
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

    public AudioSource _jumpSfx;

    void Awake() {
        kb = BirdGame.Instance.InputContext.Keyboards[0];
        //_jumpSfx = GameObject.AddComponent<AudioSource>(); //FIXME: we absolutley could create components on gamobjects from components
        using var audioStream = Files.GetFile("Sounds/jump.ogg").GetStream();
        _jumpSfx.Track = new OggSoundFile(audioStream, false);
        _jumpSfx.IsLooping = false;
    }
    
    void Update() {
        ApplyGravity();
        if (kb.IsKeyPressed(Key.W)) {
            if (!jumped) {
                Speed = -JumpForce;
                _jumpSfx.Play();
            }
            jumped = true;
        }
        else {
            jumped = false;
        }
        if (Transform.LocalPosition.Y is > 720-64 or < 0)
            //SceneManager.LoadScene(new MenuScene()); Old Behaviour
            Util.LoadSceneFromFilesystem("Scenes/MenuScene.lscene");
    }

    void ApplyGravity() {
        Speed += Acceleration*Time.DeltaF;
        Transform.LocalPosition = Transform.LocalPosition with { Y = Transform.LocalPosition.Y + Speed };
    }
}