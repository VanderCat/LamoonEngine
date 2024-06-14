using System.Globalization;
using System.Numerics;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Lamoon.Data.YamlSupport; 

public class YamlVector3 : IYamlTypeConverter {
    public bool Accepts(Type type) => type == typeof(Vector3);
    
    public object? ReadYaml(IParser parser, Type type) {

        parser.Consume<SequenceStart>();
        
        var x = float.Parse(parser.Consume<Scalar>().Value, NumberStyles.Any, CultureInfo.InvariantCulture);
        var y = float.Parse(parser.Consume<Scalar>().Value, NumberStyles.Any, CultureInfo.InvariantCulture);
        var z = float.Parse(parser.Consume<Scalar>().Value, NumberStyles.Any, CultureInfo.InvariantCulture);

        parser.Consume<SequenceEnd>();
        return new Vector3(x, y, z);;
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type) {
        if (value is null) {
            throw new ArgumentException("the vector can't be null");
        }
        var vector = (Vector3) value;
        emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));
        emitter.Emit(new Scalar(vector.X.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(vector.Y.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(vector.Z.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new SequenceEnd());
    }
}