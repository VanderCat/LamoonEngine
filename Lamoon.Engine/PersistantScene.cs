using NekoLib.Core;
using NekoLib.Scenes;

namespace Lamoon.Engine; 

public class PersistantScene : IScene {
    public string Name => "DontDestroyOnLoad";
    public bool DestroyOnLoad => false;
    public int Index { get; set; }
    public List<GameObject> GameObjects { get; set; } = new();
    public void Initialize() {
        
    }

    public void Update() {
        foreach (var go in GameObjects) {
            go.Update();
        }
    }

    public void Draw() {
        foreach (var go in GameObjects) {
            go.Draw();
        }
    }
}