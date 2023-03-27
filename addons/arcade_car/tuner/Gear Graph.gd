@tool
extends Control

func _ready() -> void:
	set_custom_minimum_size(Vector2(256, 128))

func _draw() -> void:
	draw_line(Vector2(0, size.y), Vector2(size.x, size.y), Color.RED, -1.0)
	draw_line(Vector2(0, 0), Vector2(0, size.y), Color.RED, -1.0)
	
	
	pass
