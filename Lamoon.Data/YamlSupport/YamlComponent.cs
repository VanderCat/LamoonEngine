using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Lamoon.Data.YamlSupport; 

public class YamlComponent : IYamlTypeConverter {
    public bool Accepts(Type type) => typeof(ComponentDefinition).IsAssignableFrom(type);

    public object? ReadYaml(IParser parser, Type type) {
        var definition = new ComponentDefinition();
        parser.Consume<MappingStart>();
        while (parser.TryConsume<Scalar>(out var name)) {
            if (name is null)
                throw new YamlException("An unknown error occured");
            var field = definition.GetType().GetField(name.Value);
            if (field is null) {
                Util.Log.Warning("An unknown field {FieldName} have been passed to a component definition. Skipping...", name.Value);
                parser.SkipThisAndNestedEvents();
                break;
            }
            /*switch (field.FieldType) {
                parser.
            }*/
        }
        throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type) {
        throw new NotImplementedException();
    }
}