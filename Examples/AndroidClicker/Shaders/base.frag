#version 300 es

in vec2 frag_texCoords;

out vec4 out_color;

uniform sampler2D uTexture;
uniform mat4 transform;
uniform mat4 view;
uniform mat4 projection;
uniform vec4 color;

void main()
{
    out_color = texture(uTexture, frag_texCoords)*color;
    if (abs(out_color.a) < 0.001)
        discard;
}