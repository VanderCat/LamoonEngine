using System.Drawing;
using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Lamoon.Data.YamlSupport; 

public class YamlSize : IYamlTypeConverter {
    public bool Accepts(Type type) => type == typeof(Size);
    
    public object? ReadYaml(IParser parser, Type type) {

        parser.Consume<SequenceStart>();
        
        var w = int.Parse(parser.Consume<Scalar>().Value);
        var h = int.Parse(parser.Consume<Scalar>().Value);

        parser.Consume<SequenceEnd>();
        return new Size(w, h);;
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type) {
        if (value is null) {
            throw new ArgumentException("the vector can't be null");
        }
        var size = (Size) value;
        emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));
        emitter.Emit(new Scalar(size.Width.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(size.Height.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new SequenceEnd());
    }
}