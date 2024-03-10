namespace Lamoon.Engine.Console; 

[AttributeUsage(AttributeTargets.Property)]
public class ConsoleVariableAttribute : Attribute {
    public string ConVarName;

    public delegate object? ParseFunc(string value);

    public ParseFunc? ParseFunction;

    public ConsoleVariableAttribute(string conVarName) {
        ConVarName = conVarName;
    }
}