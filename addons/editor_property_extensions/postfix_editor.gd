@tool
extends EditorProperty

var edit = SpinBox.new()
var current_value: float = 0

func _init():
	add_child(edit)
	add_focusable(edit)
	edit.value_changed.connect(func(value): emit_changed(get_edited_property(), value))

func _update_property() -> void:
	edit.value = get_edited_object()[get_edited_property()]
