using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Class for determining which melee state to use. Plot twist: there's only one ATM.</summary>
///<remarks>Fun fact: this state lasts for 3 frames max</remarks>
public class MeleeEntryState : State
{
    State nextState;
    public override void OnEnter(CombatStateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        nextState = new GroundAttackState1();//This will probably change to some switch/case
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        stateMachine.SetNextState(nextState);
    }
}
