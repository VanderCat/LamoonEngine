using Lamoon.Engine;

namespace Lamoon.TestGame;

public static class Program {
    public static void Main() {
        var game = NekoGame.Instance;
        game.InitializeWindow();
        game.BindCallbacks(game.Window);
        game.RunWindow();
    }
}