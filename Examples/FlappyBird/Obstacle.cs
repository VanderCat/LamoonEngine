using System.Drawing;
using System.Numerics;
using FlappyBird.UI;
using Lamoon.Engine;
using Lamoon.Graphics;
using NekoLib.Core;
using NekoLib.Scenes;

namespace FlappyBird; 

public class Obstacle : Behaviour {
    public float Speed = 256f; 
    public static Texture Texture = Texture.FromFilesystem("Textures/obstacle.jpg");
    public Rect Rect;
    public static GameObject SpawnObstacle(GameObject parent) {
        var obstacle = new GameObject("bg");
        obstacle.Transform.Parent = parent.Transform;
        var obstacleLogic = obstacle.AddComponent<Obstacle>();
        obstacleLogic.Rect = obstacle.AddComponent<Rect>();
        var obstacleImage = obstacle.AddComponent<SkiaImage>();
        obstacleImage.Image = Texture;
        obstacleLogic.Rect.Size = new SizeF(128f, 360f);
        return obstacle;
    }

    void Update() {
        if (Transform.LocalPosition.X < -128f) Destroy(GameObject);
        Transform.LocalPosition = new Vector3(
            Transform.LocalPosition.X - Speed * Time.DeltaF,
            Transform.LocalPosition.Y,
            Transform.LocalPosition.Z
        );
        var anotherRect = FindBirdBrains().GameObject.GetComponent<Rect>();
        
        if (AABBCheck(Rect, anotherRect)) SceneManager.LoadScene(new MenuScene());
    }

    bool AABBCheck(Rect rect1, Rect rect2) {
        var x1 = rect1.LocalPosition.X;
        var y1 = rect1.LocalPosition.Y;
        var x2 = rect2.LocalPosition.X;
        var y2 = rect2.LocalPosition.Y;
        var w1 = rect1.Size.Width;
        var h1 = rect1.Size.Height;
        var w2 = rect2.Size.Width;
        var h2 = rect2.Size.Height;
        return x1 < x2 + w2 &&
               x1 + w1 > x2 &&
               y1 < y2 + h2 &&
               y1 + h1 > y2;
    }

    BirdBrains FindBirdBrains() {
        return GameObject.Transform.Parent.GameObject.GetComponentInChildren<BirdBrains>();
    }
}