#version 330 core

in vec2 frag_texCoords;

out vec4 out_color;

uniform sampler2D uTexture;
uniform mat4 transform = mat4(1);
uniform mat4 view = mat4(1);
uniform mat4 projection = mat4(1);
uniform vec4 color = vec4(1);

void main()
{
    out_color = texture(uTexture, frag_texCoords)*color;
    if (out_color.a == 0)
        discard;
}