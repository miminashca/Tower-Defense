using UnityEngine;

public class DraggingShopStateTower : State
{
    public DraggingShopStateTower(StateMachine pSM) : base(pSM)
    {
    }
    public override void OnEnterState()
    {
        Debug.Log($"{SM.gameObject.name} enters dragging state");
        EventBus.TowerStartDrag(SM.gameObject);
        EventBus.OnShopClosed += StartAttackState;
    }
    public override void Handle()
    {
        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    public override void OnExitState()
    {
        EventBus.OnShopClosed -= StartAttackState;
    }
    
    private void EndDrag()
    {
        EventBus.TowerEndDrag(SM.gameObject);
        SM.TransitToState(new IdleShopStateTower(SM));
    }

    private void StartAttackState()
    {
        EventBus.TowerEndDrag(SM.gameObject);
        SM.TransitToState(new AttackStateTower(SM));
    }
}
