using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeNS;

public class AttackPlayer : Node
{
    private Stats player;
    private static float timer = 0;
    private Enemy caller;

    public AttackPlayer(Enemy _caller)
    {
        caller = _caller;
    }

    public override NodeState Run()
    {
        caller.rigidbody2d.velocity = Vector2.zero;
        if (timer <= 0)
        {
            ComboCharacter target = ((Transform)GetData("target")).GetComponent<ComboCharacter>();
            //Run animation
            target.ReceiveDamage(caller.stats);
            if (target.playerStats.currentHP <= 0)
                caller.root.ClearData("target");
            timer = caller.attackSpeed;
        }
        else
            timer -= Time.deltaTime;

        return NodeState.Success;
    }
}
