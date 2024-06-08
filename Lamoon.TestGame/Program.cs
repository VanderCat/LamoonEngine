using Lamoon.Engine;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace Lamoon.TestGame;

public static class Program {
    public static void Main() {
        var game = (NekoGame)NekoGame.Instance;
        game.WindowOptions = game.WindowOptions with {Size = new Vector2D<int>(1920, 1080), TopMost = true, WindowBorder = WindowBorder.Hidden, WindowState = WindowState.Fullscreen};
        game.InitializeWindow();
        game.BindCallbacks(game.Window);
        game.RunWindow();
    }
}