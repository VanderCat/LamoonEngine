using NekoLib.Core;
using Serilog;
using SkiaSharp;

namespace Lamoon.Engine.Dev; 

public class Hierarchy : Behaviour {
    private SKTypeface Typeface = SKTypeface.FromFamilyName("Roboto Mono NF");
    private SKFont Font;

    void Awake() {
        Font = new SKFont(Typeface);
    }

    private string GetComponentHierarchy(GameObject go, int level) {
        var componentList = "";
        var componentIndex = 0;
        foreach (var component in go.GetComponents()) {
            componentList = $"{componentList}{new string(' ', level*4)}{componentIndex}. {component.ToString()}\n";
            componentIndex++;
        }

        return componentList;
    }
    private string GetSceneHierarchy() {
        var text = "";
        var index = 0;
        foreach (var go in GameObject.Scene.GetRootGameObjects()) {
            text = $"{text} {index}. {go.Name}\n{GetComponentHierarchy(go, 1)}\n{GetObjectHierarchy(go, 1)}\n";
            index++;
        }

        return text;
    }

    private string GetObjectHierarchy(GameObject gameObject, int level) {
        var text = "";
        foreach (var transform in gameObject.Transform) {
            text = $"{text}{new string(' ', level*4)}{level}. {transform.GameObject.Name}\n{GetComponentHierarchy(transform.GameObject, level+1)}\n{GetObjectHierarchy(transform.GameObject, level+1)}";
        }

        return text;
    }
    
    void SkiaDraw(SKCanvas canvas) {
        var hierarchy = GetSceneHierarchy();

        var offset = 1;
        foreach (var line in hierarchy.Split("\n")) {
            var size = Font.Size;
            
            canvas.DrawText(line, 0, 0+offset*size, Font, new SKPaint {Color = SKColors.White});
            offset++;
        }
        
    }
}