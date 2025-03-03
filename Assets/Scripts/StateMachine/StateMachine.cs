using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public event System.Action<int> OnEnterStateWithID;
    public event System.Action<int> OnExitStateWithID;

    [SerializeField] public State currentState {get; private set;}
    private void OnEnable()
    {
        State[] states = GetComponents<State>();
        if (states.Length > 0)
        {
            foreach (State state in states)
            {
                state.Init(this);
            }
        }
        //Set default to chase state to setup the agent.
        if(!currentState) TransitToState(GetComponent<IdleState>());
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Handle();
    }

    public void TransitToState(State pState)
    {
        if(currentState != null)
        {
            currentState.OnExitState();
            //Notify subscribers about the state change, e.g. an animation controller
            //can listen to this event and use the stateID to play/stop animation
            OnExitStateWithID?.Invoke(currentState.stateID);
        }
        currentState = pState;
        currentState.OnEnterState();
        //Notify subscribers about the state change, e.g. an animation controller
        //can listen to this event and use the stateID to play/stop animation
        OnEnterStateWithID?.Invoke(currentState.stateID);
    }

    private void OnDestroy()
    {
        // if no state machine - no states
        List<State> states = new List<State>(GetComponents<State>());
        foreach (State state in states)
        {
            Destroy(state);
        }
    }
}