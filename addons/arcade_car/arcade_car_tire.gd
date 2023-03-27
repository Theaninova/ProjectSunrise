extends Resource

## Represents a rubber tire
class_name ArcadeCarTire

## Pressure of the tire, assumed to be filled
## at environmental temperature
@export_range(0, 1000, 1, "or_greater", "suffix:kPa")
var pressure: float = 200

@export_range(0, 100, 1, "or_greater", "suffix:cm")
var radius: float = 15

@export_range(0, 100, 1, "or_greater", "suffix:cm")
var width: float = 10

## Kinetic or sliding friction that occurs when the
## static friction has been overcome
@export_range(0, 100, 0.1, "suffix:%")
var drift_traction: float = 70

## The amount of lubricant that can pass through the
## tire. More flow means less lubricant that forms a film
## between the wheel and the ground.
@export_range(0, 100, 0.1, "suffix:%")
var fluid_flow: float = 2

## Penetration of soft surfaces
@export_range(0, 100, 1)
var penetration: float = 1

@export
var profile: Image

## Dynamics of the tire
class Dynamics extends Node3D:
	var tire: ArcadeCarTire
	var reality: ArcadeCarReality
	
	var absolute_pressure: float
	var temperature: float
	
	var normal_force: float
	
	func _init(tire: ArcadeCarTire, reality: ArcadeCarReality):
		self.tire = tire
		self.reality = reality
		absolute_pressure = tire.pressure + reality.atmosphere_pressure
		temperature = reality.temperature
	
	func _physics_process(delta: float) -> void:
		var pressure = (temperature / reality.temperature) * absolute_pressure
		var contact_area = normal_force / pressure

## The amount the tire can roll along a surface depending
## on the drift angle
func rolling_contribution(drift_angle: float) -> float:
	return cos(drift_angle)

func dynamic_pressure(environment_pressure: float, temperature: float) -> float:
	return (pressure + environment_pressure)

## The contact area of the tire given a normal force on it where
## F is the magnitute of the normal force in Newton
## p is the pressure in Pascal where 1 kPa = 1000 N/m²
## A is the area of surface contact in m²
##
## A = F / p
func contact_area(normal_force: float) -> float:
	return normal_force / pressure
	


