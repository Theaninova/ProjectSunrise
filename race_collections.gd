extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

# FUN_180052b00

func unknown_1(x):
	if x >= 0:
		if x > 1:
			x = 1
		else:
			x = sin(x * (PI / 2))
			if x < 0:
				x = 0
	else:
		x = 0
