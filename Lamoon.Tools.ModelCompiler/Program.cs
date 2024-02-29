using System.Numerics;
using System.Runtime.InteropServices;
using CommandLine;
using Lamoon.Data;
using Lamoon.Engine.Components;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Lamoon.Graphics;
using MessagePack;
using NekoLib.Core;
using Silk.NET.Assimp;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using File = System.IO.File;
using Mesh = Lamoon.Graphics.Mesh;

namespace Lamoon.Tools.ModelCompiler;

public static class Program {
    
    private static LoggingLevelSwitch _levelSwitch = new();

    public static void Main(string[] args)
    {
        const string outputTemplate = "{Timestamp:HH:mm:ss} [{Level}] {Message} {Exception}{NewLine}";
        _levelSwitch.MinimumLevel = LogEventLevel.Information;
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(_levelSwitch)
            .Enrich.FromLogContext()
            .WriteTo.Console(LogEventLevel.Verbose, outputTemplate)
            .CreateLogger();
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(RunOptions)
            .WithNotParsed(HandleParseError);
    }
    static void RunOptions(Options opts) {
        if (opts.Verbose)
            _levelSwitch.MinimumLevel = LogEventLevel.Verbose;
        Log.Information("Welcome to Lamoon Model compiler!");
        if (opts.InputFile is null) {
            Log.Warning("Input file was not provided, nothing to do, exiting...");
            return;
        }
        
        var md = OpenModel(opts.InputFile);
        
        Init();
        
        var basepath = Path.GetDirectoryName(Path.GetFullPath(opts.InputFile)) ?? "";
        var model = BuildModel(md, basepath);

        var lz4Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
        var serializedModel = MessagePackSerializer.Serialize(model, lz4Options);
        
        var output = opts.Output ?? Path.Join(basepath, Path.GetFileNameWithoutExtension(opts.InputFile) + ".lmdlc");
        Log.Information("Saving model to {output}", output);
        File.WriteAllBytes(output, serializedModel);
    }

    private static Assimp AssImpAPI;

    private static ModelDefinition OpenModel(string path) {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        ModelDefinition md;
        try {
            md = deserializer.Deserialize<ModelDefinition>(File.ReadAllText(path));
        }
        catch (FileNotFoundException e) {
            Log.Error("Could not find the model at {path}!", Path.GetFullPath(path));
            throw;
        }
        catch (Exception e) {
            Log.Error(e, "Failed to read model!\n");
            throw;
        }
        Log.Information("Successfully opened model {name} by {author}", Path.GetFileName(path), md.Info?.Author ?? "Unknown");
        return md;
    }
    
    private static void Init() {
        Log.Debug("Initializing AssImp");
        AssImpAPI = Assimp.GetApi();
        Log.Verbose("Loaded AssImp "+AssImpAPI.GetVersionMajor()+" "+AssImpAPI.GetVersionMinor());
    }

    private unsafe static SerializableModel GetPartialModel(string path, string basepath, ModelDefinition mdl) {
        var fullpath = Path.Join(basepath, path);
        Log.Debug("Processing mesh {mesh}", path);
        if (!File.Exists(fullpath)) {
            Log.Error("Failed to load mesh {0}! it does not exist", fullpath);
            throw new FileNotFoundException();
        }

        var bytes = File.ReadAllBytes(fullpath);
        Scene* scene;
        //assimp.
        fixed (byte* buf = bytes) {
            Log.Verbose("Trying to read byte array at {pointer}, with length {lenght}", new IntPtr(buf),
                bytes.Length);
            scene = AssImpAPI.ImportFileFromMemory(buf, (uint)bytes.Length * sizeof(byte), 0u, Path.GetExtension(path));
            AssImpAPI.ApplyPostProcessing(scene, (uint)PostProcessSteps.Triangulate);
        }

        var error = AssImpAPI.GetErrorStringS() ?? "";
        if (error != "") {
            Log.Error("Assimp reported an error: "+error);
            throw new Exception();
        }

        Log.Debug("AssImp Successfully imported the mesh");

        var rNode = scene->MRootNode;

        Log.Debug("Generating Meshes");
        var meshes = GenerataeLamoonMeshes(scene);
        
        Log.Debug("Generating Model");
        var model = new SerializableModel {
            Meshes = meshes
        };
        AssImpRecursive(rNode, model);

       Log.Debug("Parsing materials");
        for (int i = 0; i < scene->MNumMaterials; i++) {
            var mat = scene->MMaterials[i];
            var assimpString = new AssimpString();
            AssImpAPI.GetMaterialString(mat, "?mat.name", 0, 0, ref assimpString );
            var name = assimpString.AsString;
            model.Material[i] = name;
            Log.Verbose("found material {0}", name);
            if (model.Material[i] == "DefaultMaterial" && mdl.Materials is not null && mdl.Materials.Count > 0) model.Material[i] = mdl.Materials[0];
            Log.Verbose("using material {0}", model.Material[i]);
            if (mdl.MaterialRedefinition is null) continue;
            Log.Verbose("Overriding material");
            if (mdl.MaterialRedefinition.TryGetValue(name, out var value))
                model.Material[i] =(mdl.Materials is null)?name : mdl.Materials[value];
            Log.Verbose("Material {name} is set to {material}", name, model.Material[i]);
        }

        return model;
    }
    
    private unsafe static SerializableMesh[] GenerataeLamoonMeshes(Scene* scene) {
        var generatedMeshes = new SerializableMesh[scene->MNumMeshes];
        for (int i = 0; i < scene->MNumMeshes; i++) {
            var mesh = scene->MMeshes[i];
            var verticies = new Vertex[mesh->MNumVertices];
            var indexes = new List<uint>();
            if (mesh->MTextureCoords[1] != null) {
                Log.Warning("It seems mesh {mesh} have more than 1 UV. This is currently unsupported, and other UVs will be ignored.",
                    mesh->MName.AsString);
            }

            var materialIndex = mesh->MMaterialIndex;

            for (int j = 0; j < mesh->MNumVertices; j++) {
                var vert = new Vertex {
                    Position = mesh->MVertices[j],
                    Normal = mesh->MNormals[j]
                };
                if (mesh->MTextureCoords[0] != null) {
                    Vector3 texcoord3 = mesh->MTextureCoords[0][j];
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
            
            generatedMeshes[i] = new SerializableMesh {
                Name = mesh->MName.AsString,
                Verticies = verticies.BuildVerticies(),
                Indicies = indexes.ToArray()
            };
        }

        return generatedMeshes;
    }
    
    private unsafe static int AssImpRecursive(Node* node, SerializableModel model, int parent = -1) {
        Log.Verbose("Processing node {0}", node->MName.AsString);
        var numChildren = node->MNumChildren;
        
        for (int i = 0; i < numChildren; i++) {
            var child = node->MChildren[i];
            for (int j = 0; j < child->MNumMeshes; j++) {
                model.Relationships[(int)child->MMeshes[j]] = parent;
                AssImpRecursive(child, model, (int)child->MMeshes[j]);
            }
        }
        return parent;
    }

    public static SerializableModel BuildModel(ModelDefinition md, string basepath) {
        var model = new SerializableModel();
        if (md.MeshList is not null) {
            foreach (var meshPath in md.MeshList) {
                var partialModel = GetPartialModel(meshPath, basepath, md);
                model.Children.Add(partialModel);
            }
        }
        return model;
    }
    static void HandleParseError(IEnumerable<Error> errs)
    {
        foreach (var error in errs) {
            Log.Error("{error}",error);
        }
    }
}