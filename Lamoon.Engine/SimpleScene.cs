using NekoLib.Core;
using NekoLib.Scenes;
using Serilog;

namespace Lamoon.Engine; 

public class SimpleScene : IScene {
    public virtual string Name { get; protected set; } = "Unnamed";
    public virtual bool DestroyOnLoad { get; set; } = true;
    public int Index { get; set; }
    public List<GameObject> GameObjects { get; } = new();
    public virtual void Initialize() {
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

    public void Dispose() {
        Log.Debug("Disposing scene");
        var currentGameObjects = new GameObject[GameObjects.Count];
        GameObjects.CopyTo(currentGameObjects);
        foreach (var gameObject in currentGameObjects) {
            gameObject.Dispose();
        }
    }
}