using System.Numerics;
using Lamoon.Engine.Components;
using Lamoon.Graphics;
using NekoLib.Core;
using NekoLib.Scenes;
using Serilog;
using Texture = Lamoon.Graphics.Texture;

namespace Lamoon.Engine.Dev; 

public class TestScene : IScene {
    public string Name => "TestScene";
    public bool DestroyOnLoad => false;
    public int Index { get; set; }

    public List<GameObject> _gameObjects = new();
    public List<GameObject> GameObjects => _gameObjects;
    public void Initialize() {
        /*var a = new GameObject();
        a.AddComponent<TestComponent>();
        a.AddComponent<Movement>();
        var help = new GameObject();
        var b = help.AddComponent<TestComponent>();
        b.onAwake += () => {
            b._tex = Texture.FromFileSystem("Textures/test1.png");
        };
        help.Transform.LocalPosition = new Vector3(0.5f, -1f, 1f);
        help.Transform.Parent = a.Transform;
        a.Transform.LocalRotation = Quaternion.CreateFromYawPitchRoll(0,Single.DegreesToRadians(20), Single.DegreesToRadians(45));
        */
        var camera = new GameObject();
        camera.Name = "Camera";
        var cameraComponent = camera.AddComponent<Camera>();
        

        var testMesh = new GameObject();
        testMesh.Name = "TestMesh";
        var mesh  = new Mesh(new[] {
                0.5f, 0.5f, 0.0f, 1.0f, 1.0f,
                0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
                -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
                -0.5f, 0.5f, 0.0f, 0.0f, 1.0f
            },
            new[] {
                0u, 1u, 3u,
                1u, 2u, 3u
            });
        var material = new Material(Texture.FromFileSystem("Textures/test1.png"), Shader.Default);
        var meshRenderer = testMesh.AddComponent<MeshRenderer>();
        meshRenderer.Mesh = mesh;
        meshRenderer.Material = material;
        
        /*testMesh.Transform.LocalRotation 
            = Quaternion.CreateFromAxisAngle(
                new Vector3(1, 0, 0), 
                float.DegreesToRadians(-55f)
            );*/
        camera.Transform.LocalPosition = new Vector3(0, 2.5f, 5f);
        camera.Transform.LocalRotation 
            = Quaternion.CreateFromAxisAngle(
                new Vector3(1, 0, 0), 
                float.DegreesToRadians(-20f)
            );
        testMesh.AddComponent<Movement>();
        cameraComponent.OrthoScale = 0.001f;
        cameraComponent.Orthographic = false;
        
        var testMesh2 = new GameObject();
        testMesh2.Name = "TestMesh";
        var material2 = new Material(Texture.FromFileSystem("Textures/test.png"), Shader.Default);
        var meshRenderer2 = testMesh2.AddComponent<MeshRenderer>();
        meshRenderer2.Mesh = mesh;
        meshRenderer2.Material = material2;
        camera.AddComponent<Rotation>();

        var testModel = Model.FromFileSystem("Models/rocks_03.obj");
        testModel.Transform.LocalScale = new Vector3(0.01f, 0.01f, 0.01f);

        var skiaDraw = new GameObject();
        skiaDraw.AddComponent<SkiaCanvas>();
        var drawHierarcy = new GameObject();
        drawHierarcy.Transform.Parent = skiaDraw.Transform;
        drawHierarcy.AddComponent<Hierarchy>();

        //var model = testModel.GetChildByNameRecursively("wood_barrel_big");
        //Log.Debug(model!.Transform.ToString());
        

        //var watcher = camera.AddComponent<Watcher>();
        //watcher.watch = testMesh.Transform;
        
        foreach (var gameObject in GameObjects) {
            gameObject.Initialize();
        }
    }

    public void Update() {
        foreach (var gameObject in GameObjects) {
            gameObject.Update();
        }
    }

    public void Draw() {
        foreach (var gameObject in GameObjects) {
            gameObject.Draw();
            
        }
        
    }
}