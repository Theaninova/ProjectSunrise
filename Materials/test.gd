@tool
extends Node

@export var prt: bool = false:
	set(value):
		var shader: Shader = preload("res://Materials/kanban_normal.gdshader")
		print(shader.get_shader_uniform_list(true))
