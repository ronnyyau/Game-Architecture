#version 330
 
in vec2 v_TexCoord;
in vec3 v_Normal;
in vec3 v_FragPos;
uniform sampler2D s_texture;

out vec4 Color;
 
void main()
{
	vec3 lightColor = vec3(1,1,1);
	vec4 lightAmbient = vec4(0.1, 0.1, 0.1, 1.0);
	vec3 lightPos = vec3(0,15,3);

	vec3 norm = normalize(v_Normal);
	vec3 lightDir = normalize(lightPos - v_FragPos); 
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * lightColor;

    Color = lightAmbient + (texture2D(s_texture, v_TexCoord) * vec4(diffuse, 0));
}