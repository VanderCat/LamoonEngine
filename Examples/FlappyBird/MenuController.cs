using Lamoon.Engine;
using Lamoon.Engine.Components;
using NekoLib.Core;
using Silk.NET.Input;

namespace FlappyBird; 

public class MenuController : Behaviour {
    private IKeyboard kb;
    public AudioSource Source;
    
    void Start() {
        Source.Play();
        
        kb = BirdGame.Instance.InputContext.Keyboards[0];
        kb.KeyDown += runGame;
    }
    
    private void runGame(IKeyboard kb, Key key, int idk) {
        kb.KeyDown -= runGame;
        Util.LoadSceneFromFilesystem("Scenes/GameScene.lscene"); 
    }
}