using Lamoon.Data;
using Lamoon.Filesystem;
using Serilog;

namespace Lamoon.Scripting;

public static class ScriptingManager {
    public static IScriptingProvider ScriptingProvider;

    public static object? ExecuteFromFileSystem(string path) {
        if (!path.EndsWith("."+ScriptingProvider.Extension)) path += "."+ScriptingProvider.Extension;
        if (Files.FileExists(path)) {
            using var stream = Files.GetFile(path).GetStream();
            return ScriptingProvider.Run(stream);
        }
        else {
            Log.Error("{Script} does not exist!", path);
        }

        return null;
    }
}