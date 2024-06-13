namespace Lamoon.Data; 

public class SceneDefinition : Definition {
    public Guid Id = Guid.NewGuid();
    public string Name = "Unknown Scene";
    public string Type = "Lamoon.Engine.SerializableScene";
    public bool DestroyOnLoad = true;
    public List<GameObjectDefinition> GameObjects = new();
}