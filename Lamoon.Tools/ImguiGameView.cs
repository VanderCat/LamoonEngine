using System.Drawing;
using System.Numerics;
using Lamoon.Engine;
using Lamoon.Engine.Components;
using Lamoon.Graphics;
using NekoLib.Core;
using Silk.NET.OpenGL;
using Texture = Lamoon.Graphics.Texture;

namespace Lamoon.Tools; 

public class ImguiGameView : Behaviour {
    void DrawGui() {
        if (ImGui.Begin("Game View")) {
            ImGui.BeginChild("GameRenderer");
            var wsize = ImGui.GetWindowSize();
            var aspectRatio = GraphicsReferences.ScreenSize.GetAspectRatio();

            if (Camera.MainCamera is not null) {
                GraphicsReferences.OpenGl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                ImGui.Image((nint) Camera.MainCamera.RenderTexture.OpenGlHandle, wsize with {Y = wsize.X/aspectRatio}, new(0, 1), new(1, 0));
            }
            else
                ImGui.Image((nint)Texture.Missing.OpenGlHandle, wsize, new(0, 1), new(1, 0));
            ImGui.EndChild();
        }
        ImGui.End();
    }
}