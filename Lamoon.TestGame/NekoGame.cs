using Lamoon.Engine;
using Lamoon.Filesystem;
using Lamoon.Graphics;
using Lamoon.TestGame.Dev;
using Lamoon.Tools;
using Lamoon.Tools.ModelViewer;
using NekoLib.Core;
using NekoLib.Scenes;
using Serilog;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Shader = Lamoon.Graphics.Shader;
using Texture = Lamoon.Graphics.Texture;

namespace Lamoon.TestGame; 

public class NekoGame : Game {

    public new static NekoGame Instance {
        get {
            _instance ??= new NekoGame();
            return (NekoGame)_instance;
        }
    }

    public override void Load() {
        base.Load();
        
        var tools = true; //todo: make it program arg
        SceneManager.LoadScene(new PersistantScene());
        if (tools) LoadTools();
        SceneManager.LoadScene(new TestScene());
        //Util.LoadSceneFromFilesystem("Scenes/TestScene.lscene");
    }

    public void LoadTools() {
        var tools = new GameObject("EngineTools");
        var t = tools.AddComponent<ImguiToolsController>();
        t.InputContext = InputContext; //hack
        tools.Initialize();
    }
}