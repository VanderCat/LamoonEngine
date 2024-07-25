using Lamoon.Graphics;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Lamoon.Engine.YamlExtras; 

public class MaterialConverter : IYamlTypeConverter {
    public bool Accepts(Type type) => type.IsAssignableTo(typeof(Material));

    public object? ReadYaml(IParser parser, Type type) {
        var path = parser.Consume<Scalar>();
        return Material.FromFilesystem(path.Value);
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type) {
        //emitter.Emit(new Scalar(((Texture)value).Path));
    }
}