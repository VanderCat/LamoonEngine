using NekoLib.Core;
using ImGuiNET;
using NekoLib.Scenes;

namespace Lamoon.Tools; 

public class ImguiSceneViewer : Behaviour {

    private ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.OpenOnDoubleClick | ImGuiTreeNodeFlags.SpanAvailWidth;
    public ImguiInspect Inspect;

    void Awake() {
        Inspect = GameObject.GetComponent<ImguiInspect>();
    }
    
    void DrawGui() {
        if (ImGui.Begin("Hierarchy")) {
            foreach (var scene in SceneManager.Scenes) {
                if (scene == SceneManager.ActiveScene) ImGui.SetNextItemOpen(true, ImGuiCond.Once);
                DrawSceneGui(scene);
            }
        }
    }

    void DrawSceneGui(IScene scene) {
        if (ImGui.TreeNode(scene.Name)) {
            DrawGameObjectHierarchyRootGui(scene);
            ImGui.TreePop();
        }
    }

    void DrawGameObjectHierarchyRootGui(IScene scene) {
        foreach (var gameObject in scene.GameObjects) {
            if (gameObject.Transform.Parent is not null) return;
            var node = ImGui.TreeNodeEx(gameObject.Name, flags);
            if (ImGui.IsItemClicked() && !ImGui.IsItemToggledOpen())
                Inspect.SelectedObject = gameObject;
            if (node) {
                DrawGameObjectHierarchyGui(gameObject);
                ImGui.TreePop();
            }
        }
    }
    
    void DrawGameObjectHierarchyGui(GameObject gameObject) {
        foreach (var go in gameObject.Transform) {
            var node = ImGui.TreeNodeEx(gameObject.Name, flags);
            if (ImGui.IsItemClicked() && !ImGui.IsItemToggledOpen())
                Inspect.SelectedObject = gameObject;
            if (node) {
                DrawGameObjectHierarchyGui(go.GameObject);
                ImGui.TreePop();
            }
        }
    }
}