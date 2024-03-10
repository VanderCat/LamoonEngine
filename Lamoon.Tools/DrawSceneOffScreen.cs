using System.Numerics;
using NekoLib.Core;
using ImGuiNET;
using Lamoon.Graphics;
//using Lamoon.TestGame.Dev;
using Silk.NET.OpenGL;

namespace Lamoon.Tools; 

public class DrawSceneOffScreen : Behaviour {
    void DrawGui() {
        if (ImGui.Begin("GameWindow")) {
            ImGui.BeginChild("GameRenderer");
            var wsize = ImGui.GetWindowSize();
            GraphicsReferences.OpenGl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            //ImGui.Image((nint)((TestScene)GameObject.Scene).RenderTexture.OpenGlHandle, wsize, new(0, 1), new(1, 0));
            ImGui.EndChild();
        }
        ImGui.End();
    }
}