namespace Lamoon.Data; 

public class GameManifest : Definition {
    public class GameMeta {
        public string[]? Developers;
        public string? Website;
        public string? License;
    }

    public class FilesystemPaths {
        public string Bin;
        public List<string> Mount = new();
    }
    
    public string Game = "Lamoon";
    public GameMeta? Meta;
    public FilesystemPaths Filesystem;
}