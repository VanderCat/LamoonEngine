using Serilog;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Lamoon.Data.YamlSupport; 

public class YamlType : IYamlTypeConverter {
    public bool Accepts(Type type) => typeof(Type).IsAssignableFrom(type);

    public object ReadYaml(IParser parser, Type type)
    {
        var value = parser.Consume<Scalar>().Value;
        var t = Util.FindType(value);
        Util.Log.Debug("{Meow}",GetType().AssemblyQualifiedName);
        if (t is null) {
            throw new ArgumentException();
        }
        return t;
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        var systemType = (Type)value!;
        emitter.Emit(new Scalar(AnchorName.Empty, TagName.Empty, systemType.AssemblyQualifiedName!, ScalarStyle.Any, true, false));
    }
}