using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttackState3 : MeleeBaseState
{
    public override void OnEnter(CombatStateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        attackIndex = 3;
        attackType = "Ground";
        duration = 0.7f;
        comboFramesOpen = 0.55f;
        comboFramesStop = 0.75f;
        animator.SetTrigger($"Attack{attackType}{attackIndex}");
        Debug.Log($"Player Attack{attackType}{attackIndex} fired!");
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (time > duration)
        {
            stateMachine.SetNextStateToMain();
        }
    }
}
