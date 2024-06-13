using Lamoon.Data.YamlSupport;
using Serilog;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Lamoon.Data; 

public abstract class Definition {
    public static TDefinition FromStream<TDefinition>(Stream stream) where TDefinition : Definition {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeConverter(new YamlVector3())
            .Build();
        TDefinition definition;
        try {
            using var sr = new StreamReader(stream);
            definition = deserializer.Deserialize<TDefinition>(sr);
        }
        catch (Exception e) {
            Log.Error(e, "Failed to create {DefinitionName} from file! \n", nameof(TDefinition));
            throw;
        }

        return definition;
    }
}