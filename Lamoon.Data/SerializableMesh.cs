using System.Numerics;
using MessagePack;

namespace Lamoon.Data; 

[MessagePackObject]
public class SerializableMesh {
    [Key(0)]
    public string Name;
    [Key(1)]
    public float[] Verticies;
    [Key(2)]
    public uint[] Indicies;

    [Key(3)] public int MaterialIndex = 0;
    //[Key(4)] public Matrix4x4 Transform; // TODO: fix
}