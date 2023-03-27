@tool
extends EditorInspectorPlugin

var PostfixEditor = preload("res://addons/editor_property_extensions/postfix_editor.gd")

func _can_handle(object: Object) -> bool:
	return true

func _parse_property(object: Object, type, name: String, hint_type, hint_string: String, usage_flag, wide: bool) -> bool:
	#if type == TYPE_FLOAT and name.ends_with("speed"):
	#	var edit = PostfixEditor.new()
	#	edit.edit.suffix = "km/h"
	#	add_property_editor(name, edit)
	#	return true
	#if type == TYPE_FLOAT and name.ends_with("acceleration"):
	#	var edit = PostfixEditor.new()
	#	edit.edit.suffix = "m/s²"
	#	add_property_editor(name, edit)
	#	return true
	#if type == TYPE_FLOAT and name.ends_with("radius"):
	#	var edit = PostfixEditor.new()
	#	edit.edit.suffix = "m"
	#	add_property_editor(name, edit)
	#	return true
	#if type == TYPE_FLOAT and name.ends_with("weight"):
	#	var edit = PostfixEditor.new()
	#	edit.edit.suffix = "kg"
	#	add_property_editor(name, edit)
	#	return true
	#else:
	return false
