using System.Drawing;
using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Lamoon.Data.YamlSupport; 

public class YamlSizeF : IYamlTypeConverter {
    public bool Accepts(Type type) => type == typeof(SizeF);
    
    public object? ReadYaml(IParser parser, Type type) {

        parser.Consume<SequenceStart>();
        
        var w = float.Parse(parser.Consume<Scalar>().Value, NumberStyles.Any, CultureInfo.InvariantCulture);
        var h = float.Parse(parser.Consume<Scalar>().Value, NumberStyles.Any, CultureInfo.InvariantCulture);

        parser.Consume<SequenceEnd>();
        return new SizeF(w, h);;
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type) {
        if (value is null) {
            throw new ArgumentException("the size can't be null");
        }
        var size = (SizeF) value;
        emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));
        emitter.Emit(new Scalar(size.Width.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(size.Height.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new SequenceEnd());
    }
}