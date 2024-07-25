using Lamoon.Engine;
using Lamoon.Filesystem;
//using Lamoon.Tools;
//Lamoon.Tools.ModelViewer;
//using NekoLib.Core;
using NekoLib.Scenes;
using Console = Lamoon.Engine.Console.Console;

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
        //Console.RegisterType<BirdCommands>();
        var tools = true;
        SceneManager.LoadScene(new PersistantScene());
        //if (tools) LoadTools();
        SceneManager.LoadScene(new MenuScene());
        //Util.LoadSceneFromFilesystem("Scenes/MenuScene.lscene");
    }

    /*public void LoadTools() {
        var tools = new GameObject("EngineTools");
        var t = tools.AddComponent<ImguiToolsController>();
        t.InputContext = InputContext; //hack
        tools.Initialize();
    }*/
}