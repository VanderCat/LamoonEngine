using Serilog;

namespace Lamoon.Filesystem; 

public class FolderFilesystem : IMountable {
    
    public void Dispose() {
        
    }

    public FolderFilesystem(string path, bool allowCreation = false) {
        PhysicalPath = path;
        DirectoryInfo = new DirectoryInfo(path);
        
        if (!DirectoryInfo.Exists)
            if (!allowCreation)
                throw new DirectoryNotFoundException();
            else 
                DirectoryInfo.Create();
    }

    public readonly DirectoryInfo DirectoryInfo;

    public string PhysicalPath { get; }
    public bool IsReadOnly => DirectoryInfo.Attributes.HasFlag(FileAttributes.ReadOnly);
    public void OnMount() {
        Log.Debug("Mounted {0}", PhysicalPath);
    }
    

    public IFile GetFile(string path) {
        return new FolderFile(path, Path.Combine(PhysicalPath, path));
    }

    public IFile CreateFile(string path) {
        throw new NotImplementedException();
    }

    public string[] ListDirectory(string path) {
        throw new NotImplementedException();
    }

    public bool FileExists(string path) {
        return GetFile(path).Exists();
    }
}