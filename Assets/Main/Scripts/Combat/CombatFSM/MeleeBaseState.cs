using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>A state from all melee states derive from</summary>
public /*abstract*/ class MeleeBaseState : State
{
    protected Animator animator;
    protected bool shouldCombo;

    protected Stats playerStats;

    public float duration;
    ///<value>These two say when you can hit the button to continue attacking</value>
    public float comboFramesOpen, comboFramesStop;
    ///<value>What collider to use for hitting stuff</value>
    protected Collider2D hitCollider;
    ///<value>I think I've damaged those in this attack</value>
    protected List<Collider2D> damaged;

    //Names for attacks for partial automation on firing Animator triggers
    protected string attackType;
    protected int attackIndex;

    private MainInputActions playerInput;

    public override void OnEnter(CombatStateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        
        //Value initialization
        shouldCombo = false;
        animator = GetComponent<Animator>();
        hitCollider = GetComponent<ComboCharacter>().hitbox;
        damaged = new List<Collider2D>();
        playerStats = CombatManager.playerStats;
        while (playerInput == null)
        {
            playerInput = InputManager.playerInput;
        }

        //Input initialization
        playerInput.Melee.Attack.performed += Combo;
        playerInput.Moving.Disable();
    }

    ///<summary>Trying to extend your combo? Then you have to go through me!</summary>
    protected void Combo(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(time > comboFramesOpen && time < comboFramesStop)
        {
            shouldCombo = true;
        }
    }
    
    ///<summary>You've moved, so I guess you don't need to attack anymore.
    ///Anyway, it makes you unable to extend combo and cancels it.</summary>
    ///<remarks>Probably running like coward RN</remarks>
    protected void StopCombo(UnityEngine.InputSystem.InputAction.CallbackContext callback)
    {
        shouldCombo = false;
        playerInput.Melee.Attack.performed -= Combo;
        stateMachine.SetNextStateToMain();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //You cannot move until you finish attacking
        if (time > duration && !shouldCombo)
        {
            playerInput.Moving.Enable();
            playerInput.Moving.Move.performed += StopCombo;
            playerInput.Moving.Jump.performed += StopCombo;
            playerInput.Melee.Dash.performed += StopCombo;
        }
        //Earlier used comboFramesOpen/Stop, but this thing is a bit more accurate
        if (animator.GetFloat("WeaponActive") > 0f)
        {
            Attack();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        //Throwing out garbage
        playerInput.Melee.Attack.performed -= Combo;
        playerInput.Moving.Move.performed -= StopCombo;
        playerInput.Moving.Jump.performed -= StopCombo;
        playerInput.Melee.Dash.performed -= StopCombo;
    }

    ///<summary>Don't think about it too much, it worked when I checked it last time</summary>
    protected void Attack()
    {
        //Checking if hit some trigger
        List<Collider2D> hits = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        _ = Physics2D.OverlapCollider(hitCollider, filter, hits);

        //For everything that was hit:
        foreach(Collider2D current in hits)
        {
            //Check if it wasn't damaged in this state
            if (!damaged.Contains(current))
            {
                //If it is also damageable:
                //(I hope this spaghetti improves the performance)
                if (current.TryGetComponent(out IDamageable hit)) {
                    //And deal damage if it is an enemy
                    if (hit.stats.team == Team.Enemy)
                    {
                        hit.ReceiveDamage(playerStats);//Maybe I should change everything to Receive so I won't have to deal with updating?
                        hit.UpdateHP();
                        //Debug.Log("Dealer stats: " + playerStats + "\nReceiver stats: " + hitObjectStats);
                        damaged.Add(current);//This prevents attacking same target twice with one attack
                        
                        //MAMMA MIA, MARCELLO! WHAT A MAGNIFICENT SPAGHETTI!
                    }
                }
            }
        }
    }
}
