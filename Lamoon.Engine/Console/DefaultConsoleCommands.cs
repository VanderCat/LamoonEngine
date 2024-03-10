namespace Lamoon.Engine.Console; 

public abstract class DefaultConsoleCommands {
    [ConsoleCommand("echo")]
    public static void Echo(string text) {
        Console.Log.Information("{Message}", string.Concat(text));
    }
    
    [ConsoleVariable("test_var")] public static bool TestVar { get; set; }

    [ConsoleCommand("help")]
    public static void Help() {
        foreach (var kp in Console.Commands) {
            Console.Log.Information(kp.Key);
        }
    }
}