namespace Lamoon.Data; 

public class ComponentDefinition {
    public Guid Id = Guid.NewGuid();
    public string Type = "NekoLib.Core.Component";
    public Dictionary<string, object>? Fields;
}