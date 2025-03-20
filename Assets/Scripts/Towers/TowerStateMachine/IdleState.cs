/// <summary>
/// An abstract state representing an idle condition. Classes inheriting from this
/// are expected to implement the logic for when a tower or object remains stationary
/// and not actively performing other behaviors.
/// </summary>
public abstract class IdleState : State
{
    protected IdleState(StateMachine pSM) : base(pSM)
    {
    }
}