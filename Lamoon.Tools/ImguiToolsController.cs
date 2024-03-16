using Lamoon.Engine;
using NekoLib.Core;
using Silk.NET.Input;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace Lamoon.Tools; 

public class ImguiToolsController : Behaviour {
    private ImguiInspect _inspect;
    private ImguiSceneViewer _sceneViewer;
    private ImguiConsole _imguiConsole;
    private GameObject other;

    public IInputContext InputContext; //TODO: use global context;
    
    void Awake() {
        other = new GameObject("AllTools");
        other.Transform.Parent = Transform;
        //GameObject.AddComponent<DrawSceneOffScreen>();
        _inspect = other.AddComponent<ImguiInspect>();
        _sceneViewer = other.AddComponent<ImguiSceneViewer>();
        _imguiConsole = other.AddComponent<ImguiConsole>();
        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        InputContext.Keyboards[0].KeyDown += (keyboard, key, arg3) => {
            if (key == Key.F5) ToggleTools();
        };
        _inspect.Invoke("Awake");
        _sceneViewer.Invoke("Awake");
        _imguiConsole.Invoke("Awake");
    }

    void DrawGui() {
        if (ImGui.BeginMainMenuBar())
        {
            const bool enabled = true;
            if (ImGui.BeginMenu("Tools")) {
                if (ImGui.MenuItem("Toggle", "F5")) {
                    ToggleTools();
                }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("View")) {
                ImGui.MenuItem("Inspector", "", ref _inspect.Enabled);
                ImGui.MenuItem("Scene Viewer", "", ref _sceneViewer.Enabled);
                ImGui.MenuItem("Console", "", ref _imguiConsole.Enabled);
                ImGui.EndMenu();
            }

            ImGui.EndMainMenuBar();
        }
        ImGui.DockSpaceOverViewport();
    }
    void ToggleTools() {
        Enabled = !Enabled;
        other.Active = Enabled;
    }
}