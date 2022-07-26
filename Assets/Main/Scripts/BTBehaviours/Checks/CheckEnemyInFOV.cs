using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeNS;

public class CheckEnemyInFOV : Node
{
    private Transform transform;
    private LayerMask mask;
    private Enemy caller;
    private float chaseDist;

    public CheckEnemyInFOV(Enemy _caller, Transform _transform, LayerMask _mask)
    {
        transform = _transform;
        mask = _mask;
        caller = _caller;
        chaseDist = caller.chaseDist;
    }

    public override NodeState Run()
    {
        //!Directives and final return are included to make debug easier

        //Checks if target was already set
        Transform target = (Transform) GetData("target");
        if(target == null)
        {   
            //If it wasn't, check for colliders in the area
            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                transform.position,
                caller.lookRange,
                mask
            );

            //If any collider is tagged as player, set is as target and succeed
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    caller.root.SetData("target", collider.transform);
                    state = NodeState.Success;

                    #if !UNITY_EDITOR
                    return state;
                    #endif
                }
            }

            //If none of them were the player, return false
            if(state != NodeState.Success)
            {
                state = NodeState.Failure;
                #if !UNITY_EDITOR
                return state;
                #endif
            }
            
        }
        else
        {
            //If the target is set, checks if its a transform and then if the distance between enemy and player is
            //lesser than previously set, returns success, failure otherwise
            if(target.GetType() == typeof(Transform))
            {
                if(Vector2.Distance(transform.position, target.position) < chaseDist)
                {
                    state = NodeState.Success;
                }
                else
                {
                    caller.root.ClearData("target");
                    state = NodeState.Failure;
                }
            }
            else
                state = NodeState.Failure;
        }
        //Debug.Log(state);
        return state;
    }
}
