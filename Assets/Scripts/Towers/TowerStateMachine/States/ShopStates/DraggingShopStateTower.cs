using UnityEngine;

/// <summary>
/// Represents the tower's state when it is being dragged in the shop phase. 
/// This state listens for mouse release to end dragging, and transitions to 
/// either an idle shop state or an attack state if the shop closes.
/// </summary>
public class DraggingShopStateTower : State
{
    public DraggingShopStateTower(StateMachine pSM) : base(pSM)
    {
    }
    public override void OnEnterState()
    {
        // Debug.Log($"{SM.gameObject.name} enters dragging state");
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

    /// <summary>
    /// Ends the dragging process by notifying the system that the tower 
    /// has stopped dragging and transitions to the idle shop state.
    /// </summary>
    private void EndDrag()
    {
        TowerEventBus.TowerEndDrag(SM.gameObject);
        SM.TransitToState(new IdleShopStateTower(SM));
    }

    /// <summary>
    /// Called when the shop closes. It ends the dragging process and transitions 
    /// to the AttackStateTower, indicating the tower should become active in gameplay.
    /// </summary>
    private void StartAttackState()
    {
        TowerEventBus.TowerEndDrag(SM.gameObject);
        SM.TransitToState(new AttackStateTower(SM));
    }
}
