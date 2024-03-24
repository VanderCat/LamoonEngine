using System.Numerics;
using Lamoon.Engine;
using Lamoon.Engine.Components;
using Lamoon.Scripting;
using Lamoon.Scripting.Lua;
using NekoLib.Core;
using NekoLib.Scenes;
using NLua;

namespace Lamoon.TestGame.ScriptingTest; 

public class ScriptScene : IScene {
    public string Name => "ScriptableScene";
    public bool DestroyOnLoad => false;
    public int Index { get; set; }

    public List<GameObject> _gameObjects = new();
    public List<GameObject> GameObjects => _gameObjects;
    public void Initialize() {
        ScriptingManager.ScriptingProvider = new LuaScriptingProvider();
        var go = new GameObject();
        var sb = go.AddComponent<ScriptableBehaviour>();
        sb.ScriptPath = "Scripts/test.lua";
        sb.Name = "TestScript";
        sb.Init();
        ((LuaTable) sb.ScriptBlob)["this"] = sb;

        var model = Model.Spawn("models/character_soldier");
        model.Transform.Parent = go.Transform;
        model.Transform.LocalScale = new Vector3(0.1f);
        
        var camera = new GameObject("Camera");
        var cameraComponent = camera.AddComponent<Camera>();
        cameraComponent.IsMain = true;
        
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
            gameObject.Update();
        }
        
    }
}