using NekoLib.Core;
using NekoLib.Scenes;

namespace FlappyBird; 

public class BirdScene : IScene {
    public string Name => "Unnamed";
    public bool DestroyOnLoad => true;
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
}