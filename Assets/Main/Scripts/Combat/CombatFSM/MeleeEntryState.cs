using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEntryState : State
{
    State nextState;
    public override void OnEnter(CombatStateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        nextState = new GroundAttackState1();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        stateMachine.SetNextState(nextState);
    }
}
