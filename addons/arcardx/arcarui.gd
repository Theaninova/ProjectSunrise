extends GridContainer

class_name ArcarUI

@export var arcar_dx: ArcarDX

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	%ui_revs.text = "%04d RPM @ %02d%%" % [
		roundi(arcar_dx.revs_s * 60),
		roundi(arcar_dx.power_efficiency * 100)
	]
	%ui_speed.text = "%03d km/h" % roundi(arcar_dx.speed_ms * 3.6)
	pass
