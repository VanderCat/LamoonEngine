# Lamoon Material (.lmat)
A material definition.

## `shader` : str
a path within file

## Sample:
```yaml
#
shader: Shaders/Default
textures:
  - Materials/texture
color: [1, 1, 1, 1]
backfaceCulling: True
uniform:
  normalMap: Materials/normal
```