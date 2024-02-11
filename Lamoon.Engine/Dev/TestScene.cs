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