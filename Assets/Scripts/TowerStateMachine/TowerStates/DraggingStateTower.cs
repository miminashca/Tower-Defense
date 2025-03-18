using UnityEngine;

public class DraggingStateTower : State
{
    public DraggingStateTower(StateMachine pSM) : base(pSM)
    {
    }
    public override void OnEnterState()
    {
        //Debug.Log($"{gameObject.name} enters dragging state");
    }
    public override void Handle()
    {
        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }
    public override void OnExitState() {}
    
    private void EndDrag()
    {
        EventBus.TowerEndDrag(SM.gameObject);
        SM.TransitToState(new IdleStateTower(SM));
    }
}
