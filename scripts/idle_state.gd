class_name IdleState
extends GroundState

func enter():
    player.velocity.x = 0
    player.animation.play('idle')
