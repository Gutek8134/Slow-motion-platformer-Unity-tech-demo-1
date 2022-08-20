using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeNS;
using Pathfinding;

public class Patrol : Node
{
    private Transform transform;
    private Transform[] waypoints;

    private int currentWaypointIndex = 0;
    private float waitTime = 1.5f;
    private float waitCounter = 0f;

    private bool isWaiting = false;

    private Enemy caller;

    public Patrol(Enemy _caller, Transform _transform, Transform[] _waypoints)
    {
        transform = _transform;
        waypoints = _waypoints;
        caller = _caller;
    }

    public override NodeState Run()
    {
        if (isWaiting)
        {
            //If you are waiting, wait until you've waited enough
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
            {
                isWaiting = false;
            }
        }
        else
        {
            //Get the next point you should go to
            Transform wp = waypoints[currentWaypointIndex];
            //If the distance is very small, teleport your ass there and start waiting
            if (Vector2.Distance(transform.position, wp.position) <= 0.1f)
            {
                transform.position = wp.position;
                caller.rigidbody2d.velocity *= 0;
                waitCounter = 0f;
                isWaiting = true;

                //Get the next index, returning to zero after exceeding the limit
                currentWaypointIndex = ++currentWaypointIndex % waypoints.Length;
            }
            //If the distance is not that small, move in the general direction
            else
            {
                caller.rigidbody2d.AddForce(
                    (wp.position - transform.position).normalized * caller.speed
                );
            }
        }
        //You can't stop patrolling, can you?
        state = NodeState.Running;
        return state;
    }
}
