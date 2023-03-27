shader_type spatial;
render_mode blend_add;

uniform vec4 NU_FOGPOWER;

uniform sampler2D custom_texture00;

void fragment() {
	vec4 color = texture(custom_texture00, UV);
	float blend = 1.0 - abs(length(UV - vec2(0.5))) / 0.5;
	ALBEDO = color.rgb;
	ALPHA = color.a * clamp(length(VERTEX) / 1000.0, 0.0, 1.0) * blend;
}
