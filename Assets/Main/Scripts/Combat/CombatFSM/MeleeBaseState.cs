using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public /*abstract*/ class MeleeBaseState : State
{
    protected Animator animator;
    protected bool shouldCombo;

    protected Stats playerStats;

    public float duration;
    public float comboFramesOpen, comboFramesStop;
    protected Collider2D hitCollider;
    protected List<Collider2D> damaged;
    protected string attackType;
    protected int attackIndex;

    private MainInputActions playerInput;

    public override void OnEnter(CombatStateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        shouldCombo = false;
        animator = GetComponent<Animator>();
        hitCollider = GetComponent<ComboCharacter>().hitbox;
        damaged = new List<Collider2D>();
        while (playerInput == null)
        {
            playerInput = InputManager.playerInput;
        }
        playerStats = CombatManager.playerStats;
        playerInput.Melee.Attack.performed += Combo;
        playerInput.Moving.Disable();
    }

    protected void Combo(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(time > comboFramesOpen && time < comboFramesStop)
        {
            shouldCombo = true;
        }
    }
    
    protected void StopCombo(UnityEngine.InputSystem.InputAction.CallbackContext callback)
    {
        shouldCombo = false;
        playerInput.Melee.Attack.performed -= Combo;
        stateMachine.SetNextStateToMain();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (time > duration && !shouldCombo)
        {
            playerInput.Moving.Enable();
            playerInput.Moving.Move.performed += StopCombo;
            playerInput.Moving.Jump.performed += StopCombo;
            playerInput.Melee.Dash.performed += StopCombo;
        }
        if (animator.GetFloat("WeaponActive") > 0f)
        {
            Attack();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        playerInput.Melee.Attack.performed -= Combo;
        playerInput.Moving.Move.performed -= StopCombo;
        playerInput.Moving.Jump.performed -= StopCombo;
        playerInput.Melee.Dash.performed -= StopCombo;
    }

    protected void Attack()
    {
        List<Collider2D> hits = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        _ = Physics2D.OverlapCollider(hitCollider, filter, hits);
        foreach(Collider2D current in hits)
        {
            if (!damaged.Contains(current))
            {
                if (current.TryGetComponent<Enemy>(out var temp)) {
                    Stats hitObjectStats = temp.stats;
                    if (hitObjectStats.team == Team.Enemy)
                    {
                        playerStats.DealDamage(hitObjectStats);
                        //Debug.Log("Dealer stats: " + playerStats + "\nReceiver stats: " + hitObjectStats);
                        damaged.Add(current);
                    }
                }
            }
        }
    }
}
