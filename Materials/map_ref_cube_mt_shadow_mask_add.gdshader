shader_type spatial;

#include "res://Materials/lighting.gdshaderinc"

uniform vec4 NU_FOGPOWER;

uniform sampler2D custom_texture00;
uniform sampler2D custom_texture01;
uniform sampler2D custom_texture02;
uniform sampler2D custom_texture03;

void fragment() {
	vec4 color = texture(custom_texture00, UV);
	ALBEDO = color.rgb;
	vec4 ramp = texture(custom_texture01, UV);
	vec4 metallic = texture(custom_texture03, UV);
	
	ROUGHNESS = 1.0 - ramp.r;
    SPECULAR = ramp.b;
	//METALLIC = 1.0 - metallic.r;
	
	NORMAL_MAP = texture(custom_texture02, UV2).rgb;
	NORMAL_MAP_DEPTH = 0.1;
}
