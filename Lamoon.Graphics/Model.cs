using System.Numerics;
using Silk.NET.OpenGL;

namespace Lamoon.Graphics; 

public class Model {
    public class ModelInfo {
        public string? Name;
        public string? Author;
        public string? Date;
        public string? Description;
    }
    
    public class Metadata {
        public List<string>? MeshList;
        public List<string>? PhysicsMeshList;
        public ModelInfo? Info;
        public List<string>? Materials;
        public Dictionary<string, int>? MaterialRedefinition;
    }

    public class TransformedMesh : Mesh {
        private TransformedMesh? _parent;
        private List<TransformedMesh> _children = new();
        public TransformedMesh? Parent {
            get => _parent;
            set {
                if (_parent == value) return;
                if (value is null) {
                    _parent?._children.Remove(this);
                    _parent = value;
                    return;
                }
                _parent = value;
                _parent._children.Add(this);
            }
        }

        public Matrix4x4 LocalMatrix;
        public Matrix4x4 GlobalMatrix => Parent?.GlobalMatrix ?? Matrix4x4.Identity * LocalMatrix;

        public TransformedMesh(float[] vertices, uint[] indices, PrimitiveType type = PrimitiveType.Triangles) : base(vertices, indices, type) { }
        
        public TransformedMesh GetChild(int index) {
            return _children[index];
        }
    }
}