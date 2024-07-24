using NekoLib.Core;
using NekoLib.Scenes;

namespace Lamoon.Engine; 

public class PersistantScene : SimpleScene {
    public override string Name => "DontDestroyOnLoad";
    public override bool DestroyOnLoad => false;
}