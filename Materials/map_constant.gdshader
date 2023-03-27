shader_type spatial;

#include "res://Materials/lighting.gdshaderinc"

uniform sampler2D custom_texture00;

void fragment() {
	vec4 color = texture(custom_texture00, UV);
	ALBEDO = color.rgb;
	ALPHA = color.a;
}
