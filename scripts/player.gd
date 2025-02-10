class_name Player
extends CharacterBody2D

@onready var state_machine: StateMachine = $StateMachine
@onready var animation: AnimationPlayer = $AnimationPlayer
@onready var sprite: Sprite2D = $Sprite2D

@export var opponent: Node2D

@export var FORWARD_SPEED: float
@export var BACKWARD_SPEED: float
@export var JUMP_SPEED: float

var input_buffer: Array[Dictionary] = [ 
	{
		'dpad': Vector2.ZERO,
		'buttons': {
			'punch': false,
			'special': false,
			'kick': false,
			'heavy': false
		},
		'frames': 0
	}
]
var gravity: float = ProjectSettings.get_setting('physics/2d/default_gravity')

func _ready():
	state_machine.init()

func _process(delta: float):
	state_machine.process(delta)
	move_and_slide()

func _physics_process(delta: float):
	state_machine.physics_process(delta)
	input()

func input():
	var dpad = Input.get_vector('left', 'right', 'up', 'down').sign()
	if sprite.flip_h:
		dpad.x *= -1
	var buttons = {
		'punch': Input.is_action_pressed('punch'),
		'special': Input.is_action_pressed('special'),
		'kick': Input.is_action_pressed('kick'),
		'heavy': Input.is_action_pressed('heavy')
	}
	if input_buffer[0]['dpad'] == dpad and input_buffer[0]['buttons'] == buttons:
		input_buffer[0]['frames'] += 1
	else:
		input_buffer.insert(0, {'dpad': dpad, 'buttons': buttons, 'frames': 1})
		if len(input_buffer) > 10:
			input_buffer.pop_back()
		state_machine.handle_input(input_buffer)
