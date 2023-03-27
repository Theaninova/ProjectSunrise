@icon("icons/mechanism.png")

extends ArcadeCarPart

class_name ArcadeCarEngine

@export_range(0, 12_000, 100, "or_greater", "suffix:RPM") var rev_limit = 6000
@export_range(0, 12_000, 100, "or_greater", "suffix:RPM") var idle_rpm = 1000
@export_range(0, 12_000, 100, "or_greater", "suffix:RPM") var optimal_power = 5000
@export_range(0, 1000, 10, "or_greater", "suffix:HP") var power = 500
@export_exp_easing var power_curve = 0.2

@onready var power_jule_s = power * 745.7

var current_rpm = 0
var current_power = 0

func _physics_process(delta: float) -> void:
	pass
