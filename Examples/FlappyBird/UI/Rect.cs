using System.Drawing;
using System.Numerics;
using NekoLib.Core;

namespace FlappyBird.UI; 

public class Rect : Behaviour {
    public Rect? Parent {
        get {
            var parent = Transform.Parent;
            if (parent is null) return null;
            if (parent.GameObject.HasComponent<Rect>())
                return parent.GameObject.GetComponent<Rect>();
            return null;
        }
    }

    public SizeF Size;

    public Vector2 LocalPosition {
        get => new(Transform.LocalPosition.X, Transform.LocalPosition.Y);
        set => Transform.LocalPosition = new(value, Transform.LocalPosition.Z);
    }
}