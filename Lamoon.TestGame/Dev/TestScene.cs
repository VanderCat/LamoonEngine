using System.Numerics;
using Lamoon.Engine.Components;
using Lamoon.Graphics;
using NekoLib.Core;
using NekoLib.Scenes;
using Lamoon.Engine;
using Lamoon.Filesystem;
using Texture = Lamoon.Graphics.Texture;

namespace Lamoon.TestGame.Dev; 

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
        var mesh  = Mesh.Quad;
        var material = Material.FromFilesystem("Materials/dev_01.lmat");
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
        camera.AddComponent<ControlCamera>();
        testMesh.AddComponent<Movement>();
        cameraComponent.OrthoScale = 0.001f;
        cameraComponent.Orthographic = false;
        
        var testMesh2 = new GameObject();
        testMesh2.Name = "TestMesh";
        var material2 = new Material(Texture.FromFilesystem("Materials/test.png"), Shader.Default);
        var meshRenderer2 = testMesh2.AddComponent<MeshRenderer>();
        meshRenderer2.Mesh = mesh;
        meshRenderer2.Material = material2;
        //camera.AddComponent<Rotation>();

        //var testModel = new GameObject();
        //testModel.AddComponent<MeshRenderer>().Mesh = Mesh.FromObjStream(Files.GetFile("Models/amy.obj").GetStream());
        //testModel.Transform.LocalScale = new Vector3(10f, 10f, 10f);
        //testModel.AddComponent<Movementegl>();
        //testModel.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, float.DegreesToRadians(90));
        var testModel = Engine.Model.FromFileSystem("Models/test_map.obj");
        testModel.GetComponentInChildren<MeshRenderer>().Material = new Material(Texture.FromFilesystem("Materials/test.png"), Shader.Default);
        var brokenModel = Engine.Model.FromFileSystem("Models/i_dont_exsist.fbx");

        var skiaDraw = new GameObject();
        //skiaDraw.AddComponent<SkiaCanvas>();
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