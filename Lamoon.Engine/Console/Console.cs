using System.Reflection;
using Serilog;

namespace Lamoon.Engine.Console; 


// currently this only works with static stuff and i could possibly make it work with instances too
// should i?
public static class Console {
    public static ILogger Log = Serilog.Log.Logger.ForContext("Name", "Console");
    public delegate void Command(object?[]? args);
    public static Dictionary<string, Command> Commands = new ();

    public static void Run(string command, object?[]? args = null) {
        try {
            Commands[command](args);
        }
        catch (Exception e) {
            Log.Error(e, "Failed to run command {Command}:", command);
        }
    }

    public static void RegisterType<T>() {
        Log.Verbose("Registering type {Type} properties", typeof(T));
        foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic)) {
            var attribute = property.GetCustomAttribute<ConsoleVariableAttribute>();
            if (attribute is null) continue;
            if (Commands.ContainsKey(attribute.ConVarName)) {
                Log.Warning("There is already a Console Variable/Command under this name! Overriding");
            }

            Commands[attribute.ConVarName] = args => {
                if (args is null) {
                    if (property.GetMethod is null) {
                        Log.Information("{ConName} = unk (no getter)", attribute.ConVarName);
                        return;
                    }
                    Log.Information("{ConName} = {Value}", attribute.ConVarName, property.GetValue(null));
                    return;
                }
                if (args.Length < 2) return;
                
                if (property.GetType() == args?[0]?.GetType()) {
                    property.SetValue(null, args[0]);
                    return;
                }

                if (attribute.ParseFunction is not null) {
                    property.SetValue(null, attribute.ParseFunction((string)args[0]));
                    return;
                }
                var parseMethod = property.GetType().GetMethod("Parse",
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy
                );
                if (parseMethod is null) {
                    Log.Error("Could not set property {ConName} bcs i cant parse it", attribute.ConVarName);
                    return;
                }

                var value = parseMethod.Invoke(null, new[] {args[0]});
                property.SetValue(null, value);
            };
        }
        Log.Verbose("Registering type {Type} properties", typeof(T));
        foreach (var method in typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.IgnoreReturn)) {
            var attribute = method.GetCustomAttribute<ConsoleCommandAttribute>();
            if (attribute is null) continue;
            
            Commands[attribute.ConCommandName] = args => {
                method.Invoke(null, args);
            };
        }
    }
    
}