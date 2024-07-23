using System.Collections;

namespace Lamoon.Nodes; 

public class NodeCollection : IEnumerable<Node>, IList<Node> {
    private List<Node> _nodes = new();

    public IEnumerator<Node> GetEnumerator() =>  new NodeEnumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public void Add(Node item) {
        item.NodeCollection = this;
        _nodes.Add(item);
    }

    public void Clear() {
        foreach (var node in _nodes) {
            node.NodeCollection = null;
        }
        _nodes.Clear();
    }

    public bool Contains(Node item) => _nodes.Contains(item);

    public void CopyTo(Node[] array, int arrayIndex) {
        throw new NotSupportedException();
    }

    public bool Remove(Node item) {
        if (item.NodeCollection != this)
            return false;
        item.NodeCollection = null;
        return _nodes.Remove(item);
    }

    public int Count => _nodes.Count;
    public bool IsReadOnly => false;
    public int IndexOf(Node item) => _nodes.IndexOf(item);

    public void Insert(int index, Node item) {
        item.NodeCollection = this;
        _nodes.Insert(index, item);
    }

    public void RemoveAt(int index) {
        this[index].NodeCollection = null;
        _nodes.RemoveAt(index);
    }

    public Node this[int index] {
        get => _nodes[index];
        set => _nodes[index] = value;
    }
}

internal class NodeEnumerator : IEnumerator<Node> {
    public NodeCollection Collection;
    public NodeEnumerator(NodeCollection collection) {
        Collection = collection;
    }

    public bool MoveNext() => ++Cursor<Collection.Count;

    public void Reset() {
        Cursor = -1;
    }

    public int Cursor = -1;
    public Node Current => Collection[Cursor];

    object IEnumerator.Current => Current;

    public void Dispose() {
        Reset();
    }
}