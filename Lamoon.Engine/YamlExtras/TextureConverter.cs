using Lamoon.Graphics;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Lamoon.Engine.YamlExtras; 

public class TextureConverter : IYamlTypeConverter {
    public bool Accepts(Type type) => type.IsAssignableTo(typeof(Texture));

    public object? ReadYaml(IParser parser, Type type) {
        var path = parser.Consume<Scalar>();
        return Texture.FromFilesystem(path.Value);
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type) {
        //emitter.Emit(new Scalar(((Texture)value).Path));
    }
}