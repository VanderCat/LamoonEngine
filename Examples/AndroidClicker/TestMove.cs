using System.Numerics;
using NekoLib.Core;
using Serilog;
using Silk.NET.Input;
using Silk.NET.SDL;
using SkiaSharp;

namespace AndroidClicker; 

public class TestMove : Behaviour {
    private SKTypeface Typeface = SKTypeface.FromFamilyName("Roboto");
    private SKFont Font;

    void Awake() {
        AndroidGame.Instance.InputContext.Mice[0].MouseMove += OnMouse;
        AndroidGame.Instance.InputContext.Mice[0].MouseDown += OnMouseDown;
        Font = new SKFont(Typeface);
    }

    void SkiaDraw(SKCanvas canvas) {
        canvas.DrawText("test", 128, 128, Font, new SKPaint {Color = SKColors.White});
    }

    void Dispose() {
        AndroidGame.Instance.InputContext.Mice[0].MouseMove -= OnMouse;
        AndroidGame.Instance.InputContext.Mice[0].MouseDown -= OnMouseDown;
    }

    void OnMouse(IMouse mouse, Vector2 position) {
        Log.Debug("postion: {Pos}, Mouse: {Mouse}", position, mouse);
    }
    
    void OnMouseDown(IMouse mouse, MouseButton button) {
        
    }
}