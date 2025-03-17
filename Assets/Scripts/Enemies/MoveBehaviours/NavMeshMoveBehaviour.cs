using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMoveBehaviour : MoveBehaviour
{
    private NavMeshAgent agent;
    
    protected override void Awake()
    {
        base.Awake();
        if (!GetComponent<NavMeshAgent>())
        {
            agent = gameObject.AddComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        }
        else
        {
            agent = GetComponent<NavMeshAgent>();
        }

        targetRange = 2f;
    }
    
    public override void SetTargetPosition(Vector3 position)
    {
        base.SetTargetPosition(position);
        agent.destination = position;
    }

    protected override void Stop()
    { 
        base.Stop();
        agent.ResetPath();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        agent.speed = speed;
    }
}
