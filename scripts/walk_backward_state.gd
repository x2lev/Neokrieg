class_name WalkBackwardState
extends GroundState

func enter():
    player.animation.play_backwards('walk')
    if not player.sprite.flip_h:
        player.velocity.x = -player.BACKWARD_SPEED
    else:
        player.velocity.x = player.BACKWARD_SPEED
