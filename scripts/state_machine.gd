class_name StateMachine
extends Node2D

@export var starting_state: State
var current_state: State
var states: Dictionary = {}

func init():
    for child in get_children():
        print('child: ', child.name)
        if child is State:
            print('is a state!')
            states[child.name] = child
            child.change_state.connect(on_change_state)
    on_change_state(starting_state.name)

func process(delta: float):
    current_state.process(delta)

func physics_process(delta: float):
    current_state.physics_process(delta)

func handle_input(input_buffer: Array[Dictionary]):
    current_state.handle_input(input_buffer)

func on_change_state(new_state_name: String):
    print('new state: ', new_state_name)
    if current_state:
        current_state.exit()
    current_state = states[new_state_name]
    current_state.enter()
