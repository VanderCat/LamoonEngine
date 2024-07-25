using System.Reflection;
using Lamoon.Data;
using Lamoon.Data.YamlSupport;
using Lamoon.Engine.YamlExtras;
using Lamoon.Filesystem;
using Serilog;
using Serilog.Events;
//using Silk.NET.Windowing.Glfw;

namespace Lamoon.Engine; 

public static class Bootstrapper {
    private static Game GetGame(string binPath) {
        Game game;
        if (!File.Exists(binPath+"/game.dll")) return new NoGame();
        var gameDll = Assembly.LoadFrom(binPath + "/game.dll");
        var gameType = gameDll.GetTypes().FirstOrDefault(type => type.IsAssignableTo(typeof(Game)));
        if (gameType is null)
            game = new NoGame();
        else
            game = (Game)gameType.GetProperty("Instance").GetValue(null);
        return game;
    }

    public static void Init() {
        //GlfwWindowing.Use();
        
        const string outputTemplate = "{Timestamp:HH:mm:ss} [{Level}] {Name}: {Message}{Exception}{NewLine}";
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Verbose()
#else
            .MinimumLevel.Information()
#endif
            .Enrich.FromLogContext()
            .WriteTo.Console(LogEventLevel.Verbose, outputTemplate)
            .WriteTo.File($"logs/lamoon{DateTime.Now:yy.MM.dd-hh.MM.ss}.log", LogEventLevel.Verbose, outputTemplate)
            .CreateLogger()
            .ForContext("Name", "LamoonEngine");
        
        Definition.TypeConverters.Add(new ObjectRefConverter());
        Definition.TypeConverters.Add(new TextureConverter());
        Definition.TypeConverters.Add(new MaterialConverter());
        Definition.TypeConverters.Add(new SpriteConverter());
        Definition.TypeConverters.Add(new OggSoundFileConverter());
        Definition.TypeConverters.Add(new FilesystemPathsResolver());
    }
    
    public static void Start(string[] args) {
        Init();
        
        var gameId = "default";
        for (int i = 0; i < args.Length; i++) {
            if (args[i] != "-game") continue;
            gameId = args[i + 1];
            break;
        }

        using var stream = new FileStream(gameId + "/meta.lgame", FileMode.Open, FileAccess.Read);
        var conf = Definition.FromStream<GameManifest>(stream);
        var game = GetGame(conf.Filesystem.Bin.Replace("{{mod}}", gameId));
        for (var i = conf.Filesystem.Mount.Count - 1; i >= 0; i--) {
            var mountPoint = conf.Filesystem.Mount[i].Replace("{{mod}}", gameId);
            if (mountPoint.EndsWith("*")) {
                try {
                    var allMountPoints = Directory.EnumerateDirectories(mountPoint).ToArray();
                    for (var j = allMountPoints.Length - 1; j >= 0; j--) {
                        var recursiveMountPoint = allMountPoints[i];
                        new FolderFilesystem(recursiveMountPoint)
                            .Mount(); //TODO: allow any archive type through meta.lgame
                        Log.Information("Mounted {Directory}", recursiveMountPoint);
                    }
                }
                catch (DirectoryNotFoundException) {
                    Log.Warning("{Directory} was not found, skipping mounting", mountPoint);
                }

                continue;
            }
            new FolderFilesystem(mountPoint).Mount();
            Log.Information("Mounted {Directory}", mountPoint);
        }
        
        game.WindowOptions = game.WindowOptions;
        game.InitializeWindow();
        game.BindCallbacks(game.Window);
        game.RunWindow();
    }
}