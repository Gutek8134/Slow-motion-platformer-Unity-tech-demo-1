using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeNS;

public class CheckEnemyInAttackRange : Node
{
    private Enemy caller;
    private Transform target;

    public CheckEnemyInAttackRange(Enemy _caller)
    {
        caller = _caller;
    }

    public override NodeState Run()
    {
        NodeState state = NodeState.Failure;
        if (!ComboCharacter.lives)
            return state;
        target = (Transform)GetData("target");
        if (target != null)
        {
            state =
                Vector2.Distance(target.position, caller.transform.position)
                <= caller.attackDistance
                    ? NodeState.Success
                    : NodeState.Failure;
        }
        return state;
    }
}
