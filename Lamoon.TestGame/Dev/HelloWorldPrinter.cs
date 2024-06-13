using NekoLib.Core;
using Serilog;

namespace Lamoon.TestGame.Dev; 

public class HelloWorldPrinter : Behaviour {
    public string test = "no hello for you";

    void Awake() {
        Log.Information(test);
    }
}