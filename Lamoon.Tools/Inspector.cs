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
            ).Where(type => {
                var attr = type.GetCustomAttribute<CustomInspectorAttribute>();
                if (attr is null) return false;
                return target.GetType().IsAssignableTo(attr.InspectType);
            });
        var b = a.FirstOrDefault(type => {
            var attr = type.GetCustomAttribute<CustomInspectorAttribute>();
            if (attr is null) return false;
            return target.GetType() == attr.InspectType;
        })??a.First();
        var instance = Activator.CreateInstance(b);
        if (instance is null) return null;
        ((Inspector) instance).Target = target;
        return (Inspector) instance;
    }  
}