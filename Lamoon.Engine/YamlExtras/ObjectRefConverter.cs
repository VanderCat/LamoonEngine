using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Lamoon.Engine.YamlExtras; 

public class ObjectRefConverter : IYamlTypeConverter {
    public bool Accepts(Type type) => type.IsAssignableTo(typeof(ObjectRef));

    public object? ReadYaml(IParser parser, Type type) {
        var rawguid = parser.Consume<Scalar>();
        var guid = Guid.Parse(rawguid.Value);
        if (type.IsAssignableTo(typeof(GameObjectRef)))
            return new GameObjectRef(guid);
        if (type.IsAssignableTo(typeof(ComponentRef)))
            return new ComponentRef(guid);
        return new ObjectRef(guid);
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type) {
        throw new NotImplementedException();
    }
}