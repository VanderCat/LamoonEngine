#version 300 es

in vec2 frag_texCoords;
in vec3 vNormal;

out vec4 out_color;

uniform float uTime = 0;
uniform vec4 color;
uniform mat4 view;
uniform mat4 transform;

void main()
{
    out_color = mix(color, vec4(1f,1f,1f,1f),(sin(uTime*16f)+0.5f)*.25f);
    float cos_theha = clamp( dot( vNormal, normalize((transform*vec4(0f,0f,0f,1f)-inverse(view)*vec4(0f,0f,0f,1f)).xyz) ), 0f,1f );
    vec3 lightColor = vec3(1f,1f,1f);
    vec3 lightFactor = lightColor*cos_theha;
    out_color = vec4(out_color.rgb*lightColor, out_color.a);
    if (abs(out_color.a) < 0.001)
    discard;
}