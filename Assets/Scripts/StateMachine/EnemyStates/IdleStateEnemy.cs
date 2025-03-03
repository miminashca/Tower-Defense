using System;
using UnityEngine;

public class IdleStateEnemy : IdleState
{
    private MoveBehaviour moveBehaviour;

    public override void OnEnterState()
    {
        moveBehaviour = GetComponent<MoveBehaviour>();
        if (moveBehaviour) moveBehaviour.OnTargetSet += StartMove;
    }

    public override void Handle() {}
    
    private void StartMove()
    {
        if(this) SM.TransitToState(GetComponent<MovingStateEnemy>());
    }

    public override void OnExitState()
    {
        if (moveBehaviour) moveBehaviour.OnTargetSet -= StartMove;
    }
    
}