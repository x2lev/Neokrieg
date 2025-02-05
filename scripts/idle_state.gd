class_name IdleState
extends PlayerState

func enter():
    player.animation.play('idle')

func physics_process(_delta: float):
    if player.input_buffer[0]['dpad'].x > 0 == not player.sprite.flip_h:
        change_state.emit('RunState')
    elif player.input_buffer[0]['dpad'].x < 0 == not player.sprite.flip_h:
        change_state.emit('RunBackState')
