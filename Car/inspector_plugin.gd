@tool
extends EditorInspectorPlugin

func _parse_begin(object: Object) -> void:
	var control = Button.new()
	add_custom_control(control)
