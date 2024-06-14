# Lamoon Scene (.lscene)
A scene definition.

## Sample:
```yaml
#
type: Lamoon.Engine.SerializableScene
id: f831ac9a-df26-44de-9444-b2dc0dcdd373
name: Unknown Scene
destroyOnLoad: true
gameObjects:
  - name: "GameObject"
    type: NekoLib.Core.GameObject
    id: 237b4d26-ff48-474f-938a-aad23c780dec
    transform:
      position: [0, 0, 0]
      rotation: [0, 0, 0]
      scale: [1, 1, 1]
    components:
      - type: NekoLib.Core.Component
        id: 2795f9e1-0fb5-49eb-a2c2-afaa6ad435c4
        fields:
          test: !type:System.String hello, from file
    children:
      - name: "GameObject"
        type: NekoLib.Core.GameObject
        id: 822af43b-a808-4d06-a7ca-71c6b3bcd065
        transform:
          position: [ 0, 0, 0 ]
          rotation: [ 0, 0, 0 ]
          scale: [ 1, 1, 1 ]
        components:
          - type: NekoLib.Core.Component
            id: cd202f15-7d77-4057-bb3e-3be46d40fec6
            fields:
              test: !type:System.String hello, from file
              componentRef: !type:Lamoon.Engine.YamlExtras.ComponentRef 2795f9e1-0fb5-49eb-a2c2-afaa6ad435c4
              objectRef: !type:Lamoon.Engine.YamlExtras.GameObjectRef 237b4d26-ff48-474f-938a-aad23c780dec
```