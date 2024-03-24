using System.Text;

namespace Lamoon.Scripting.Lua;

public class LuaScriptingProvider : IScriptingProvider {
    public string Extension => "lua";

    public object? Run(Stream buffer) {
        using var streamReader = new StreamReader(buffer);
        var scriptResult = _lua.DoString(streamReader.ReadToEnd());
        if (scriptResult is not null)
            if (scriptResult.Length > 0)
                return scriptResult[0];
        return null;
    }

    public object? InvokeMethod(string? globalName, string name, object? o) {
        if (globalName is not null) {
            using var go = _lua.GetTable(globalName);
            if (go is not null) {
                if (go[name] is null) return null;
                using var method = _lua.GetFunction(globalName + "." + name);
                var methodResult = method?.Call(new[] {go, o});
                if (methodResult is null) return null;
                if (methodResult.Length < 1) return null;
                return methodResult[0];
            }
        }
        using var methoda = _lua.GetFunction(name);
        return methoda.Call(new[] {o})[0];
    }

    private readonly NLua.Lua _lua = new();

    public LuaScriptingProvider() {
        _lua.State.Encoding = Encoding.UTF8;
        _lua.LoadCLRPackage();
    }
}