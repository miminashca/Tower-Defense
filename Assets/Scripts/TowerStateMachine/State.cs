using UnityEngine;

/// <summary>
/// Base class for states
/// </summary>
public abstract class State : MonoBehaviour
{
    protected StateMachine SM;
    [SerializeField]
    protected int _stateID;
    public int stateID => _stateID;

    public void Init(StateMachine pSM)
    {
        SM = pSM;
    }

    public abstract void OnEnterState();
    public abstract void Handle();
    public abstract void OnExitState();
}