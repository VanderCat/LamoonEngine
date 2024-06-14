using NekoLib;
using NekoLib.Core;
using NekoLib.Scenes;
using Serilog;
using Object = NekoLib.Core.Object;

namespace Lamoon.Engine.YamlExtras; 

public class ComponentRef : ObjectRef {
    public override Component? Object  {
        get {
            foreach (var scene in SceneManager.Scenes) {
                Log.Verbose("searching scene {Scene}", scene);
                var component = scene.GetComponentById(Reference);
                if (component is not null)
                    return component;
            }
            return null;
        }
    }
    public ComponentRef(Guid id) : base(id) { }
}