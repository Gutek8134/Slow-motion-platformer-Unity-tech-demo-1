using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO: Change to Physics
public class SidescrollerController : DynamicObject
{
    /// <summary>
    /// Max horizontal speed of the player.
    /// </summary>
    public float maxSpeed = 7;

    /// <summary>
    /// Initial jump velocity at the start of a jump.
    /// </summary>
    public float jumpHeight = 7;
    public float jumpDeceleration = 0;

    [SerializeField]
    private Vector2 position,
        size;

    public JumpState jumpState = JumpState.Grounded;
    private bool stopJump;

    [SerializeField]
    private LayerMask groundFilter;
    private ContactFilter2D filter;

    /*internal new*/public Collider2D collider2d;

    [SerializeField]
    private Collider2D hitbox;

    bool jump;
    SpriteRenderer spriteRenderer;
    internal Animator animator;

    private MainInputActions playerInput;

    protected override void Start()
    {
        base.Start();
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        filter.SetLayerMask(groundFilter);
        filter.useTriggers = false;
        while (playerInput == null)
        {
            playerInput = InputManager.playerInput;
        }
        playerInput.Moving.Enable();
        playerInput.Moving.Jump.started += context =>
            jumpState = (jumpState == JumpState.Grounded) ? JumpState.PrepareToJump : jumpState;
        playerInput.Moving.Jump.canceled += context => stopJump = true;
        playerInput.Melee.Enable();
    }

    protected override void Update()
    {
        move.x = maxSpeed * playerInput.Moving.Move.ReadValue<float>();
        base.Update();
    }

    protected override void FixedUpdate()
    {
        IsGrounded =
            Physics2D.BoxCast(
                (Vector2)transform.position - position,
                size,
                0,
                Vector2.up,
                filter,
                new RaycastHit2D[1]
            ) > 0;

        if (jump && IsGrounded)
        {
            Bounce(jumpHeight);
            jump = false;
        }
        else if (stopJump)
        {
            if (body.velocity.y > 0)
            {
                body.AddForce(Vector2.down * jumpDeceleration, ForceMode2D.Impulse);
            }
            else if (jumpState == JumpState.InFlight)
                stopJump = false;
        }

        UpdateJumpState();
        PerformMovement(move);
    }

    void UpdateJumpState()
    {
        jump = false;
        switch (jumpState)
        {
            case JumpState.PrepareToJump:
                jumpState = JumpState.Jumping;
                jump = true;
                stopJump = false;
                break;
            case JumpState.Jumping:
                if (!IsGrounded)
                {
                    jumpState = JumpState.InFlight;
                }
                else if (stopJump)
                {
                    jumpState = JumpState.Landed;
                }
                break;
            case JumpState.InFlight:
                if (IsGrounded)
                {
                    jumpState = JumpState.Landed;
                }
                break;
            case JumpState.Landed:
                jumpState = JumpState.Grounded;
                break;
        }
    }

    protected override void ComputeVelocity()
    {
        if (move.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (move.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }

        animator.SetBool("grounded", IsGrounded);
        animator.SetFloat("velocityX", Mathf.Abs(body.velocity.x) / maxSpeed);
    }

    public enum JumpState
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position - position, size);
    }
}

public class SidescrollerControllerK : KinematicObject
{
    /// <summary>
    /// Max horizontal speed of the player.
    /// </summary>
    public float maxSpeed = 7;

    /// <summary>
    /// Initial jump velocity at the start of a jump.
    /// </summary>
    public float jumpTakeOffSpeed = 7;
    public float jumpDeceleration = 0;

    public JumpState jumpState = JumpState.Grounded;
    private bool stopJump;

    /*internal new*/public Collider2D collider2d;

    bool jump;
    Vector2 move;
    SpriteRenderer spriteRenderer;
    internal Animator animator;

    public Bounds Bounds => collider2d.bounds;
    private MainInputActions playerInput;

    protected override void Start()
    {
        base.Start();
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        while (playerInput == null)
        {
            playerInput = InputManager.playerInput;
        }
        playerInput.Moving.Enable();
        playerInput.Moving.Jump.started += context =>
            jumpState = (jumpState == JumpState.Grounded) ? JumpState.PrepareToJump : jumpState;
        playerInput.Moving.Jump.canceled += context => stopJump = true;
        playerInput.Melee.Enable();
    }

    protected override void Update()
    {
        move.x = playerInput.Moving.Move.ReadValue<float>();
        UpdateJumpState();
        base.Update();
    }

    void UpdateJumpState()
    {
        jump = false;
        switch (jumpState)
        {
            case JumpState.PrepareToJump:
                jumpState = JumpState.Jumping;
                jump = true;
                stopJump = false;
                break;
            case JumpState.Jumping:
                if (!IsGrounded)
                {
                    jumpState = JumpState.InFlight;
                }
                break;
            case JumpState.InFlight:
                if (IsGrounded)
                {
                    jumpState = JumpState.Landed;
                }
                break;
            case JumpState.Landed:
                jumpState = JumpState.Grounded;
                break;
        }
    }

    protected override void ComputeVelocity()
    {
        if (jump && IsGrounded)
        {
            velocity.y = jumpTakeOffSpeed;
            jump = false;
        }
        else if (stopJump)
        {
            stopJump = false;
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * jumpDeceleration;
            }
        }

        if (move.x > 0.01f)
            spriteRenderer.flipX = false;
        else if (move.x < -0.01f)
            spriteRenderer.flipX = true;

        animator.SetBool("grounded", IsGrounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }

    public enum JumpState
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed
    }
}
