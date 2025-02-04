extends CharacterBody2D

@export var SPEED = 300.0
@export var JUMP_VELOCITY = -350.0
@onready var sprite = $AnimatedSprite2D
var gravity = ProjectSettings.get_setting('physics/2d/default_gravity')
var input_buffer = [
	{
		'dpad': Vector2.ZERO,
		'buttons': {
			'square': false, 'triangle': false,
			'cross': false,'circle': false,
			'r1': false, 'l1': false,
			'r2': false, 'l2': false,
			'r3': false, 'l3': false
		},
		'frames': 0
	}
]

func _physics_process(delta):
	update_input_buffer()
	
	var input_string = str(input_buffer[0]['dpad'])
	for button in input_buffer[0]['buttons']:
		if input_buffer[0]['buttons'][button]:
			input_string += ' ' + button
	print(input_string, ' ', input_buffer[0]['frames'])

	if not is_on_floor():
		velocity.y += gravity * delta * (1.0 if velocity.y < 0 else 1.5)

	if Input.is_action_just_pressed('up') and is_on_floor():
		velocity.y = JUMP_VELOCITY

	var direction = Input.get_axis('left', 'right')
	if direction:
		velocity.x = direction * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)

	if is_on_floor():
		sprite.flip_h = position.x > 0

	move_and_slide()

func update_input_buffer():
	var dpad = Input.get_vector('left', 'right', 'up', 'down')
	var buttons = {
		'square': Input.is_action_pressed('square'),
		'triangle': Input.is_action_pressed('triangle'),
		'cross': Input.is_action_pressed('cross'),
		'circle': Input.is_action_pressed('circle'),
		'r1': Input.is_action_pressed('r1'),
		'l1': Input.is_action_pressed('l1'),
		'r2': Input.is_action_pressed('r2'),
		'l2': Input.is_action_pressed('l2'),
		'r3': Input.is_action_pressed('r3'),
		'l3': Input.is_action_pressed('l3')
	}
	
	if input_buffer[0]['dpad'] == dpad and input_buffer[0]['buttons'] == buttons:
		input_buffer[0]['frames'] = min(999, input_buffer[0]['frames'] + 1)
	else:
		input_buffer.insert(0, {'dpad': dpad, 'buttons': buttons, 'frames': 1})
		if len(input_buffer) > 10:
			input_buffer.pop_back()
