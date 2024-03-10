using System.Numerics;
using ImGuiNET;
using NekoLib.Core;
using Serilog;

namespace Lamoon.Tools; 

public class ImguiInspect : Behaviour {
    private object? _selectedObject;
    private Inspector? _inspector;
    public object? SelectedObject {
        get => _selectedObject;
        set {
            _selectedObject = value;
            try {
                _inspector = Inspector.GetInspectorFor(value);
            }
            catch (Exception e) {
                _lastException = e;
                Log.Error(e, "Could not create inspector");
            }
        }
    }

    private Exception _lastException;
    
    void DrawGui() {
        if (ImGui.Begin("Inspector")) {
            if (_selectedObject is null ) return;
            if (_inspector is null) DrawFail(_lastException);
            try {
                _inspector.DrawGui();
            }
            catch (Exception e) {
                DrawFail(e);
            }
        }
    }

    void DrawFail(Exception e) {
        ImGui.TextColored(Vector4.UnitX, "Failed to render inspector!");
        if (ImGui.CollapsingHeader("Error")) {
            ImGui.TextWrapped($"{e}");
        }
    }
}