using Lamoon.Engine;

namespace FlappyBird;

public static class Program {
    public static void Main() {
        var game = BirdGame.Instance;
        game.WindowOptions = game.WindowOptions;
        game.InitializeWindow();
        game.BindCallbacks(game.Window);
        game.RunWindow();
    }
}