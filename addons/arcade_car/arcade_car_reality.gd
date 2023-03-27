extends Resource

class_name ArcadeCarReality

@export_range(0, 20, 0.01, "or_greater", "suffix:m/s²")
var gravity: float = 9.81

@export_range(-100, 100, 0.1, "or_greater", "or_less", "suffix:°C")
var temperature: float = 21

@export_group("Atmosphere", "atmosphere")

@export_range(0, 200, 0.1, "or_greater", "suffix:kPa")
var atmosphere_pressure: float = 101.3

@export_group("Reality Bending")

@export_exp_easing("inout")
var drift: float = -12.5

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
