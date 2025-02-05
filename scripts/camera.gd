extends Camera2D

@export var player: Node2D

func _ready():
	pass

func _process(_delta: float):
	position.x = clamp(player.position.x/2, -144, 144)
