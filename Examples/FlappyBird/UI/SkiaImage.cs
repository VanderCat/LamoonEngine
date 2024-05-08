using Lamoon.Engine.Components;
using Lamoon.Graphics;
using Lamoon.Graphics.Skia;
using NekoLib.Core;
using SkiaSharp;

namespace FlappyBird.UI; 

public class SkiaImage : Behaviour {
    private Texture _texture;
    private SKImage _skImage;
    private bool isDirty = true;
    private Rect _rect;
    public Texture Image {
        get {
            return _texture;
        }
        set {
            isDirty = true;
            _texture = value;
        }
    }

    void Awake() {
        // TODO: to be more inline with unity create aliases for components like Component.GetComponent
        _rect = GameObject.GetComponent<Rect>();
    }

    void SkiaDraw(SKCanvas canvas) {
        if (isDirty) UpdateImage(canvas);
        canvas.DrawImage(_skImage, _rect.ToSKRect());
    }

    void UpdateImage(SKCanvas canvas) {
        _skImage = SKImage.FromTexture(
            Skia.Instance.GrContext,
            new GRBackendTexture(
                _texture.Size.Width,
                _texture.Size.Height,
                true,
                new GRGlTextureInfo((uint)_texture.Type, _texture.OpenGlHandle, (uint)_texture.Format)
            ),
            SKColorType.Rgba8888
        ); //Figure out mipmap stuff, and color type conversion
        isDirty = false;
    }
}