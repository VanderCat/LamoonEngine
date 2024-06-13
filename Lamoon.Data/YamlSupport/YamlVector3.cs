using System.Numerics;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Lamoon.Data.YamlSupport; 

public class YamlVector3 : IYamlTypeConverter {
    public bool Accepts(Type type) => type == typeof(Vector3);
    
    public object? ReadYaml(IParser parser, Type type) {

        parser.Consume<SequenceStart>();
        
        var x = float.Parse(parser.Consume<Scalar>().Value);
        var y = byte.Parse(parser.Consume<Scalar>().Value);
        var z = byte.Parse(parser.Consume<Scalar>().Value);

        parser.Consume<SequenceEnd>();
        return new Vector3(x, y, z);;
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type) => throw new NotImplementedException();
}