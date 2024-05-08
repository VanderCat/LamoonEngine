using Lamoon.Engine;
using Lamoon.Filesystem;
using Lamoon.Tools;
using Lamoon.Tools.ModelViewer;
using NekoLib.Core;
using NekoLib.Scenes;

namespace FlappyBird; 

public class BirdGame : Game {

    public new static BirdGame Instance {
        get {
            _instance ??= new BirdGame();
            return (BirdGame)_instance;
        }
    }

    public override void Load() {
        base.Load();
        var WorkFolder = new FolderFilesystem("Data");
        WorkFolder.Mount();

        var tools = false;
        SceneManager.LoadScene(new PersistantScene());
        if (tools) LoadTools();
        SceneManager.LoadScene(new MenuScene());
    }

    public void LoadTools() {
        var tools = new GameObject("EngineTools");
        var t = tools.AddComponent<ImguiToolsController>();
        t.InputContext = InputContext; //hack
        tools.Initialize();
    }
}