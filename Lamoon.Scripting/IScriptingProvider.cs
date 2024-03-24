namespace Lamoon.Scripting; 

public interface IScriptingProvider {
    public string Extension { get; }
    
    public object? Run(Stream buffer);

    public object? InvokeMethod(string? globalName, string name, object? o);
}