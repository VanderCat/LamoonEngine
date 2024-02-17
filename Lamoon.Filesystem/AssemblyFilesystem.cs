using System.Reflection;
using Serilog;

namespace Lamoon.Filesystem; 

public class AssemblyFilesystem : IMountable {
    public void Dispose() {
        
    }

    public string MountPoint { get; private set; }
    public string PhysicalPath => Assembly.Location;
    public bool IsReadOnly => true;

    public Assembly Assembly;

    public AssemblyFilesystem(Assembly assembly) {
        Assembly = assembly;
    }

    public AssemblyFilesystem() : this(Assembly.GetEntryAssembly()) { }

    public void OnMount() {
        Log.Debug("Mounted assembly fs for {0}", Assembly.FullName);
    }

    private string TransfromPath(string path) => Assembly.GetName().Name+"."+path.Replace("/", ".");

    public IFile GetFile(string path) {
        if (!FileExists(path))
            throw new FileNotFoundException();
        var assemblyPath = TransfromPath(path);
        return new AssemblyFile(assemblyPath, path, this);
    }

    public IFile CreateFile(string path) {
        throw new NotSupportedException();
    }

    public string[] ListDirectory(string path) {
        var assemblyNames = Assembly.GetManifestResourceNames();
        var fsNames = new List<string>();
        foreach (var name in assemblyNames) {
            var newName = name.Substring(Assembly.GetName().FullName.Length + 1).Replace(".", "/");
            var lastIdx = newName.LastIndexOf("/", StringComparison.Ordinal);
            newName = newName.Remove(lastIdx).Insert(lastIdx, ".");
            fsNames.Add(newName);
        }
        return fsNames.ToArray();
    }

    public bool FileExists(string path) {
        var assemblyPath = TransfromPath(path);
        Log.Verbose(assemblyPath);
        using var stream = Assembly.GetManifestResourceStream(assemblyPath);
        return stream is not null;
    }
}