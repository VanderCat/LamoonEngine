using NekoLib;
using NekoLib.Core;
using NekoLib.Scenes;
using Serilog;
using Object = NekoLib.Core.Object;

namespace Lamoon.Engine.YamlExtras; 

public class GameObjectRef : ObjectRef {
    public override GameObject? Object {
        get {
            foreach (var scene in SceneManager.Scenes) {
                Log.Verbose("searching scene {Scene}", scene);
                var gameObject = scene.GetGameObjectById(Reference);
                if (gameObject is not null)
                    return gameObject;
            }

            return null;
        }
    }
    public GameObjectRef(Guid id) : base(id) { }
}