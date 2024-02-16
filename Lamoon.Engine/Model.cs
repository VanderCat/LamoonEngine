using System.Numerics;
using System.Runtime.InteropServices;
using Lamoon.Engine.Components;
using Lamoon.Filesystem;
using Lamoon.Graphics;
using NekoLib.Core;
using Serilog;
using SharpGLTF.Schema2;
using Silk.NET.Assimp;
using Mesh = Silk.NET.Assimp.Mesh;
using Node = SharpGLTF.Schema2.Node;
using Scene = Silk.NET.Assimp.Scene;

namespace Lamoon.Engine; 

public static class Model {
    private static ILogger Log = Serilog.Log.Logger.ForContext("Name", "ModelLoader");
    
    public unsafe static GameObject FromFileSystem(string path) {
        Log.Information("Loading model from {0}", path);
        var assimp = Assimp.GetApi();
        Log.Verbose("Got AssImp api {0}.{1}", assimp.GetVersionMajor(), assimp.GetVersionMinor());
        var bytes = Files.GetFile(path).ReadBinary();
        Log.Verbose("Successfully read the file");
        Log.Verbose("Trying to read byte array at {pointer}, with length {lenght}", new IntPtr(&bytes), bytes.Length);
        Scene* scene;
        fixed(byte* buf = bytes)
            scene = assimp.ImportFileFromMemory(buf, (uint)bytes.Length*sizeof(byte), 0u, Path.GetExtension(path));
        var error = assimp.GetErrorStringS() ?? "";
        if (error != "") {
            Log.Error("Assimp reported an error: "+error);
            throw new Exception("Could not load the model!");
        }

        Log.Debug("AssImp Successfully imported the model");

        var rNode = scene->MRootNode;
        var numChildren = rNode->MNumChildren;

        Log.Debug("Generating Meshes");
        var meshes = GenerataeLamoonMeshes(scene);
        
        Log.Debug("Generating GameObjects");
        var gameObject = AssImpRecursive(rNode, meshes);

        return gameObject;
    }

    private unsafe static Graphics.Mesh[] GenerataeLamoonMeshes(Scene* scene) {
        var generatedMeshes = new Graphics.Mesh[scene->MNumMeshes];
        for (int i = 0; i < scene->MNumMeshes; i++) {
            var mesh = scene->MMeshes[i];
            var verticies = new Vertex[mesh->MNumVertices];
            var indexes = new List<uint>();
            for (int j = 0; j < mesh->MNumVertices; j++) {
                var vert = new Vertex();
                vert.Position = mesh->MVertices[j];
                vert.Normal = mesh->MNormals[j];
                if (mesh->MTextureCoords[0] != null) // does the mesh contain texture coordinates?
                {
                    // a vertex can contain up to 8 different texture coordinates. We thus make the assumption that we won't 
                    // use models where a vertex can have multiple texture coordinates so we always take the first set (0).
                    Vector3 texcoord3 = mesh->MTextureCoords[0][i];
                    vert.TexCoords = new Vector2(texcoord3.X, texcoord3.Y);
                }

                verticies[j] = vert;
            }

            for (int j = 0; j < mesh->MNumFaces; j++) {
                var face = mesh->MFaces[j];
                for (int k = 0; k < face.MNumIndices; k++) {
                    indexes.Add(face.MIndices[k]);
                }
            }
            
            generatedMeshes[i] = new Graphics.Mesh(verticies.BuildVerticies(), indexes.ToArray());
        }

        return generatedMeshes;
    }
    
    private unsafe static GameObject AssImpRecursive(Silk.NET.Assimp.Node* node, Graphics.Mesh[] meshes, GameObject? parent = null) {
        Log.Verbose("Processing node {0}", Marshal.PtrToStringAuto(new IntPtr(node->MName.Data), (int)node->MName.Length));
        var numChildren = node->MNumChildren;
        
        parent ??= new GameObject();
        parent.Name = Marshal.PtrToStringAuto(new IntPtr(node->MName.Data), (int)node->MName.Length) ?? string.Empty;
        for (int i = 0; i < numChildren; i++) {
            var child = node->MChildren[i];
            AssImpRecursive(child, meshes, new GameObject()).Transform.Parent = parent.Transform;
        }
        CopyMeshes(parent, meshes, node);
        return parent;
    }

    private unsafe static void CopyMeshes(GameObject go, Graphics.Mesh[] meshes, Silk.NET.Assimp.Node* node) {
        var numMeshes = node->MNumMeshes;
        for (int i = 0; i < numMeshes; i++) {
            var meshId = node->MNumMeshes;
            var meshComponent = go.AddComponent<MeshRenderer>();
            meshComponent.Mesh = meshes[meshId-1];
        }
    }

    [Obsolete("unfinished")]
    public unsafe static GameObject FromGltfFileStstem(string path) {
        ModelRoot model;
        using (var bytes = Files.GetFile(path).GetStream()) {
            //model = ModelRoot.ReadGLB(bytes. );
        }

        var go = new GameObject();
        //foreach (var node in model.DefaultScene.VisualChildren) {
        //    CreateHirearchy(node, go);
        //}

        return go;
    }

    private static GameObject CreateHirearchy(Node node, GameObject? parent) {
        parent ??= new GameObject();
        foreach (var child in node.VisualChildren) {
            var go = CreateHirearchy(child, CreateHirearchy(node, parent));
            go.Transform.Parent = parent.Transform;
            go.Transform.LocalPosition = child.LocalTransform.Translation;
            go.Transform.LocalRotation = child.LocalTransform.Rotation;
            go.Transform.LocalScale = child.LocalTransform.Scale;
            go.Name = child.Name;
            foreach (var mesh in child.Mesh.Primitives) {
                var meshComponent = go.AddComponent<MeshRenderer>();
                //mesh.
            }
        }

        return parent;
    }
}