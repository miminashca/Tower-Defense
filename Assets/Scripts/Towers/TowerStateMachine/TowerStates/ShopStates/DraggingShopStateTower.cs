using UnityEngine;

public class DraggingShopStateTower : State
{
    public DraggingShopStateTower(StateMachine pSM) : base(pSM)
    {
    }
    public override void OnEnterState()
    {
        //Debug.Log($"{SM.gameObject.name} enters dragging state");
        TowerEventBus.TowerStartDrag(SM.gameObject);
        ShopEventBus.OnShopClosed += StartAttackState;
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
        ShopEventBus.OnShopClosed -= StartAttackState;
    }
    
    private void EndDrag()
    {
        TowerEventBus.TowerEndDrag(SM.gameObject);
        SM.TransitToState(new IdleShopStateTower(SM));
    }

    private void StartAttackState()
    {
        TowerEventBus.TowerEndDrag(SM.gameObject);
        SM.TransitToState(new AttackStateTower(SM));
    }
}
