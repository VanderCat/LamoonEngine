using NekoLib.Core;

namespace Lamoon.Engine.Dev; 

public class Watcher : Behaviour {
    public Transform watch;

    void Update() {
        Transform.LookAt(watch.Position);
    }
}