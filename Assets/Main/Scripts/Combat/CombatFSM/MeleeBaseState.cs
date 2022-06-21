using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public /*abstract*/ class MeleeBaseState : State
{
    protected Animator animator;
    protected bool shouldCombo;

    public float duration;
    public float comboFramesOpen, comboFramesStop;
    protected string attackType;
    protected int attackIndex;

    private MainInputActions playerInput;

    public override void OnEnter(CombatStateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        shouldCombo = false;
        animator = GetComponent<Animator>();
        while (playerInput == null)
        {
            playerInput = InputManager.playerInput;
        }
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
    }

    public override void OnExit()
    {
        base.OnExit();
        playerInput.Moving.Enable();
        playerInput.Moving.Move.performed -= StopCombo;
        playerInput.Moving.Jump.performed -= StopCombo;
        playerInput.Melee.Dash.performed -= StopCombo;
    }
}
