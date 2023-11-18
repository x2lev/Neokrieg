using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D _rigidbody;
    public Animator _animator;
    public Collider2D hurtbox;
    public float walkSpeed = 5f;
    public float runSpeed = 7.5f;
    public float backDash = 7.5f;
    public float jumpForce = 5f;
    
    [HideInInspector] public Vector2 dpad = Vector2.zero;
    [HideInInspector] public int direction = 1;
    [HideInInspector] public List<bool> buttons = new() { false, false, false, false, false };
    [HideInInspector] public bool grounded = true;
    [HideInInspector] public bool crouching = false;
    [HideInInspector] public bool blocking = false;
    [HideInInspector] public bool attacking = false;
    public void flip()
    {
        Quaternion q = transform.rotation;
        q[1] = q[1]==0 ? 180 : 0;
        transform.rotation = q;
        direction *= -1;
    }
    public void OnDpad(InputAction.CallbackContext context)
    {
        dpad = context.ReadValue<Vector2>().normalized;
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

    public virtual void LateFixedUpdate() { }
    public void FixedUpdate()
    {
        /*
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(0, _collider.bounds.center.y-_collider.bounds.size.y/2+.1f), Vector2.down, 1f);
        Debug.Log(_collider.bounds.size.y);
        if (hit)
            Debug.Log(hit.transform.gameObject + " " + (_collider.bounds.center.y- _collider.bounds.size.y/2+.1f));
        else
            Debug.Log("no hit " + (_collider.bounds.center.y - _collider.bounds.size.y / 2 + .1f));
        if (hit && hit.transform.gameObject.name == "Stage")
            grounded = true;
        */

        float x = _rigidbody.velocity.x;
        float y = _rigidbody.velocity.y;
        if (y == 0)
        {
            x = dpad.x * walkSpeed;
            y = (dpad.y > 0) ? jumpForce : 0;
        }
        blocking = dpad.x * direction < 0 && !attacking;
        crouching = grounded && dpad.y < 0;
        Debug.Log("Dpad: " + dpad.ToString() + " Buttons: { " + string.Join(", ", buttons) + " } G: " + grounded + " C: " + crouching);

        _animator.SetFloat("x_velocity", _rigidbody.velocity.x * direction);
        _animator.SetFloat("y_velocity", _rigidbody.velocity.y);
        LateFixedUpdate();

        _rigidbody.velocity = new Vector2(x, y);
        buttons = new() { false, false, false, false, false };
    }
    public virtual void LateCollisionEnter2D(Collision2D collision) { }
    public virtual void LateCollisionExit2D(Collision2D collision) { }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Floor")
            grounded = true;
        LateCollisionEnter2D(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Floor")
            grounded = false;
        LateCollisionExit2D(collision);
    }
}
