using Lamoon.Engine.Components;
using NekoLib.Core;
using NekoLib.Scenes;

namespace AndroidClicker; 

public class TestScene : IScene {
    public string Name => GetType().Name;
    public bool DestroyOnLoad => true;
    public int Index { get; set; }
    public List<GameObject> GameObjects { get; } = new();
    public virtual void Initialize() {
        //FIXME: skia does not work now
        //var go = new GameObject("test");
        //go.AddComponent<SkiaCanvas>();
        var another = new GameObject("meow");
        //another.Transform.Parent = go.Transform;
        another.AddComponent<TestMove>();
        var currentGameObjects = new GameObject[GameObjects.Count];
        GameObjects.CopyTo(currentGameObjects);
        foreach (var gameObject in currentGameObjects) {
            gameObject.Initialize();
        }
    }

    public virtual void Update() {
        var currentGameObjects = new GameObject[GameObjects.Count];
        GameObjects.CopyTo(currentGameObjects);
        foreach (var gameObject in currentGameObjects) {
            gameObject.Update();
        }
    }

    public virtual void Draw() {
        var currentGameObjects = new GameObject[GameObjects.Count];
        GameObjects.CopyTo(currentGameObjects);
        foreach (var gameObject in currentGameObjects) {
            gameObject.SendMessage("Draw");
        }
        
        foreach (var gameObject in currentGameObjects) {
            gameObject.SendMessage("DrawGui");
        }
    }
}