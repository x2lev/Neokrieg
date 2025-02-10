class_name GroundState
extends PlayerState

func process(_delta: float):
    player.sprite.flip_h = player.position.x > player.opponent.position.x

func handle_input(input_buffer: Array[Dictionary]):
    print('yellow')
    var dpad = input_buffer[0]['dpad']
    var buttons = input_buffer[0]['buttons']
    if buttons.values().all(func(x): return not x):
        if dpad.y > 0:
            change_state.emit('CrouchState')
        elif dpad.y < 0:
            change_state.emit('JumpState')
        else:
            if dpad.x > 0:
                change_state.emit('WalkForwardState')
            elif dpad.x < 0:
                change_state.emit('WalkBackwardState')
            else:
                change_state.emit('IdleState')
    else: # attacking
        pass
