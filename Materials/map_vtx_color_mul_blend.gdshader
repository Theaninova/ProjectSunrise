///
/// Shadow Maps
///
shader_type spatial;
render_mode blend_mul;

uniform vec4 NU_FOGPOWER;

uniform sampler2D custom_texture00;

void fragment() {
	vec4 color = texture(custom_texture00, UV);
	ALBEDO = color.rgb * COLOR.rgb;
	ALPHA = 1.0 - mix(color.r, color.g, color.b);
}

void light() {
	DIFFUSE_LIGHT = ALBEDO;
}
