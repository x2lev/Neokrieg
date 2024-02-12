using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public Animator _animator;

    [Header("Physics")]
    [SerializeField] float gravity = 1f;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 7.5f;
    [SerializeField] float jumpForce = 5f;

    [HideInInspector] public bool player1 = false;
    [HideInInspector] public int direction = 1;
    [HideInInspector] public int backToWall = 0;
    [HideInInspector] public bool grounded = true;
    [HideInInspector] public Vector3 velocity = Vector3.zero;

    [HideInInspector] public HurtboxScript hurtb;
    [HideInInspector] public HitboxScript hitb;
    [HideInInspector] public PushboxScript pushb;
    [HideInInspector] public Move jump;
    [HideInInspector] public Move land;
    [HideInInspector] public Move attack;
    [HideInInspector] public Move activeMove;

    private Vector2 dpad = Vector2.zero;
    private List<int> buttons = new();
    private bool crouching = false;
    private bool blocking = false;
    private bool attacking = false;

    [HideInInspector] public float pixel = 1 / 12f;

    public void Start()
    {
        jump = new(this, new() { 
            new(1, new() { }, new() { }, new() { new("jump", new(0, 27 * pixel), new(14 * pixel, 14 * pixel)) }) 
        });
        land = new(this, new() { 
            new(1, new() { }, new() { }, new() { new("land", new(0, 18 * pixel), new(14 * pixel, 32 * pixel)) })
        });
        attack = new(this, new() { 
            new(6, new() { }, new() { new("attack0", new(12 * pixel, 30 * pixel), new(14 * pixel, 2 * pixel)) }, new() { }),
            new(6, new() { }, new() { new("attack1", new(12 * pixel, 28 * pixel), new(14 * pixel, 2 * pixel)) }, new() { }),
            new(6, new() { }, new() { new("attack2", new(12 * pixel, 26 * pixel), new(14 * pixel, 2 * pixel)) }, new() { }),
            new(6, new() { }, new() { new("attack3", new(12 * pixel, 24 * pixel), new(14 * pixel, 2 * pixel)) }, new() { }),
            new(6, new() { }, new() { new("attack4", new(12 * pixel, 22 * pixel), new(14 * pixel, 2 * pixel)) }, new() { }),
            new(6, new() { }, new() { new("attack5", new(12 * pixel, 20 * pixel), new(14 * pixel, 2 * pixel)) }, new() { }),
            new(6, new() { }, new() { new("attack6", new(12 * pixel, 18 * pixel), new(14 * pixel, 2 * pixel)) }, new() { }),
            new(6, new() { }, new() { new("attack7", new(12 * pixel, 16 * pixel), new(14 * pixel, 2 * pixel)) }, new() { }),
            new(6, new() { }, new() { new("attack8", new(12 * pixel, 14 * pixel), new(14 * pixel, 2 * pixel)) }, new() { }),
            new(6, new() { }, new() { new("attack9", new(12 * pixel, 12 * pixel), new(14 * pixel, 2 * pixel)) }, new() { }),
            new(1, new() { }, new() { new("clear", Vector2.zero, Vector2.zero) }, new() { })
        });
        Debug.Log(string.Join(", ", attack.phases[0].hitboxes));
    }
    public void Flip()
    {
        Quaternion q = transform.rotation;
        q[1] = q[1] == 0 ? 180 : 0;
        transform.rotation = q;
        direction *= -1;
    }
    public void OnDpad(InputAction.CallbackContext context)
    {
        dpad = context.ReadValue<Vector2>();
        dpad = new Vector2(Math.Sign(dpad.x), Math.Sign(dpad.y));
    }
    public void OnOptions() { buttons.Add(0); }
    public void On1() { buttons.Add(1); Debug.Log(0); }
    public void On2() { buttons.Add(2); }
    public void On3() { buttons.Add(3); }
    public void On4() { buttons.Add(4); }

    public void FixedUpdate()
    {
        activeMove?.Advance();

        buttons.Sort();

        if (buttons.Contains(1) && activeMove != attack)
        {
            Debug.Log(1);
            attack.Start();
            attacking = true;
        }

        AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);


        velocity.y -= velocity.y < 0 ? 1.25f : 1 * gravity * Time.deltaTime;

        if (grounded)
        {
            velocity.x = dpad.x * walkSpeed;
            if (dpad.y > 0)
                velocity.y = jumpForce;
        }

        blocking = dpad.x * direction < 0 && !attacking;
        crouching = grounded && dpad.y < 0;

        if (crouching || state.IsName("crouch"))
            velocity.x = 0;

        _animator.SetFloat("x_velocity", velocity.x * direction);
        _animator.SetFloat("y_velocity", velocity.y);
        _animator.SetBool("crouching", crouching);
        _animator.SetBool("grounded", grounded);

        transform.position += velocity * Time.deltaTime;

        buttons.Clear();
    }
}
