using NekoLib.Core;

namespace Lamoon.Tools; 

public class ImguiDemoView : Behaviour {
    void DrawGui() {
        ImGui.ShowDemoWindow();
    }
}