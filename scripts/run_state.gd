class_name RunState
extends PlayerState

func enter():
    player.animation.play('run')

func process(delta: float):
    super(delta)
    player.velocity.x = player.input_buffer[0]['dpad'].x * player.SPEED

func physics_process(_delta: float):
    if player.velocity.x == 0:
        change_state.emit('IdleState')
