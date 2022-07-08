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
        duration = 1.1f;
        comboFramesOpen = duration - 0.25f;
        comboFramesStop = duration + 0.15f;
        animator.SetTrigger($"Attack{attackType}{attackIndex}");
        //Debug.Log($"Player Attack{attackType}{attackIndex} fired!");
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        //There's no attack to further extend the combo, so just wait for currrent attack to end and finish it
        if (time > duration)
        {
            stateMachine.SetNextStateToMain();
        }
    }
}
