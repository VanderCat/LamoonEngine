namespace Lamoon.Data; 

public class ModelDefinition {
    public class ModelInfo {
        public string? Name;
        public string? Author;
        public string? Date;
        public string? Description;
    }
    public ModelInfo? Info;
    
    public List<string>? MeshList;
    public List<string>? PhysicsMeshList;
    public List<string>? Materials;
    public Dictionary<string, int>? MaterialRedefinition;
}