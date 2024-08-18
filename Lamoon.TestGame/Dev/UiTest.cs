using Lamoon.Engine;
using Lamoon.Engine.Components;
using Lamoon.Filesystem;
using Lamoon.UI;
using NekoLib.Core;
using NekoLib.Filesystem;

namespace Lamoon.TestGame.Dev; 

public class UiTest : SimpleScene {
    public override void Initialize() {
        var a = new GameObject("Ui");
        var html = a.AddComponent<HtmlUi>();
        a.AddComponent<SkiaCanvas>();
        base.Initialize();
        html.LoadInlineHtml(Files.GetFile("Ui/test.html").Read());
        //html.LoadInlineHtml("<h1 style='background-color: #ffffff; color: #000000; width: 100px; height: 20px;'>Some example source</h1><p>This is a paragraph element</p>");
    }
}