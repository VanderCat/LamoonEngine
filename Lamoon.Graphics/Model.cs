using System.Numerics;
using Lamoon.Data;
using Lamoon.Filesystem;
using MessagePack;
using NekoLib.Filesystem;
using Serilog;
using Silk.NET.OpenGL;

namespace Lamoon.Graphics; 

public class Model {
    public readonly static Model Error = FromFilesystem("Models/missing_model");

    public List<TransformedMesh> Root = new();
    public List<TransformedMesh> AllMeshes = new();
    public Dictionary<int, Material> Materials = new();

    public class TransformedMesh : Mesh {
        private TransformedMesh? _parent;
        private List<TransformedMesh> _children = new();
        public int MaterialIndex = 0;

        private int ChildrenCount => _children.Count;
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

        public Matrix4x4 LocalMatrix = Matrix4x4.Identity;
        public Matrix4x4 GlobalMatrix => Parent?.GlobalMatrix ?? Matrix4x4.Identity * LocalMatrix;

        public TransformedMesh(float[] vertices, uint[] indices, PrimitiveType type = PrimitiveType.Triangles) : base(vertices, indices, type) { }
        
        public TransformedMesh GetChild(int index) {
            return _children[index];
        }

        public TransformedMesh[] GetAll() {
            var children = _children.AsEnumerable();
            foreach (var child in _children) {
                children = children.Concat(child.GetAll());
            }

            return children.ToArray();
        }
    }

    public const int Version = 2;

    public static Model FromSerialized(SerializableModel model) {
        var realModel = new Model();
        if (model.Version != Version) {
            Log.Warning("Model and runtime version mismatch! Things may work incorrectly");
        }

        //FIXME: support for nested models or smth idk, this is a hack ultimately
        foreach (var mesh in model.Children[0].Meshes) {
            var tm = new TransformedMesh(mesh.Verticies, mesh.Indicies);
            //tm.LocalMatrix = mesh.Transform;
            realModel.AllMeshes.Add(tm);
        }

        foreach (var pair in model.Children[0].Relationships) {
            if (pair.Value == -1) {
                realModel.Root.Add(realModel.AllMeshes[pair.Key]);
                continue;
            }

            realModel.AllMeshes[pair.Value].Parent = realModel.AllMeshes[pair.Key];
        }

        foreach (var pair in model.Children[0].Material) {
            realModel.Materials[pair.Key] = Material.FromFilesystem(pair.Value);
        }

        return realModel;
    }

    public static Model? FromFilesystem(string path) {
        if (path.EndsWith(".lmdl")) {
            Log.Error("Attempt to load uncompiled model {Name}!", path);
            throw new NotSupportedException();
        }
        if (!path.EndsWith(".lmdlc")) path += ".lmdlc";
        if (!Files.FileExists(path)) {
            Log.Error("Model {Name} was not found!", path);
            return null;
        }
        var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
        using var stream = Files.GetFile(path).GetStream();
        var serializedModel = MessagePackSerializer.Deserialize<SerializableModel>(stream, lz4Options);

        return FromSerialized(serializedModel);
    }
}