using System.Numerics;
using Lamoon.Engine.Components;
using Lamoon.Graphics;
using NekoLib.Core;
using NekoLib.Scenes;

using Model = Lamoon.Engine.Model;


namespace Lamoon.TestGame.Dev; 

public class ImguiTestScene : IScene {
    public string Name => "TestScene";
    public bool DestroyOnLoad => false;
    public int Index { get; set; }

    public List<GameObject> _gameObjects = new();
    public List<GameObject> GameObjects => _gameObjects;
    public void Initialize() {
        var go = new GameObject();
        go.AddComponent<ImguiDemo>();
        
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