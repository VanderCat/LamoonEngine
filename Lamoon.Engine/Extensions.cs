using System.Drawing;
using System.Runtime.CompilerServices;
using Lamoon.Graphics;
using NekoLib.Core;
using NekoLib.Filesystem;
using NekoLib.Scenes;

namespace Lamoon.Engine; 

public static class Extensions {
    public static Texture FromFilesystem(this Texture t, string path) {
        return Texture.FromStream(Files.GetFile(path).GetStream());
    }

    public static GameObject? GetChildByNameRecursively(this GameObject gameObject, string name) {
        foreach (var child in gameObject.Transform) {
            if (child.GameObject.Name == name)
                return child.GameObject;
            var result = child.GameObject.GetChildByNameRecursively(name);
            if (result is not null)
                return result;
        }

        return null;
    }
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetAspectRatio(this Size size) => (float)size.Width / (float)size.Height;
}