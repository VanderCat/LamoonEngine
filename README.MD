# Lamoon Engine
*what if unity was made like source engine?*

## About project
An experemental engine that focuses on modding
and expansion of games made on it. It is like
Doom/Quake/Source etc. but made from scratch!

Also features some cutting edge-esque stuff
like net 8.0 and etc.

## Features (Non-exclusive)
1. Virtual Filesystem with simple API to use, and expand
2. Unity-like ECS and Scene Management (powered by NekoLib)
3. It does not have editor (yes i count that as a feature)
4. Skia 2D Rendering
5. Jolt Physics (Heavy WIP)
6. Custom formats for assets, for easy replacement in mods.
7. Cross-Platform! (Primarily developed on Linux)

## What's done?
Not that much done yet. I have basic rendering,
encapsulating of some of the OGL stuff, but
it is pretty much barebones. I'm focusing
now on underlying data. I want this engine
to be Data-Driven (again, like source), so
models, animations, textures, materials, etc.
will be easily overridable.

## Custom formats
Right now there is some developement on
custom formats. I decided to stick with few
principles:
1. Compile raw data for engine to use
2. Don't overengineer stuff
3. Use serialization libraries:
   1. MessagePack
   2. Yaml

### Why MessagePack
I don't see a point why should i make my own
parser of a handmade format, when i can just
use a great (in my opinion) serialization
specification to use, because
1. It's fast
2. It's easy to setup
3. It's binary format

### Why YAML for raw data
Before engine could use something (well i
don't mean it could not use pngs, it just
would be inconvinient for modders) we need
to compile it from an easy to write and look at
file. Yaml is perfect for such things, though
it is rather slow. That's not even an issue,
since we only use it for a specification
for a compiler. That said, i plan to use it
for materials as is anyway.

1. It's pretty
2. It's really really pretty
3. Easy to write by hand


# Trivia
The engine is named after my OC, Alice Lamoon,
a young scientist, that is prototyped and developed
fully featured Neko-Android all by herself. 

You can say, this engine is me, like her, trying
to create something amazing.