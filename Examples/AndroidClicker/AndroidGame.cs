using System.Reflection;
using Lamoon.Engine;
using Lamoon.Filesystem;
using NekoLib.Core;
using NekoLib.Scenes;
using Serilog;
using Serilog.Events;
using Silk.NET.Input.Sdl;

namespace AndroidClicker; 

public class AndroidGame : Game {
    public new static AndroidGame Instance {
        get {
            _instance ??= new AndroidGame();
            return (AndroidGame)_instance;
        }
    }

    public override void Load() {
        const string outputTemplate = "{Name}: {Message}{Exception}";
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.AndroidLog(LogEventLevel.Verbose, outputTemplate)
            .CreateLogger()
            .ForContext("Name", "LamoonEngine");
        base.Load();
        
        new AssemblyFilesystem(Assembly.GetAssembly(typeof(AndroidGame))).Mount();

        SceneManager.LoadScene(new TestScene());
    }

}