using System.Diagnostics;
using System.Numerics;
using JoltPhysicsSharp;
using Lamoon.Engine.Components;
using Lamoon.Graphics;
using NekoLib.Core;
using NekoLib.Scenes;
using Lamoon.Engine;
using Lamoon.Filesystem;
using Lamoon.Physics;
using Lamoon.Physics.Shapes;
using Texture = Lamoon.Graphics.Texture;

namespace Lamoon.TestGame.Dev; 

public class PhysicsScene : IScene {
    public string Name => "TestScene";
    public bool DestroyOnLoad => false;
    public int Index { get; set; }

    private List<GameObject> _gameObjects = new();
    public List<GameObject> GameObjects => _gameObjects;

    private PhysicsWorld _world = PhysicsWorld.Instance;
 
    public void Initialize() {
        var camera = new GameObject();
        camera.Name = "Camera";
        camera.AddComponent<Camera>();
        camera.Transform.LocalPosition = -Vector3.UnitY*16 + -Vector3.UnitX*5;
        camera.Transform.LocalRotation = Quaternion.CreateFromAxisAngle(
            Vector3.UnitX,
            float.DegreesToRadians(90f)
        );
        camera.AddComponent<Rotation>();
        var testFloor = new GameObject();
        testFloor.AddComponent<MeshRenderer>().Mesh = Mesh.Quad;
        testFloor.Transform.LocalScale = Vector3.One * 10;

        var model = Model.SpawnErrorModel();
        model.Transform.LocalPosition = Vector3.UnitY * 5;
        var rb = model.AddComponent<Rigidbody>();
        var shape = model.AddComponent<BoxCollision>();

        var skiaDraw = new GameObject();
        skiaDraw.AddComponent<SkiaCanvas>();
        var drawTransform = new GameObject();
        drawTransform.Transform.Parent = skiaDraw.Transform;
        drawTransform.AddComponent<TransformInfo>().ToWatch = camera.Transform;

        foreach (var gameObject in GameObjects) {
            gameObject.Initialize();
        }
    }

    public void Update() {
        foreach (var gameObject in GameObjects) {
            gameObject.Update();
        }
    }

    public void FixedUpdate() {
        _world.Update();
    }

    public void Draw() {
        foreach (var gameObject in GameObjects) {
            gameObject.Draw();
        }
    }
}