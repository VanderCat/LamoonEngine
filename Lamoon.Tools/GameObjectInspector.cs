using System.Reflection;
using ImGuiNET;
using Lamoon.Engine;
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
        if (ImGui.CollapsingHeader(MaterialIcons.Control_camera + "Transform")) {
            TransformInspector?.DrawGui();
        }
        foreach (var component in target.GetComponents()) {
            var iconAttribute = (ToolsIconAttribute?)component.GetType().GetCustomAttribute(typeof(ToolsIconAttribute));
            var icon = iconAttribute?.Icon;
            if (ImGui.CollapsingHeader((icon??MaterialIcons.Insert_drive_file)+ component.GetType().Name+"##"+component.Id)) {
                ImGui.TextDisabled($"ID:{component.Id}");
                var a = GetInspectorFor(component); // FIXME: it will be SLOWWWW
                a.DrawGui();
            }
        }
    }
}