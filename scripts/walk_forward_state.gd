class_name WalkForwardState
extends GroundState

func enter():
    player.animation.play('walk')
    if not player.sprite.flip_h:
        player.velocity.x = player.FORWARD_SPEED
    else:
        player.velocity.x = -player.FORWARD_SPEED
