using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBaseState : State
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
    }

    protected void Combo(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(time > comboFramesOpen && time < comboFramesStop)
        {
            shouldCombo = true;
        }
    }
    

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
