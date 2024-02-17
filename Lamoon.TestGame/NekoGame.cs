using Lamoon.Engine;
using Lamoon.Filesystem;
using Lamoon.Graphics;
using Lamoon.TestGame.Dev;
using NekoLib.Scenes;
using Serilog;
using Silk.NET.OpenGL;
using Shader = Lamoon.Graphics.Shader;
using Texture = Lamoon.Graphics.Texture;

namespace Lamoon.TestGame; 

public class NekoGame : Game {

    public new static Game Instance {
        get {
            _instance ??= new NekoGame();
            return _instance;
        }
    }
    
    public override void Load() {
        base.Load();
        var WorkFolder = new FolderFilesystem("Data");
        WorkFolder.Mount();
        //var mod = new FolderFilesystem("Mods/TestMod");
        //mod.Mount();
        
        SceneManager.LoadScene(new TestScene());
    }
}