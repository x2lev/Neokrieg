using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [HideInInspector] public HitboxScript hitb;
    [HideInInspector] public HurtboxScript hurtb;
    [HideInInspector] public PushboxScript pushb;

    private Vector2 dpad = Vector2.zero;
    private List<int> buttons = new();
    private bool crouching = false;
    private bool blocking = false;
    private bool attacking = false;
    private float frame;

    private const float pixel = 1 / 12f;

    private void Start()
    {
        frame = Time.frameCount;
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
    public void On1() { buttons.Add(1); }
    public void On2() { buttons.Add(2); }
    public void On3() { buttons.Add(3); }
    public void On4() { buttons.Add(4); }
    public void OnOptions() { buttons.Add(0); }

    public void FixedUpdate()
    {
        buttons.Sort();

        if (player1) { }
        // Debug.Log("Dpad=" + dpad + " Buttons=" + string.Join(" ", buttons));

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
    }
    public void IdleBox(PushboxScript pb)
    {
        pb.ClearColliders();
        pb.AddCollider("idle", new Vector2(0, 18 * pixel), new Vector2(14 * pixel, 32 * pixel));
    }
    public void Jumpbox(PushboxScript pb)
    {
        pb.ClearColliders();
        pb.AddCollider("jumpbox", new Vector2(0, 26 * pixel), new Vector2(14 * pixel, 14 * pixel));
    }
}
