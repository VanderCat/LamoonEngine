using System.Numerics;

namespace Lamoon.Nodes; 

public class Node {
    public Vector2 Position;
    public string Title;
    public NodeCollection? NodeCollection { get; internal set; }
}