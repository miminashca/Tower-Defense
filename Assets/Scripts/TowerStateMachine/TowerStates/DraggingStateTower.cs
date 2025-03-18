using UnityEngine;

public class DraggingStateTower : State
{
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
        EventBus.TowerEndDrag(gameObject);
        SM.TransitToState(GetComponent<IdleStateTower>());
    }
}
