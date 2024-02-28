# Lamoon Sprite
A basic simple image definition (e.g. for 2D Games),
you can also override some material settings here

## Example:
```yaml
material: Materials\Sprite01
override: #Material Override
  texture: Sprites\Character
frames:
  - origin: [0,0,16,16]
    quad: [0,0]
animationSpeed: 1 #in seconds for frame
#etc. wip
```