using Lamoon.Data;
using NekoLib.Core;

namespace Lamoon.Engine; 

public class SerializableScene : SimpleScene {
    public Guid Id;
    private SceneDefinition _sceneDefinition;
    public SerializableScene(SceneDefinition definition) {
        Id = definition.Id;
        Name = definition.Name;
        DestroyOnLoad = definition.DestroyOnLoad;
        _sceneDefinition = definition;
    }

    public override void Initialize() {
        foreach (var definition in _sceneDefinition.GameObjects) {
            Util.CreateGameObjectsFromDefinition(definition);
        }
        base.Initialize();
    }
}