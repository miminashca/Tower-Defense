using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State currentState;
    private void OnEnable()
    {
        // State[] states = GetComponents<State>();
        // if (states.Length > 0)
        // {
        //     foreach (State state in states)
        //     {
        //         state.Init(this);
        //     }
        // }
        TransitToState(new IdleStateTower(this));
    }

    void Update()
    {
        currentState.Handle();
    }

    public void TransitToState(State pState)
    {
        if(currentState != null)
        {
            currentState.OnExitState();
        }
        currentState = pState;
        currentState.OnEnterState();
    }

    // private void OnDestroy()
    // {
    //     List<State> states = new List<State>(GetComponents<State>());
    //     foreach (State state in states)
    //     {
    //         Destroy(state);
    //     }
    // }
}