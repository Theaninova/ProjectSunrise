@icon("res://addons/skybox_3d/Skybox3D.svg")
@tool
extends SubViewport

class_name Skybox3D

var renderer: ColorRect
var panorama_scene: Node3D

func _ready() -> void:
	own_world_3d = true
	
	var scene = preload("res://addons/skybox_3d/panorama_cam.tscn")
	panorama_scene = scene.instantiate()
	add_child(panorama_scene)
	renderer = panorama_scene.find_child("renderer")
	
	_on_size_changed()
	size_changed.connect(_on_size_changed)

func _on_size_changed() -> void:
	renderer.size = size
	var viewports = panorama_scene.find_children("*", "SubViewport")
	var viewport_size = size / Vector2i(4, 2)
	for viewport in viewports:
		viewport.size = viewport_size

func _process(delta: float) -> void:
	pass
