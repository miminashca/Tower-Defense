using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStateEnemy : State
{
    private MoveBehaviour moveBehaviour;
    private void Start()
    {
        moveBehaviour = GetComponent<MoveBehaviour>();
    }

    public override void OnEnterState()
    {
        if (moveBehaviour) moveBehaviour.OnTargetReached += StopMove;
    }

    public override void Handle()
    {
    }
    
    private void StopMove()
    {
        if (SM)
        {
            //Debug.Log("go to idle");
            SM.TransitToState(GetComponent<IdleStateEnemy>());
        }
    }

    public override void OnExitState()
    {
        if (moveBehaviour) moveBehaviour.OnTargetReached -= StopMove;
    }
}
