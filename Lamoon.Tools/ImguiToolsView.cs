using System.Drawing;
using Lamoon.Engine.Components;
using Lamoon.Graphics;
using NekoLib.Core;
using Silk.NET.OpenGL;
using Framebuffer = Lamoon.Graphics.Framebuffer;
using Renderbuffer = Lamoon.Graphics.Renderbuffer;
using Texture = Lamoon.Graphics.Texture;

namespace Lamoon.Tools; 

public class ImguiToolsView : Behaviour {
    public Camera DevCamera;

    void Awake() {
        DevCamera = GameObject.AddComponent<Camera>();
        DevCamera.Invoke("Awake");
    }

    void UpdateSize() {
        if (!_sizeChanged) return;
        DevCamera.ChangeSize(_renderSize);
    }

    void Update() {
        UpdateSize();
    }

    private Size _renderSize = GraphicsReferences.ScreenSize;
    private bool _sizeChanged;
    void DrawGui() {
        if (ImGui.Begin("Tools View")) {
            ImGui.BeginChild("ToolRenderer");
            var wsize = ImGui.GetWindowSize();
            var size = new Size((int) wsize.X, (int) wsize.Y);
            if (_renderSize != size) {
                _sizeChanged = true;
                _renderSize = size;
            }
            GraphicsReferences.OpenGl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            ImGui.Image((nint)DevCamera.RenderTexture.OpenGlHandle, wsize, new(0, 1), new(1, 0));
            ImGui.EndChild();
        }
        ImGui.End();
    }
}