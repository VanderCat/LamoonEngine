using Lamoon.Engine;
using NekoLib.Core;

namespace Lamoon.Tools.ModelViewer; 

internal class ImGuiModelViewerGuiController : ImGuiMvComponent {
    public string Model = "";
    public SelectedModelController ModelController;
    void DrawGui() {
        if (Game.IsToolsOpened) return;
        if (ImGui.Begin("ModelViewer")) {
            ImGui.InputText("", ref Model, 256);
            ImGui.SameLine();
            if (ImGui.Button("Load")) {
                ModelController.ChangeModel(Model);
            }
            ImGui.End();
        }
    }
}