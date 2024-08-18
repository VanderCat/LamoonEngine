using Lamoon.Audio;
using NekoLib.Filesystem;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Lamoon.Engine.YamlExtras; 

public class OggSoundFileConverter : IYamlTypeConverter {
    public bool Accepts(Type type) => type == typeof(OggSoundFile);

    public object? ReadYaml(IParser parser, Type type) {
        using var audioStream = Files.GetFile(parser.Consume<Scalar>().Value).GetStream();
        return new OggSoundFile(audioStream);
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type) {
        throw new NotImplementedException();
    }
}