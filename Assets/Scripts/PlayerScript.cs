using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public Animator _animator;

    [Header("Physics")]
    public float gravity = 1f;
    public float walkSpeed = 5f;
    public float runSpeed = 7.5f;
    public float jumpForce = 5f;
    
    [HideInInspector] public Vector2 dpad = Vector2.zero;
    [HideInInspector] public Vector3 velocity = Vector3.zero;
    [HideInInspector] public List<bool> buttons = new() { false, false, false, false, false };
    [HideInInspector] public int direction = 1;
    [HideInInspector] public bool grounded = true;
    [HideInInspector] public bool crouching = false;
    [HideInInspector] public bool blocking = false;
    [HideInInspector] public int blockStun = 0;
    [HideInInspector] public int hitStun = 0;
    [HideInInspector] public bool attacking = false;
    [HideInInspector] public int recovery = 0;
    [HideInInspector] public bool player1 = false;

    private float frame;
    private const float pixel = 1/12f;
    private void Awake()
    {
        frame = Time.frameCount;
    }
    public void flip()
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
        float x = velocity.x;
        float y = velocity.y - gravity * Time.deltaTime;

        if (grounded)
        {
            x = dpad.x * walkSpeed;
            if (dpad.y > 0)
                y = jumpForce;
        }

        blocking = dpad.x * direction < 0 && !attacking;
        crouching = grounded && dpad.y < 0;

        if (crouching || state.IsName("crouch"))
            x = 0;

        if (player1)
        {
            // Debug.Log("Dpad: " + dpad.ToString() + " Buttons: { " + string.Join(", ", buttons) + " } G: " + grounded + " C: " + crouching + " xy: " + new Vector2(x, y));
        }
        
        if (Time.frameCount - frame > 48)
        {
            BoxCollider2D pushbox = transform.Find("Pushbox").GetComponent<BoxCollider2D>();
            pushbox.offset = new Vector2(0, 1.541667f);
            pushbox.size = new Vector2(1.166666f, 2.75f);
        }

        _animator.SetFloat("x_velocity", x * direction);
        _animator.SetFloat("y_velocity", y);
        _animator.SetBool("crouching", crouching);
        _animator.SetBool("grounded", grounded);

        velocity = new Vector2(x, y);

        transform.position += velocity * Time.deltaTime;

        buttons = new() { false, false, false, false, false };
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            frame = Time.frameCount;
            BoxCollider2D pushbox = collision.otherCollider as BoxCollider2D;
            pushbox.offset = new Vector2(0, 2.333333f);
            pushbox.size = new Vector2(1.166667f, 1.166667f);
            grounded = false;
        }
    }
}
