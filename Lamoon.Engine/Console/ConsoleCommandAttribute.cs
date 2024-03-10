namespace Lamoon.Engine.Console;

[AttributeUsage(AttributeTargets.Method)]
public class ConsoleCommandAttribute : Attribute {
    public string ConCommandName;

    public ConsoleCommandAttribute(string conCommandName) {
        ConCommandName = conCommandName;
    }
}