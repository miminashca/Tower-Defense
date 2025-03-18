/// <summary>
/// Base class for states
/// </summary>
public abstract class State
{
    protected StateMachine SM;
    public State(StateMachine pSM)
    {
        SM = pSM;
    }

    public abstract void OnEnterState();
    public abstract void Handle();
    public abstract void OnExitState();
}