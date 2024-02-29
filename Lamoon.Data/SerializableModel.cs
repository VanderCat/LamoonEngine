using MessagePack;

namespace Lamoon.Data; 

[MessagePackObject]
public class SerializableModel {
    [Key(0)] 
    public int Version = 1;
    
    [MessagePackObject]
    public class Metadata {
        [Key(0)]
        public string? Author;
        [Key(1)]
        public string? Description;
        [Key(2)]
        public long? CompilationTimestamp;
    }

    [Key(1)]
    public Metadata? MetaData;
    [Key(2)]
    public SerializableMesh[] Meshes = Array.Empty<SerializableMesh>();
    [Key(3)]
    public Dictionary<int, string> Material = new();

    // -1 is a root
    [Key(4)]
    public Dictionary<int, int> Relationships = new();

    [Key(5)] 
    public List<SerializableModel> Children = new();
}