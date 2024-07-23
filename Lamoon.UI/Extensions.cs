using System.Numerics;
using SkiaSharp;
using Yoga;

namespace Lamoon.UI; 

public static class Extensions {
    public static void Draw(this YogaDrawable node, SKCanvas canvas, float offsetX, float offsetY) {
        canvas.DrawRect(new SKRect(node.Left+offsetX, node.Top+offsetY, node.Width, node.Height), node.Paint);
        
        foreach (YogaDrawable child in node.Children) {
            if (child.Type == YogaNodeType.Default)
                child.Draw(canvas, offsetX+node.Left, offsetY+node.Top);
            else
                canvas.DrawText(child.Text, offsetX+node.Left, offsetY+node.Top, child.Paint);
        }
    }
}