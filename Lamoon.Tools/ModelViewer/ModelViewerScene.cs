using Lamoon.Engine.Components;
using NekoLib.Core;
using NekoLib.Scenes;

namespace Lamoon.Tools.ModelViewer; 

public class ModelViewerScene : IScene {
    public string Name => "ModelViewerScene";
    public bool DestroyOnLoad => true;
    public int Index { get; set; }
    public List<GameObject> GameObjects { get; } = new();

    public void Initialize() {
        var camera = new GameObject("Camera");
        var actualCamera = camera.AddComponent<Camera>();
        var cameraConroller = camera.AddComponent<ModelViewerCameraConroller>();
        actualCamera.IsMain = true;
        var modelChange = new GameObject("ModelController");
        var controller = modelChange.AddComponent<SelectedModelController>();
        controller.ChangeModel("Models/missing_model");
        var settingsController = new GameObject("SettingsController");
        var aa = settingsController.AddComponent<ImGuiModelViewerGuiController>();
        aa.ModelController = controller;

        cameraConroller.FocusPoint = controller.Transform;
            
        foreach (var gameObject in GameObjects) {
            gameObject.Initialize();
        }
    }

    public void Update() {
        var currentGameObjects = new GameObject[GameObjects.Count];
        GameObjects.CopyTo(currentGameObjects);
        foreach (var gameObject in currentGameObjects) {
            gameObject.Update();
        }
    }

    public void Draw() {
        var currentGameObjects = new GameObject[GameObjects.Count];
        GameObjects.CopyTo(currentGameObjects);
        foreach (var gameObject in currentGameObjects) {
            gameObject.Draw();
        }
    }
}