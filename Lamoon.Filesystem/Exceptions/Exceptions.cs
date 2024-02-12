namespace Lamoon.Filesystem.Exceptions; 

public class NoFilesystemException : Exception {
    public NoFilesystemException() : base("There isn't any filesystem mounted!") { }
    
}