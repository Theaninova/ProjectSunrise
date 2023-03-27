@tool
extends EditorPlugin

var angle_editor_plugin

func _enter_tree() -> void:
	angle_editor_plugin = preload("res://addons/editor_property_extensions/property_extensions_inspector_plugin.gd").new()
	add_inspector_plugin(angle_editor_plugin)


func _exit_tree() -> void:
	remove_inspector_plugin(angle_editor_plugin)
