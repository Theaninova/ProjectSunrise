@tool
extends ColorRect

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	var mat = material as ShaderMaterial
	var t = Texture3D.new()

	mat.set_shader_parameter("ForwardTexture", %Forward.get_texture())
	mat.set_shader_parameter("BackTexture", %Backward.get_texture())
	mat.set_shader_parameter("LeftTexture", %Left.get_texture())
	mat.set_shader_parameter("RightTexture", %Right.get_texture())
	mat.set_shader_parameter("TopTexture", %Top.get_texture())
	mat.set_shader_parameter("BottomTexture", %Bottom.get_texture())

	pass
