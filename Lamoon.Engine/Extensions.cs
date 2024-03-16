using System.Drawing;
using System.Runtime.CompilerServices;
using Lamoon.Filesystem;
using Lamoon.Graphics;
using NekoLib.Core;
using NekoLib.Scenes;

namespace Lamoon.Engine; 

public static class Extensions {
    public static Texture FromFilesystem(this Texture t, string path) {
        return Texture.FromStream(Files.GetFile(path).GetStream());
    }

    public static List<GameObject> GetRootGameObjects(this IScene scene) {
        return scene.GameObjects.Where(o => o.Transform.Parent is null).ToList();
    }

    public static GameObject? GetByName(this IScene scene, string name) {
        return scene.GameObjects.First(o => o.Name == name);
    }

    public static GameObject? GetChildByName(this GameObject gameObject, string name) {
        return gameObject.Transform.First(o => o.GameObject.Name == name).GameObject;
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