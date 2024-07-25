using System.Drawing;
using Lamoon.Graphics;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Lamoon.Engine.YamlExtras; 

public class SpriteConverter : IYamlTypeConverter {
    public bool Accepts(Type type) => type.IsAssignableTo(typeof(Sprite));

    public object? ReadYaml(IParser parser, Type type) {
        parser.Consume<MappingStart>();
        Material? material = null;
        Rectangle? bounds = null;
        while (parser.TryConsume<Scalar>(out var key)) {
            var value = parser.Consume<Scalar>();
            switch (key.Value) {
                case "Material":
                    material = Material.FromFilesystem(value.Value);
                    continue;
                case "Bounds":
                    parser.Consume<MappingStart>();
                    bounds = new Rectangle(
                        int.Parse(parser.Consume<Scalar>().Value), 
                        int.Parse(parser.Consume<Scalar>().Value), 
                        int.Parse(parser.Consume<Scalar>().Value), 
                        int.Parse(parser.Consume<Scalar>().Value));
                    continue;
            }
        }
        parser.Consume<MappingEnd>();
        if (bounds is not null) 
            return new Sprite(material ?? Material.Default, bounds.Value);
        return new Sprite(material ?? Material.Default);
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type) {

    }
}