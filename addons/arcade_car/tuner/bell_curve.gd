@tool
extends Control

class_name BellCurve2D

@export var start_point: Vector2 = Vector2(0, 0)
@export_exp_easing("inout") var start_inter: float = -2
@export var mid_point: Vector2 = Vector2(50, 100)
@export_exp_easing("attenuation") var end_inter: float = -2
@export var end_point: Vector2 = Vector2(100, 100)

@export var resolution: int = 10

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _draw() -> void:
	var last_point = start_point
	for i in range(resolution):
		var x = remap(i * start_point.x, start_point.x, end_point.x, 0, 1)
		var y = remap(i * start_point.y, start_point.y, end_point.y, 0, 1)
		var i_x = ease(x, start_inter)
		var i_y = ease(y, start_inter)
		var point = Vector2(
			remap(i_x, 0, 1, start_point.x, end_point.x),
			remap(i_y, 0, 1, start_point.y, end_point.y)
		)
		draw_line(last_point, point, Color.RED, -4.0)
		last_point = point
	
	pass
