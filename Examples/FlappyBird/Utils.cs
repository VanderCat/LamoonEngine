using FlappyBird.UI;
using NekoLib.Core;
using SkiaSharp;

namespace FlappyBird; 

public static class Utils {
    public static (GameObject, TComponent) GameObjectWithComponent<TComponent>(string? name = null) where TComponent : Component, new() {
        var gameObject = new GameObject(name??"GameObject"); 
        //TODO: be able to pass null string to gameObject 
        var component = gameObject.AddComponent<TComponent>();
        return (gameObject, component);
    }

    public static SKRect ToSKRect(this Rect rect) {
        return SKRect.Create(rect.LocalPosition.X, rect.LocalPosition.Y, rect.Size.Width, rect.Size.Height);
    }
}