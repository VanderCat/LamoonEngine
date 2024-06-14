using System.Drawing;
using Lamoon.Data.YamlSupport;
using Serilog;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Lamoon.Data; 

public abstract class Definition {

    public static List<IYamlTypeConverter> TypeConverters = new() {
        new YamlVector3(),
        new YamlSize(),
        new YamlSizeF()
    };
    
    public static TDefinition FromStream<TDefinition>(Stream stream) where TDefinition : Definition {
        var deserializerBuilder = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance);
        foreach (var typeConverter in TypeConverters) {
            deserializerBuilder.WithTypeConverter(typeConverter);
        }
        var deserializer = deserializerBuilder.WithNodeTypeResolver(new TypeResolver())
            .Build();
        TDefinition definition;
        try {
            using var sr = new StreamReader(stream);
            definition = deserializer.Deserialize<TDefinition>(sr);
        }
        catch (Exception e) {
            Util.Log.Error(e, "Failed to create {DefinitionName} from file! \n", nameof(TDefinition));
            throw;
        }

        return definition;
    }
}