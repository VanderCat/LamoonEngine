using NekoLib.Core;
using SkiaSharp;

namespace Lamoon.TestGame.Dev; 

public class TransformInfo : Behaviour {
    private SKTypeface Typeface = SKTypeface.FromFamilyName("Roboto Mono NF");
    private SKFont Font;

    public Transform ToWatch;
    
    void Awake() {
        Font = new SKFont(Typeface);
    }
    
    void SkiaDraw(SKCanvas canvas) {
        var hierarchy = ToWatch.ToString();

        var offset = 1;
        foreach (var line in hierarchy.Split("\n")) {
            var size = Font.Size;
            
            canvas.DrawText(line, 0, 0+offset*size, Font, new SKPaint {Color = SKColors.White});
            offset++;
        }
        
    }
    
}