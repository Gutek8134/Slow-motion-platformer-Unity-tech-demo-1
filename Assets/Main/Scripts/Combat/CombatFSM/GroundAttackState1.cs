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
        duration = 0.6f;
        comboFramesOpen = duration-0.25f;
        comboFramesStop = duration+0.15f;
        animator.SetTrigger($"Attack{attackType}{attackIndex}");
        //Debug.Log($"Player Attack{attackType}{attackIndex} fired!");
        
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        /*
        if (time < comboFramesOpen + 0.02f && time > comboFramesOpen - 0.02f)
        {
            Debug.Log("Combo frame open");
        }

        if (time < comboFramesStop + 0.02f && time > comboFramesStop - 0.02f)
        {
            Debug.Log("Combo frame stop");
        }*/

        //If you extended combo, it's time to execute it
        if (time > duration && time < comboFramesStop && shouldCombo)
        {
            stateMachine.SetNextState(new GroundAttackState2());
        }
        //Threw "&& !shouldCombo" out, because if it's already past cFS time, then time is surely greater than duration
        //That means this part fires when:
        //cFS < time < duration && something => always false, because cFS>duration
        //duration < time > cFS && !should combo => right what I need
        //
        //Still, black magic code
        else if(time > comboFramesStop)
        {
            stateMachine.SetNextStateToMain();
        }
    }
}
