extends Node3D

var gravity: float = 10

var min_drift_speed = 50
var acceleration: float = 10
var braking_speed: float = 1
var turn: float = PI / 100

var friction: float = 0.1

var direction: float = 0
var speed_rate: float = 0
var is_drifting: bool = false
var drift_rate: float = 0

var enable3dDriving: bool = false

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


func _physics_process(delta):
	$Control/accel.value = Input.get_action_strength("accelerate")
	$Control/brake.value = Input.get_action_strength("brake")
	$Control/left.value = Input.get_action_strength("turn_left")
	$Control/right.value = Input.get_action_strength("turn_right")
	
	var accel: float = acceleration * Input.get_action_strength("accelerate")
	accel -= braking_speed * speed_rate * Input.get_action_strength("brake")
	accel -= friction * speed_rate
	$Control/accel2.text = "%d m/s²" % accel
	
	speed_rate += accel * delta

	var angular_vel: float = turn * speed_rate * Input.get_action_strength("turn_left")
	angular_vel -= turn * speed_rate * Input.get_action_strength("turn_right")
	var drifting_vel: float = lerp(angular_vel, drift_rate, clamp(abs(drift_rate), 0, 1))
	$Control/angular_vel.text = "%d°/s²" % rad_to_deg(angular_vel)

	var drift_speed = clamp(atan(speed_rate - min_drift_speed), 0, 1)
	var speed_loss = clamp(remap(abs(drift_rate), PI / 4, PI / 2, 0, 1), 0, 1)
	var drift_blend: float = drift_speed * clamp(remap(abs(drifting_vel), PI / 4, PI, 0, 1), 0, 1)
	$Control/drift_amount.value = drift_blend
	$Control/drift_speed_loss.value = speed_loss

	drift_rate += lerp(0.0, angular_vel * delta, drift_blend)
	direction += lerp(angular_vel * delta, 0.0, drift_blend)
	
	#direction += lerp(drift_rate * delta, 0.0, drift_speed)
	drift_rate -= lerp(drift_rate * delta, 0.0, drift_speed)
	
	direction = drifting_vel * delta
	
	speed_rate -= lerp(0.0, speed_rate * delta, speed_loss)

	$Control/speed.text = "%d km/h" % (speed_rate * 3.6)
	$Control/turn.text = "%d°/s" % rad_to_deg(direction)

	translate(Vector3.FORWARD * speed_rate * delta)
	rotate_y(direction)
	
	$VisualCar.rotation = lerp(
		$VisualCar.rotation,
		Vector3(atan(accel / 400), drift_rate, atan(angular_vel / -15)),
		delta / 0.1)

	if enable3dDriving:
		if $RayCast3D.is_colliding():
			self.position.y = $RayCast3D.get_collision_point().y + 0.2

			var normal = $RayCast3D.get_collision_normal()
			var node = $CSGBox3D
			var basis = Basis()
			basis.x = normal.cross(node.basis.z)
			basis.y = normal
			basis.z = node.basis.x.cross(normal)
			node.basis = basis.orthonormalized()
			#self.quaternion = Quaternion($RayCast3D.get_collision_normal(), direction)
		else:
			self.position.y -= gravity * delta
	#look_at($RayCast3D.get_collision_normal())

	#$RayCast3D
	pass
