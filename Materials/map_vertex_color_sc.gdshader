shader_type spatial;
render_mode blend_add;

#include "res://Materials/lighting.gdshaderinc"

uniform vec4 NU_FOGPOWER;
uniform vec4 NU_SCROLLSPEED1;

uniform sampler2D custom_texture00;

void fragment() {
	vec2 shift = mod(vec2(TIME) * NU_SCROLLSPEED1.xz, 1.0);
	vec4 color = texture(custom_texture00, UV + shift);
	
	// ALBEDO = vec3(length(VERTEX) / 100.0);
	ALBEDO = color.rgb;
	ALPHA = color.a * max(min(length(VERTEX) / 400.0, 1.0), 0.0);
}
