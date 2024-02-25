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
            new(12, new() { }, new() {
                new("attack", new(8 * pixel, 49 * pixel), new(42 * pixel, 8 * pixel)) 
            }, new() { }),
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

        AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);

        // Gravity
        velocity.y -= velocity.y < 0 ? 1.25f : 1 * gravity * Time.deltaTime;

        if (grounded && activeMove is null)
        {
            velocity.x = dpad.x * walkSpeed;
            if (buttons.Contains(1))
            {
                velocity.x = 0;
                attack.Start();
            }
            if (dpad.y > 0)
                velocity.y = jumpForce;
        }

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
