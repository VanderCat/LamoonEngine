using Serilog;

namespace Lamoon.Data; 

internal static class Util {
    public static ILogger Log = Serilog.Log.Logger.ForContext("Name", "Lamoon.Data");
    
    public static Type? FindType(string fullName)
    {
        return
            AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName.Equals(fullName));
    }
}