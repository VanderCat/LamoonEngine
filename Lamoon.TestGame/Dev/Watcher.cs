using NekoLib.Core;

namespace Lamoon.TestGame.Dev; 

public class Watcher : Behaviour {
    public Transform watch;

    void Update() {
        Transform.LookAt(watch.Position);
    }
}