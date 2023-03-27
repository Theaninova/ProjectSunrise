extends Node3D

class_name ArcarDX

var gravity = 9.81

@export_range(0, 2000, 1, "or_greater", "suffix:kg") var weight: float = 1300.0
@export_range(0, 20, 0.1, "or_greater", "suffix:m") var turn_circle: float = 10.2

@export_group("Tires")
@export_range(0, 1, 0.0001, "or_greater") var rolling_friction: float = 0.0062
@export_range(0, 100, 0.01, "or_greater", "suffix:cm") var tire_radius: float = 19.05

@export_group("Engine")
@export_range(0, 20_000, 1, "or_greater", "suffix:RPM") var rev_limit: float = 6000.0
@export_range(0, 20_000, 1, "or_greater", "suffix:RPM") var idle_rev: float = 1000.0

@export_range(0, 1000, 1, "or_greater", "suffix:HP") var power: float = 600.0
## Power delivery in kW from 0 to the rev limit
@export var power_curve: Curve

@export_group("Gearbox", "gear")
## Lubricated friction coefficient for the gearbox
@export_range(0, 1, 0.0001) var gear_friction: float = 0.029
@export_range(0, 10, 0.1, "or_greater", "suffix:kg") var gear_rotating_weight: float = 5
@export_range(0, 10, 0.1, "or_greater", "suffix:cm") var gear_radius: float = 4
@export_range(-5, 5, 0.01, "or_greater", "or_less", "suffix:: 1") var gear_reverse: float = -3.38
@export_range(-5, 5, 0.01, "or_greater", "or_less", "suffix:: 1") var gear_1: float = 2.97
@export_range(-5, 5, 0.01, "or_greater", "or_less", "suffix:: 1") var gear_2: float = 2.07
@export_range(-5, 5, 0.01, "or_greater", "or_less", "suffix:: 1") var gear_3: float = 1.43
@export_range(-5, 5, 0.01, "or_greater", "or_less", "suffix:: 1") var gear_4: float = 1.0
@export_range(-5, 5, 0.01, "or_greater", "or_less", "suffix:: 1") var gear_5: float = 0.84
@export_range(-5, 5, 0.01, "or_greater", "or_less", "suffix:: 1") var gear_6: float = 0.56

# TODO
var tire_distance: float = 2


var speed_ms: float = 0.0
var revs_s: float = 1.0
var power_efficiency: float = 0.0

func _physics_process(delta: float) -> void:
	var steer: float = Input.get_action_strength("turn_right") - Input.get_action_strength("turn_left")
	var throttle: float = Input.get_action_strength("accelerate")
	var brake: float = Input.get_action_strength("brake")
	var gear: float = 0 if Input.is_action_pressed("gear_reverse") \
		else 1 if Input.is_action_pressed("gear_1") \
		else 2 if Input.is_action_pressed("gear_2") \
		else 3 if Input.is_action_pressed("gear_3") \
		else 4 if Input.is_action_pressed("gear_4") \
		else 5 if Input.is_action_pressed("gear_5") \
		else 6 if Input.is_action_pressed("gear_6") \
		else 7
	
	var max_steering_angle: float = asin(2.0 * tire_distance / turn_circle)
	var tire_radius_meters: float = tire_radius / 100.0
	var gear_radius_meters: float = gear_radius / 100.0
	
	var idle_revs_s: float = idle_rev / 60.0
	var rev_limit_s: float = rev_limit / 60.0
	
	var normal_force_newtons: float = weight * gravity
	var rolling_resistance_force: float = normal_force_newtons * rolling_friction
	
	var gear_ratios: Array[float] = [
		gear_reverse,
		gear_1,
		gear_2,
		gear_3,
		gear_4,
		gear_5,
		gear_6,
		tire_radius_meters + weight
	]
	var gear_ratio = gear_ratios[gear]
	
	power_efficiency = power_curve.sample_baked(clampf((revs_s - idle_revs_s) / rev_limit_s, 0, 1))
	var adjusted_throttle: float = throttle * int(revs_s <= rev_limit_s) + int(revs_s < idle_revs_s)
	var power_watts: float = power_efficiency * power * 745.7 * adjusted_throttle
	var torque_joules: float = power_watts / (revs_s * delta)
	
	var perpendicular_force_newtons: float = absf(gear_ratio) * torque_joules / (tire_radius_meters + gear_radius_meters)
	var acceleration: float = perpendicular_force_newtons / weight
	revs_s += acceleration * delta
	revs_s -= revs_s * gear_friction * gear_rotating_weight * delta
	if gear != 7:
		revs_s -= revs_s * rolling_resistance_force * rolling_friction * delta
		speed_ms = revs_s * tire_radius_meters / gear_ratio
	else:
		speed_ms -= speed_ms * rolling_resistance_force * rolling_friction * delta
	
	translate(Vector3.FORWARD * speed_ms * delta)
