using System.Numerics;
using Lamoon.Graphics;
using NekoLib.Core;
using NekoLib.Scenes;

namespace Lamoon.Engine.Dev; 

public class TestScene : IScene {
    public string Name => "TestScene";
    public bool DestroyOnLoad => false;
    public int Index { get; set; }

    public List<GameObject> _gameObjects = new();
    public List<GameObject> GameObjects => _gameObjects;
    public void Initialize() {
        var a = new GameObject();
        a.AddComponent<TestComponent>();
        a.AddComponent<Movement>();
        var help = new GameObject();
        var b = help.AddComponent<TestComponent>();
        b.onAwake += () => {
            b._tex = Texture.FromFileSystem("Textures/test1.png");
        };
        help.Transform.LocalPosition = new Vector3(0.5f, -1f, 1f);
        help.Transform.Parent = a.Transform;
        a.Transform.LocalRotation = Quaternion.CreateFromYawPitchRoll(0,Single.DegreesToRadians(20), Single.DegreesToRadians(45));

        foreach (var gameObject in GameObjects) {
            gameObject.Initialize();
        }
    }

    public void Update() {
        foreach (var gameObject in GameObjects) {
            gameObject.Update();
        }
    }

    public void Draw() {
        foreach (var gameObject in GameObjects) {
            gameObject.Draw();
            
        }
    }
}