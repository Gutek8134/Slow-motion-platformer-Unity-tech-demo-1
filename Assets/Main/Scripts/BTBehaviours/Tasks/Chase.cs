using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeNS;
using Pathfinding;

public class Chase : Node
{
    private static float timer;

    Enemy caller;

    Transform target;
    Transform transform;
    float speed;
    float attackDistance;

    Path path;
    int currentWaypoint;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public Chase(Enemy _caller, Transform _transform)
    {
        caller = _caller;
        transform = _transform;
        rb = caller.rigidbody2d;
        speed = caller.speed * 1.2f;
        attackDistance = caller.attackDistance;
    }

    public override NodeState Run()
    {
        target = (Transform)GetData("target");

        if (Vector2.Distance(transform.position, target.position) <= attackDistance)
        {
            rb.velocity = Vector2.zero;
            return NodeState.Success;
        }

        rb.AddForce(
            new Vector2(target.position.x > transform.position.x ? speed : -speed, 0),
            ForceMode2D.Force
        );
        return NodeState.Running;
    }
}

public class ChaseOld : Node
{
    private static float timer;

    Enemy caller;

    Transform target;
    Transform transform;
    float speed;
    float nextWaypointDistance;

    Path path;
    int currentWaypoint;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public ChaseOld(Enemy _caller, Transform _transform)
    {
        caller = _caller;
        transform = _transform;
        speed = caller.speed * 100;
        nextWaypointDistance = caller.nextWaypointDistance;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public override NodeState Run()
    {
        target = (Transform)GetData("target");
        seeker = caller.seeker;
        rb = caller.rigidbody2d;
        timer += Time.deltaTime;
        if (timer >= caller.repathTime && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
            timer = 0;
        }

        if (path == null)
            return NodeState.Failure;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            caller.root.SetData("reachedEndOfPath", reachedEndOfPath);
            return NodeState.Success;
        }
        reachedEndOfPath = false;
        caller.root.SetData("reachedEndOfPath", reachedEndOfPath);
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        rb.AddForce(force);

        if (distance < nextWaypointDistance)
        {
            ++currentWaypoint;
        }

        return NodeState.Success;
    }
}
