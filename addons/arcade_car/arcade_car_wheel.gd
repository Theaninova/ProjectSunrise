@icon("icons/car_wheel.png")
extends ArcadeCarPart

class_name ArcadeCarWheel

## Natural friction of the tire when driving straight.
## Measured as deceleration in m/s²
@export_range(0, 0, 0.1, "or_greater", "hide_slider", "suffix:m/s²") var natural_friction = 1.3

## Whether the wheel is powered, aka transfers power
## into speed.
@export var powered: bool = false

## Whether the wheel is steered, aka turns when steering
## input is received.
@export var steering: bool  = false

@export_group("Suspension", "suspension")
@export_range(0, 1, 0.01, "or_greater", "hide_slider", "suffix:m") var suspension_length: float = 1
@export_exp_easing("inout") var suspension_strength: float = 0.2

@export_group("Drifting", "drift")

@export_range(0, 90, 0, "radians") var drift_min_angle: float = PI / 16

## The amount power that gets transferred
## depending on drift angle [0°..90°]
@export_exp_easing("attenuation") var drift_power_transfer: float = 0.2

## The amount of friction that the tire produces
## depending on the drift angle [0°..90°]
@export_exp_easing var drift_friction_transfer: float = 4.5

## Friction when the car drifts at a 90° angle.
## Measured as deceleration in m/s²
@export_range(0, 0, 0.01, "hide_slider", "or_greater", "suffix:m/s²") var drift_friction = 40

var distance_to_surface: float = 0
var tire_velocity: float = 0

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta: float) -> void:
	pass
