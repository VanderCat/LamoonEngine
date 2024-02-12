using Lamoon.Filesystem;
using Lamoon.Graphics;

namespace Lamoon.Engine; 

public static class Extensions {
    public static Texture FromFilesystem(this Texture t, string path) {
        return Texture.FromStream(Files.GetFile(path).GetStream());
    }
}