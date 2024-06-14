using NekoLib.Scenes;
using Object = NekoLib.Core.Object;

namespace Lamoon.Engine.YamlExtras; 

public class ObjectRef {
    //fixme: maybe make getter on a reference so it will be lazily updated instead of querying all scenes
    public Guid Reference;
    public virtual Object? Object => throw new NotSupportedException();

    public ObjectRef(Guid id) {
        Reference = id;
    }

}