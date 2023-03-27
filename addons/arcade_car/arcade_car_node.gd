@tool
@icon("icons/chassis.png")
extends Node3D

class_name ArcadeCar

@export_group("General")

@export var reality: ArcadeCarReality

@export var tires: ArcadeCarTire

## The total weight of the car
@export_range(0, 0, 1, "or_greater", "hide_slider", "suffix:kg")
var weight: float = 1490

@export var drag: float = 0.2

## The turning circle of the car
@export_range(0, 0, 0.1, "or_greater", "hide_slider", "suffix:m")
var turning_circle: float  = 10.2

@export_group("Engine")
@export_range(0, 0, 100, "or_greater", "hide_slider", "suffix:RPM")
var rev_limit: float = 6000

@export_range(0, 0, 100, "or_greater", "hide_slider", "suffix:RPM")
var idle_revs: float = 1000

# @export_enum("Metric:1", "Imperial:2") var unit = 2

var power_watts: float
@export_range(0, 0, 10, "or_greater", "hide_slider", "suffix:HP") var max_power: float = 252:
	set(value):
		max_power = value
		power_watts = value * 745.7
		notify_property_list_changed()
@export var power_curve: Curve

@export_range(0, 0, 100, "or_greater", "hide_slider", "suffix:ft/lb")
var max_torque: float = 216
@export var torque_curve: Curve

@export_group("Gearbox", "gear")
@export var gear_ratios: Array[float] = [-0.5, 0, 0.5, 1, 1.4]

@export_group("Tires", "tire")

## Friction coefficient of the tires when the wheels
## can't rotate freely
@export var tire_grip: float = 0.2

## Rolling Resistance Coefficient of the tires C_rr
##
## Where deceleration force F = C_rr * N where N is the normal force
@export var tire_resitance: float = 0.0025

var tire_circumference_m: float
var tire_radius_m: float
## The radius of the tires
@export_range(0, 0, 0.1, "or_greater", "hide_slider", "suffix:cm") var tire_radius: float = 15:
	set(value):
		tire_radius = value
		tire_radius_m = tire_radius / 100
		tire_circumference_m = tire_radius_m * 2 * PI
		notify_property_list_changed()

@export var tire_profile: Image

@export_range(0, 0, 10, "or_greater", "hide_slider", "suffix:PSI")
var tire_pressure: float = 200

@export_group("Drifting", "drift")

## Minimum speed required for drifting
@export_range(0, 0, 1, "or_greater", "hide_slider", "suffix:km/h")
var drift_min_speed: float = 120

## Maximum speed where the minimum drift angle is reached
@export_range(0, 0, 1, "or_greater", "hide_slider", "suffix:km/h")
var drift_max_speed: float = 300

## Angle at which the car starts drifting
@export_range(0, 90, 1, "radians")
var drift_angle: float = PI / 16

## Drift angle vs speed with the reference points on the
## x-axis being min..max speed and on the y-axis 0..angle
@export var drift_angle_curve: Curve

## Friction of the tires at a full 90° drift angle
@export_range(0, 20, 1, "or_greater", "hide_slider", "suffix:m/s²")
var drift_friction

@export var drift_friction_curve: Curve

func _physics_process(delta: float) -> void:
	pass

## Calculates the current speed based on the revs/min in m/s
func calc_speed(rev: float) -> float:
	var rev_s = rev / 60
	return rev_s * tire_circumference_m

func calc_torque_force(rev: float) -> float:
	var torque = torque_curve.sample(rev / rev_limit) * max_torque
	return torque / (tire_radius * sin(PI / 2))

## Calculates the rev acceleration in rev/s²
func calc_rev_accel(rev: float):
	pass
