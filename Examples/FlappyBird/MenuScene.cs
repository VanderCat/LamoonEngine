using System.Drawing;
using FlappyBird.UI;
using Lamoon.Audio;
using Lamoon.Engine.Components;
using Lamoon.Filesystem;
using Lamoon.Graphics;
using NekoLib.Core;
using NekoLib.Scenes;
using Serilog;
using Silk.NET.Input;
using Silk.NET.OpenAL;
using Object = NekoLib.Core.Object;

namespace FlappyBird; 

public class MenuScene : BirdScene {
    
    private IKeyboard kb;
    
    public override void Initialize() {
        var cameraGameObject = new GameObject("Camera");
        var camera = cameraGameObject.AddComponent<Camera>();
        camera.IsMain = true;

        var canvasGameObject = new GameObject("UI");
        canvasGameObject.AddComponent<SkiaCanvas>();
        
        var testBg = new GameObject("bird");
        testBg.Transform.Parent = canvasGameObject.Transform;
        var bgRect = testBg.AddComponent<Rect>();
        var bgImage = testBg.AddComponent<SkiaImage>();
        bgImage.Image = Texture.FromFilesystem("Textures/bg.png");
        bgRect.Size = new SizeF(1280f, 720f);
        
        var logo = new GameObject("logo");
        logo.Transform.Parent = canvasGameObject.Transform;
        var logoRect = logo.AddComponent<Rect>();
        var logoImage = logo.AddComponent<SkiaImage>();
        logoImage.Image = Texture.FromFilesystem("Textures/logo.png");
        logoRect.Size = new SizeF(1000f, 600f);
        logo.AddComponent<LogoAnim>();
        
        var audioGo = new GameObject("Audio");
        audioGo.Transform.Parent = canvasGameObject.Transform;
        audioGo.AddComponent<AudioListener>();
        var audio = audioGo.AddComponent<AudioSource>();
        using var audioStream = Files.GetFile("Music/menu.ogg").GetStream();
        audio.Track = new OggSoundFile(audioStream, false);
        audio.IsLooping = true;
        Log.Debug("{Error}",AL.GetApi().GetError());
        audio.Play();
        Log.Debug("{Error}",AL.GetApi().GetError());
        
        kb = BirdGame.Instance.InputContext.Keyboards[0];
        kb.KeyDown += runGame;
        kb.KeyDown += (_, _, _) => {
            Object.Destroy(audio);
        };
        base.Initialize();
    }

    private void runGame(IKeyboard kb, Key key, int idk) {
        kb.KeyDown -= runGame;
        SceneManager.LoadScene(new GameScene());
        
    }
}