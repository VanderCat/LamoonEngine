using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Scalar = YamlDotNet.Core.Events.Scalar;

namespace Lamoon.Data.YamlSupport; 

public class FilesystemPathsResolver  : IYamlTypeConverter {
    public bool Accepts(Type type) => type == typeof(GameManifest.FilesystemPaths);

    public object? ReadYaml(IParser parser, Type type) {
        var filesystem = new GameManifest.FilesystemPaths();
        parser.Consume<SequenceStart>();
        while (parser.TryConsume<Scalar>(out var scalar)) {
            var value = scalar.Value;
            switch (scalar.Tag.Value) {
                case "!bin":
                    filesystem.Bin = scalar.Value;
                    break;
                case "!mount":
                    filesystem.Mount.Add(scalar.Value);
                    break;
            }
        }

        parser.Consume<SequenceEnd>();
        return filesystem;
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type) {
        throw new NotImplementedException();
    }
}