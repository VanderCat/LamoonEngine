using Silk.NET.OpenGL.Extensions.ImGui;
using NekoLib.Core;
namespace Lamoon.TestGame.Dev; 

public class ImguiDemo : Behaviour {
    void DrawGui() {
        ImGuiNET.ImGui.ShowDemoWindow();
    }
}