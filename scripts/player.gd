extends CharacterBody2D

@export var SPEED = 300.0
@export var JUMP_VELOCITY = -350.0
@export var AIR_CONTROL = .10
@onready var sprite = $AnimatedSprite2D
var gravity = ProjectSettings.get_setting('physics/2d/default_gravity')
var buffer = [
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
var crouching = false
var blocking = false
var attack = ''

func _ready():
	sprite.animation_finished.connect(_animation_finished)

func _process(_delta):
	sprite.play()
	if attack:
		sprite.animation = attack
	elif is_on_floor():
		sprite.flip_h = position.x > 0
		if crouching:
			sprite.animation = 'crouch'
		elif buffer[0]['dpad'].x == 0:
			sprite.animation = 'idle'
		elif buffer[0]['dpad'].x < 0 == sprite.flip_h:
			sprite.animation = 'run'
		elif buffer[0]['dpad'].x > 0 == sprite.flip_h:
			sprite.animation = 'run'
			sprite.play_backwards()
	else:
		if velocity.y > 0:
			sprite.animation = 'fall'
		else:
			sprite.animation = 'jump'

func _physics_process(delta):
	update_buffer(true)

	crouching = buffer[0]['dpad'].y > 0 and is_on_floor() and not attack
	blocking = velocity.x > 0 == sprite.flip_h and not attack

	if not is_on_floor():
		velocity.y += gravity * delta * (1.0 if velocity.y < 0 else 1.5)
	
	if not attack:
		check_attacks()

		if is_on_floor():
			if buffer[0]['dpad'].y < 0 and buffer[0]['frames'] < 4:
				velocity.y = JUMP_VELOCITY
			
			if not crouching:
				if buffer[0]['dpad'].x:
					velocity.x = buffer[0]['dpad'].x * SPEED
				else:
					velocity.x = move_toward(velocity.x, 0, SPEED)
		else:
			if buffer[0]['dpad'].x:
				velocity.x = clamp(velocity.x + buffer[0]['dpad'].x * SPEED * AIR_CONTROL, -SPEED, SPEED)
	
	if is_on_floor() and attack:
		velocity.x = 0

	move_and_slide()

func check_attacks():
	if is_on_floor():
		if buffer[0]['buttons']['square']:
			attack = 'attack1'
		elif buffer[0]['buttons']['triangle']:
			attack = 'attack2'
		elif buffer[0]['buttons']['r1']:
			attack = 'attack3'
	else:
		if buffer[0]['buttons']['square']:
			attack = 'air_attack'

func update_buffer(print_buffer):
	var dpad = Input.get_vector('left', 'right', 'up', 'down').sign()
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
	
	if buffer[0]['dpad'] == dpad and buffer[0]['buttons'] == buttons:
		buffer[0]['frames'] = min(999, buffer[0]['frames'] + 1)
	else:
		buffer.insert(0, {'dpad': dpad, 'buttons': buttons, 'frames': 1})
		if len(buffer) > 10:
			buffer.pop_back()

	if print_buffer:
		var input_string = str(buffer[0]['dpad'])
		for button in buffer[0]['buttons']:
			if buffer[0]['buttons'][button]:
				input_string += ' ' + button
		print(input_string, ' ', buffer[0]['frames'])

func _animation_finished():
	attack = ''
