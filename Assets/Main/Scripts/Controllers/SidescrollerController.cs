using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SidescrollerController : KinematicObject
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
    /*internal new*/ public Collider2D collider2d;

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
        playerInput.Moving.Jump.started += context => jumpState = (jumpState == JumpState.Grounded) ? JumpState.PrepareToJump : jumpState;
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