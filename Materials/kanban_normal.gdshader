shader_type spatial;

uniform vec4 NU_FOGPOWER;
uniform vec4 NU_COMA_MAX;

// test
uniform sampler2D custom_texture00;
uniform sampler2D custom_texture01;

void fragment() {
	float scroll_amount = NU_COMA_MAX.x;
	float scroll_max = NU_COMA_MAX.y;
	
	float scroll_distance = scroll_amount / scroll_max;
	int scroll_progress = int(TIME * 2.0) % int(scroll_max);
	vec2 scroll_uv = UV + scroll_distance * float(scroll_progress);
	
	ALBEDO = texture(custom_texture00, scroll_uv).rgb;
	BACKLIGHT = texture(custom_texture01, scroll_uv).rgb * 10.0;
}
