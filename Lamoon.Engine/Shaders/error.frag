#version 330 core

in vec2 frag_texCoords;
in vec3 vNormal;

out vec4 out_color;

uniform float uTime = 0;
uniform vec4 color = vec4(1,0,0,1);
uniform mat4 view = mat4(1);
uniform mat4 transform = mat4(1);

void main()
{
    out_color = mix(color, vec4(1,1,1,1),(sin(uTime*16)+0.5)*.25f);
    float cos_theha = clamp( dot( vNormal, normalize((transform*vec4(0,0,0,1)-inverse(view)*vec4(0,0,0,1)).xyz) ), 0,1 );
    vec3 lightColor = vec3(1,1,1);
    vec3 lightFactor = lightColor*cos_theha;
    out_color = vec4(out_color.rgb*lightColor, out_color.a);
    if (out_color.a == 0)
    discard;
}