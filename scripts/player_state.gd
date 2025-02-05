class_name PlayerState
extends State

@onready var player: Player = get_tree().get_first_node_in_group('Player')

func process(delta: float):
    if not player.is_on_floor():
        player.velocity.y += player.gravity * delta
