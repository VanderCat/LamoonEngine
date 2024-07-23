using AngleSharp;
using AngleSharp.Css.Dom;
using AngleSharp.Dom;
using Lamoon.Graphics;
using NekoLib.Core;
using Serilog;
using SkiaSharp;
using Yoga;

namespace Lamoon.UI; 

public class HtmlUi : Behaviour {
    public IConfiguration Config = Configuration.Default;
    public IDocument Document;
    private IBrowsingContext Context;
    private YogaDrawable _yogaRoot;

    void Awake() {
        Context = BrowsingContext.New(Config);
    }
    
    public void LoadInlineHtml(string htmlString) {
        if (!GameObject.Initialized) throw new NullReferenceException();
        Document = Context.OpenAsync(req => req.Content(htmlString)).Result;
        _yogaRoot = new YogaDrawable {
            Width = GraphicsReferences.ScreenSize.Width,
            Height = GraphicsReferences.ScreenSize.Height
        };

        foreach (var element in Document.Children) {
            ConstructYogaDom(element);
        }
        _yogaRoot.CalculateLayout();
        _yogaRoot.Print(YogaPrintOptions.Style);
    }

    private void ConstructYogaDom(IElement element, YogaDrawable? parent = null) {
        var node = new YogaDrawable();
        Log.Debug("processing element {ElementName}", element);
        parent ??= _yogaRoot;
        parent.Children.Add(node);
        var nodeStyle = element.ComputeCurrentStyle();
        Log.Debug("Node style {Style}", nodeStyle);
        //node.Paint.Color = 
        if (element.HasTextNodes()) {
            var textNode = new YogaDrawable {
                Type = YogaNodeType.Text,
                Text = element.Text()
            };
            node.Children.Add(textNode);
        }
        foreach (var elementChild in element.Children) {
            ConstructYogaDom(elementChild, node);
        }
    }

    void SkiaDraw(SKCanvas canvas) {
        _yogaRoot.Draw(canvas, 0, 0);
    }
    
}