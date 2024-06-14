using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Lamoon.Data.YamlSupport; 

public class TypeResolver : INodeTypeResolver
{
    public bool Resolve(NodeEvent? nodeEvent, ref Type currentType) {
        if (nodeEvent is null)
            throw new YamlException("An unknown error occured");
        
        if (!nodeEvent.Tag.ToString().StartsWith("!type:")) return false;
        
        var typeName = nodeEvent.Tag.ToString()[6..];
        var type = Util.FindType(typeName);
        Util.Log.Debug("{Type}",typeName);
        if (type == null) return false;
        
        currentType = type;
        return true;

    }
}