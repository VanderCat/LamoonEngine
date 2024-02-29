using Lamoon.Engine;
using Lamoon.Filesystem;
using Lamoon.Graphics;
using Lamoon.TestGame.Dev;
using NekoLib.Scenes;
using Serilog;
using Silk.NET.Input;
using Silk.NET.OpenGL;
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

    public IInputContext InputContext;

    public override void Load() {
        base.Load();
        var GameFolder = new FolderFilesystem("./");
        GameFolder.Mount();
        var WorkFolder = new FolderFilesystem("Data");
        WorkFolder.Mount();
        //var mod = new FolderFilesystem("Mods/TestMod");
        //mod.Mount();
        InputContext = View.CreateInput();
        SceneManager.LoadScene(new TestScene());
    }
}