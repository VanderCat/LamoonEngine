# Lamoon Model (.lmdl)
A YAML definition of a model.

```yaml
info:
  author: VanderCat
  name: Sample model
  description: a sample model
meshList:
  - Raw/Models/test.obj
materials:
  - Materials/testmat
materialRedefinition:
  funny.png: 1 #index in materials
```

# Lamoon Model Compiled (.lmdlc)
A MessagePack serialized instance of a class
`SerializedModel`.