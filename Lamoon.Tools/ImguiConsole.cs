using System.Numerics;
using NekoLib.Core;
using Console = Lamoon.Engine.Console.Console;

namespace Lamoon.Tools; 

public class ImguiConsole : Behaviour {
    private string inputBuffer = "";
    void DrawGui() {
        if (!ImGui.Begin("Console")) {
            ImGui.End();
        }
        /*var footerHeightToReserve = ImGui.GetStyle().ItemSpacing.Y + ImGui.GetFrameHeightWithSpacing();
        if (ImGui.BeginChild("ScrollingRegion", new Vector2(0, -footerHeightToReserve), false, ImGuiWindowFlags.HorizontalScrollbar)
        {
            
        }
        Imgui.EndChild();*/
        var reclaimFocus = false;
        var inputTextFlags = 
            ImGuiInputTextFlags.EnterReturnsTrue | 
            ImGuiInputTextFlags.EscapeClearsAll | 
            ImGuiInputTextFlags.CallbackCompletion | 
            ImGuiInputTextFlags.CallbackHistory;
        if (ImGui.InputText("", ref inputBuffer, 256, inputTextFlags)) {
            Execute(inputBuffer.Trim());
            inputBuffer = "";
            reclaimFocus = true;
        }

        // Auto-focus on window apparition
        ImGui.SetItemDefaultFocus();
        if (reclaimFocus)
            ImGui.SetKeyboardFocusHere(-1); // Auto focus previous widget

        ImGui.End();
    }

    void Execute(string command) {
        var parts = command.Split().ToList();
        var conName = parts[0];
        parts.RemoveAt(0);
        Console.Run(conName, parts.ToArray());
    }
}