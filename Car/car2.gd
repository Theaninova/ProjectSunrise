@icon("res://VehicleBody3D.svg")
extends Node3D

class_name Car

@export_group("Base Stats")
@export var max_speed: float = 300
@export var acceleration: float = 10
@export var turn_radius: float = 10
@export var weight: float = 500
@export var rpm: float = 6000

@export_group("Gears")
@export_subgroup("Reverse", "gear_rev")
@export_exp_easing("inout") var gear_rev_power_curve = 0.2

@export_group("Drift Tuning", "drift")
@export var drift_min_speed: float = 60
@export_exp_easing("attenuation") var drift_speed_loss: float = 0.35


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta: float) -> void:
	pass
