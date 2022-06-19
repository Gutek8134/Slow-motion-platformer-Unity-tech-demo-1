using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttackState1 : MeleeBaseState
{
    public override void OnEnter(CombatStateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        attackIndex = 1;
        attackType = "Ground";
        duration = 0.5f;
        comboFramesOpen = 0.35f;
        comboFramesStop = 0.55f;
        animator.SetTrigger($"Attack{attackType}{attackIndex}");
        Debug.Log($"Player Attack{attackType}{attackIndex} fired!");
        
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (time < comboFramesOpen + 0.02f && time > comboFramesOpen - 0.02f)
        {
            Debug.Log("Combo frame open");
        }

        if (time < comboFramesStop + 0.02f && time > comboFramesStop - 0.02f)
        {
            Debug.Log("Combo frame stop");
        }

        if (time > duration && time < comboFramesStop && shouldCombo)
        {
            stateMachine.SetNextState(new GroundAttackState2());
        }
        else if(time > comboFramesStop)
        {
            stateMachine.SetNextStateToMain();
        }
    }
}
