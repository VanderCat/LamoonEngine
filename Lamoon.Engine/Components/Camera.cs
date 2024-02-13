using System.Numerics;
using Lamoon.Graphics;
using NekoLib.Core;
using NekoLib.Scenes;

namespace Lamoon.Engine.Components; 

public class Camera : Behaviour {
    public static Camera? CurrentlyDrawing;
    
    public Matrix4x4 ProjectionMatrix {
        get {
            if (Orthographic)
                return Matrix4x4.CreateOrthographic(GraphicsReferences.ScreenSize.Width*OrthoScale,
                    GraphicsReferences.ScreenSize.Width*OrthoScale, ZNear, ZFar);
            var aspectRatio = (float)GraphicsReferences.ScreenSize.Width / GraphicsReferences.ScreenSize.Height;
            return Matrix4x4.CreatePerspectiveFieldOfView(
                float.DegreesToRadians(FieldOfView)*aspectRatio, 
                aspectRatio, 
                ZNear, 
                ZFar
            );
        }    
    }

    public Matrix4x4 ViewMatrix {
        get {
            if(Matrix4x4.Invert(Matrix4x4.CreateTranslation(Transform.LocalPosition)*Matrix4x4.CreateFromQuaternion(Transform.LocalRotation), out var mat))
                return mat;
            throw new Exception("Could not calculate ViewMatrix!");
        }
    }
        
    
    public bool Orthographic = false;
    public float OrthoScale = 1f;
    public float FieldOfView = 78f;
    public float ZNear = 0.1f;
    public float ZFar = 1000f;
    

    void Draw() {
        CurrentlyDrawing = this;
        foreach (var gameObject in SceneManager.ActiveScene.GameObjects) {
            gameObject.SendMessage("Render");
        }
    }
}