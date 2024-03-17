using Lamoon.Engine;
using NekoLib.Core;
using Silk.NET.Input;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace Lamoon.Tools; 

public class ImguiToolsController : Behaviour {
    private ImguiInspect _inspect;
    private ImguiSceneViewer _sceneViewer;
    private ImguiConsole _imguiConsole;
    private ImguiToolsView _imguiView;
    private ImguiGameView _imguiGameView;
    private GameObject _other;
    private GameObject _viewGo;

    public IInputContext InputContext; //TODO: use global context;
    
    void Awake() {
        Game.IsToolsOpened = true;
        _other = new GameObject("AllTools");
        _viewGo = new GameObject("DevCamera");
        _other.Transform.Parent = Transform;
        _viewGo.Transform.Parent = _other.Transform;
        //GameObject.AddComponent<DrawSceneOffScreen>();
        _inspect = _other.AddComponent<ImguiInspect>();
        _sceneViewer = _other.AddComponent<ImguiSceneViewer>();
        _imguiConsole = _other.AddComponent<ImguiConsole>();
        _imguiGameView = _other.AddComponent<ImguiGameView>();
        _imguiView = _viewGo.AddComponent<ImguiToolsView>();
        _imguiView.kb = InputContext.Keyboards[0];
        
        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        InputContext.Keyboards[0].KeyDown += (keyboard, key, arg3) => {
            if (key == Key.F5) ToggleTools();
        };
        _inspect.Invoke("Awake");
        _sceneViewer.Invoke("Awake");
        _imguiConsole.Invoke("Awake");
        _imguiView.Invoke("Awake");
        _imguiGameView.Invoke("Awake");
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
                ImGui.MenuItem("Tools View", "", ref _viewGo.ActiveSelf);
                ImGui.MenuItem("Game View", "", ref _imguiGameView.Enabled);
                ImGui.EndMenu();
            }

            ImGui.EndMainMenuBar();
        }
        ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());
    }
    void ToggleTools() {
        Enabled = !Enabled;
        _other.Active = Enabled;
        Game.IsToolsOpened = Enabled;
    }
}