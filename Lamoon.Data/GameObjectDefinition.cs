using System.Numerics;

namespace Lamoon.Data; 

public class GameObjectDefinition : Definition {
    public Guid Id = Guid.NewGuid();
    public string Type = "NekoLib.Core.GameObject";
    public string Name = "GameObject";
    public TransformDefinition Transform = new();
    public List<ComponentDefinition>? Components;
    public List<GameObjectDefinition>? Children;
}