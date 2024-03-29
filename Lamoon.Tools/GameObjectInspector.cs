using System.Reflection;
using ImGuiNET;
using NekoLib.Core;

namespace Lamoon.Tools; 

[CustomInspector(typeof(GameObject))]
public class GameObjectInspector : Inspector {

    private Inspector? TransformInspector;

    public override void Initialize() {
        TransformInspector = GetInspectorFor(((GameObject) Target).Transform);
    }

    public override void DrawGui() {
        var target = ((GameObject) Target);
        ImGui.TextDisabled($"ID:{target.Id}");
        ImGui.InputText("Name", ref target.Name, 256);
        ImGui.Checkbox("Enabled", ref target.ActiveSelf);
        if (ImGui.CollapsingHeader("Transform")) {
            TransformInspector?.DrawGui();
        }
        foreach (var component in target.GetComponents()) {
            if (ImGui.CollapsingHeader(component.GetType().Name)) {
                var a = GetInspectorFor(component); // FIXME: it will be SLOWWWW
                a.DrawGui();
            }
        }
    }
}