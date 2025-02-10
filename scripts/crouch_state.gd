class_name CrouchState
extends GroundState

func enter():
    player.velocity.x = 0
    player.animation.play('crouch')
