extends Camera2D

@export var player: Node2D

func _ready():
	pass

func _process(_delta):
	position.x = clamp(player.position.x/2, -144, 144)
