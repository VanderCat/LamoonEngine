using System.Reflection;
using Object = NekoLib.Core.Object;

namespace Lamoon.Tools; 

public class Inspector : Object {
    public object? Target;

    public virtual void DrawGui() {
        if (Target is null) return;
    }

    public static Inspector? GetInspectorFor(object? target) {
        if (target is null) return null;
        var a = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(domainAssembly => domainAssembly.GetTypes()
            ).First(type => {
                var attr = type.GetCustomAttribute<CustomInspectorAttribute>();
                if (attr is null) return false;
                return target.GetType().IsAssignableTo(attr.InspectType);
            });
        var instance = Activator.CreateInstance(a);
        if (instance is null) return null;
        ((Inspector) instance).Target = target;
        return (Inspector) instance;
    }  
}