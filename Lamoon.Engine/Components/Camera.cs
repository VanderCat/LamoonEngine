using System.Drawing;
using System.Numerics;
using Lamoon.Graphics;
using NekoLib.Core;
using NekoLib.Scenes;
using Silk.NET.OpenGL;
using Framebuffer = Lamoon.Graphics.Framebuffer;
using Renderbuffer = Lamoon.Graphics.Renderbuffer;
using Shader = Lamoon.Graphics.Shader;
using Texture = Lamoon.Graphics.Texture;

namespace Lamoon.Engine.Components;

[ToolsIcon(MaterialIcons.Videocam)]
public class Camera : Behaviour {
    public static Camera? CurrentlyDrawing { get; private set; }
    public static Camera? MainCamera { get; private set; }
    public Size? RenderSize { get; private set; }
    private Size SizeToRender => RenderSize ?? GraphicsReferences.ScreenSize;

    public Matrix4x4 ProjectionMatrix {
        get {
            if (Orthographic)
                return Matrix4x4.CreateOrthographic(SizeToRender.Width * OrthoScale,
                    SizeToRender.Height * OrthoScale, ZNear, ZFar);
            var aspectRatio = (float) SizeToRender.Width / SizeToRender.Height;
            return Matrix4x4.CreatePerspectiveFieldOfView(
                float.DegreesToRadians(FieldOfView),
                aspectRatio,
                ZNear,
                ZFar
            );
        }
    }

    public Matrix4x4 ViewMatrix {
        get {
            if (Matrix4x4.Invert(
                    Matrix4x4.CreateFromQuaternion(Transform.LocalRotation) *
                    Matrix4x4.CreateTranslation(Transform.LocalPosition), out var mat))
                return mat;
            throw new Exception("Could not calculate ViewMatrix!");
        }
    }


    public bool Orthographic = false;
    public float OrthoScale = 1f;
    public float FieldOfView = 78f;
    public float ZNear = 0.1f;
    public float ZFar = 1000f;

    private bool _isMain = false;

    public bool IsMain {
        get => _isMain;
        set {
            if (MainCamera is not null) MainCamera._isMain = false;
            MainCamera = value ? this : null;
            _isMain = value;
        }
    }

    private Framebuffer _fbo;
    private Renderbuffer _rbo;
    public Texture RenderTexture;
    private bool _separateRenderingStarted = false;

    private void SetupSeparateRendering() {
        _fbo = new Framebuffer();
        var size = SizeToRender;
        _rbo = new Renderbuffer(size);
        RenderTexture = new Texture(size);
        RenderTexture.MagFilter = TextureMagFilter.Linear;
        RenderTexture.MinFilter = TextureMinFilter.Linear;
        _fbo.SetRenderTexture(RenderTexture);
        _fbo.SetRenderBuffer(_rbo);
    }

    private void ShutdownSeparateRendering() {
        if (!_separateRenderingStarted) return;
        _separateRenderingStarted = false;
        _rbo.Dispose();
        _fbo.Dispose();
        RenderTexture.Dispose();
    }
    public void ChangeSize(Size size) {
        if (_separateRenderingStarted) ShutdownSeparateRendering();
        RenderSize = size;
        if (_separateRenderingStarted) SetupSeparateRendering();
    }
    public void ResetSize() {
        if (_separateRenderingStarted) ShutdownSeparateRendering();
        RenderSize = null;
        if (_separateRenderingStarted) SetupSeparateRendering();
    }

    void Awake() {
        SetupSeparateRendering();
    }

    void Draw() {
        var gl = GraphicsReferences.OpenGl;
        _fbo.Bind();
        gl.Enable(EnableCap.DepthTest);
        gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        CurrentlyDrawing = this;
        foreach (var gameObject in SceneManager.ActiveScene.GameObjects) {
            gameObject.SendMessage("Render");
        }

        CurrentlyDrawing = null;
        _fbo.Unbind();
        if (IsMain) DrawRenderTextureOnScreen();
    }

    void DrawRenderTextureOnScreen() {
        var gl = GraphicsReferences.OpenGl;
        gl.BindVertexArray(Mesh.Quad.VaoHandle);
        var shader = Shader.Default;
        shader.SetMatrix4x4("transform", Matrix4x4.Identity);
        shader.SetMatrix4x4("view", Matrix4x4.Identity);
        shader.SetMatrix4x4("projection", Matrix4x4.Identity);
        Immedieate.UseShader(shader);
        Immedieate.BindTexture(RenderTexture);
        
        gl.DepthMask(false);
        gl.DepthFunc(DepthFunction.Always); 
        unsafe {
            gl.DrawElements(Mesh.Quad.PrimitiveType, (uint) Mesh.Quad.Indices.Length, DrawElementsType.UnsignedInt,
                (void*) 0);
        }
        gl.DepthMask(true);
        gl.DepthFunc(DepthFunction.Less);
        gl.BindVertexArray(0);
    }

}