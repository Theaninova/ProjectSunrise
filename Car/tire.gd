@icon("res://VehicleWheel3D.svg")
extends Node3D

class_name Tire

@export var mesh: Mesh
@export var friction: float = 10
@export var grip: float = 10

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
