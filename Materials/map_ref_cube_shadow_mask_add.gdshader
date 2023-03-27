/// Various props that may have alpha blend in some cases
shader_type spatial;
render_mode depth_prepass_alpha;

#include "res://Materials/lighting.gdshaderinc"

uniform vec4 NU_FOGPOWER;

uniform sampler2D custom_texture00;
uniform sampler2D custom_texture01;

void fragment() {
	vec4 color = texture(custom_texture00, UV);
	ALBEDO = color.rgb;
	NORMAL_MAP = texture(custom_texture01, UV).rgb;
	
	ALPHA = color.a;
	ALPHA_SCISSOR_THRESHOLD = 0.5;
}
