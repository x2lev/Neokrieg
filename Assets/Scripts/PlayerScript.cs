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

    private Vector2 dpad = Vector2.zero;
    private List<bool> buttons = new() { false, false, false, false, false };
    private bool crouching = false;
    private bool blocking = false;
    private int blockStun = 0;
    private int hitStun = 0;
    private bool attacking = false;
    private int recovery = 0;
    private PushboxScript pushbox;
    private float frame;

    private const float pixel = 1/12f;
    
    private void Start()
    {
        frame = Time.frameCount;
        pushbox = GetComponentInChildren<PushboxScript>();
    }
    public void Flip()
    {
        Quaternion q = transform.rotation;
        q[1] = q[1]==0 ? 180 : 0;
        transform.rotation = q;
        direction *= -1;
    }
    public void OnDpad(InputAction.CallbackContext context)
    {
        dpad = context.ReadValue<Vector2>();
        dpad = new Vector2(Math.Sign(dpad.x), Math.Sign(dpad.y));
    }
    public void On1(InputAction.CallbackContext context)
    {
        buttons[1]= true;
    }
    public void On2(InputAction.CallbackContext context)
    {
        buttons[2] = true;
    }
    public void On3(InputAction.CallbackContext context)
    {
        buttons[3] = true;
    }
    public void On4(InputAction.CallbackContext context)
    {
        buttons[4] = true;
    }
    public void OnOptions(InputAction.CallbackContext context)
    {
        buttons[0] = true;
    }

    public void FixedUpdate()
    {
        AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);

        velocity.y -= gravity * Time.deltaTime;

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
        
        if (transform.position.y + velocity.y * Time.deltaTime < -31/6f && !grounded)
        {
            pushbox.ClearColliders();
            pushbox.AddCollider("idle", new Vector2(0, 18 * pixel), new Vector2(14 * pixel, 32 * pixel));
        }

        _animator.SetFloat("x_velocity", velocity.x * direction);
        _animator.SetFloat("y_velocity", velocity.y);
        _animator.SetBool("crouching", crouching);
        _animator.SetBool("grounded", grounded);

        transform.position += velocity * Time.deltaTime;

        buttons = new() { false, false, false, false, false };
    }
    public void Jumpbox(PushboxScript pb)
    {
        frame = Time.frameCount;
        pb.ClearColliders();
        pb.AddCollider("jumpbox", new Vector2(0, 26*pixel), new Vector2(14*pixel, 14*pixel));
    }
}
