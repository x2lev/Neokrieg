class_name RunBackState
extends RunState

func enter():
    player.animation.play_backwards('run')
