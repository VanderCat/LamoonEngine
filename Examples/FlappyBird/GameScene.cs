using System.Drawing;
using FlappyBird.UI;
using Lamoon.Engine.Components;
using Lamoon.Graphics;
using NekoLib.Core;

namespace FlappyBird; 

public class GameScene : BirdScene {
    public override void Initialize() {
        var cameraGameObject = new GameObject("Camera");
        var camera = cameraGameObject.AddComponent<Camera>();
        camera.IsMain = true;

        var canvasGameObject = new GameObject("UI");
        canvasGameObject.AddComponent<SkiaCanvas>();
        
        var testBg = new GameObject("bg");
        testBg.Transform.Parent = canvasGameObject.Transform;
        var bgRect = testBg.AddComponent<Rect>();
        var bgImage = testBg.AddComponent<SkiaImage>();
        bgImage.Image = Texture.FromFilesystem("Textures/bg.png");
        bgRect.Size = new SizeF(1280f, 720f);
        
        var testBird = new GameObject("bird");
        testBird.Transform.Parent = canvasGameObject.Transform;
        var birdRect = testBird.AddComponent<Rect>();
        var birdImage = testBird.AddComponent<SkiaImage>();
        birdImage.Image = Texture.FromFilesystem("Textures/bird.jpg");
        birdRect.Size = new SizeF(64f, 64f);
        testBird.AddComponent<BirdBrains>();
        
        var generatorGo = new GameObject("generator");
        var generator = generatorGo.AddComponent<ObstacleGenerator>();
        generator.canvas = canvasGameObject;
        
        base.Initialize();
    }
}