class_name JumpState
extends PlayerState

var falling: bool

func enter():
    falling = false
    player.animation.play('jump')
    player.velocity.y = -player.JUMP_SPEED

func exit():
    player.velocity.y = 0

func process(_delta: float):
    print(player.velocity.y)

    if not falling and player.velocity.y > 0:
        player.animation.play('fall')
        falling = true

    if falling and player.is_on_floor():
        change_state.emit('IdleState')

func physics_process(delta: float):
    player.velocity.y += player.gravity * delta
