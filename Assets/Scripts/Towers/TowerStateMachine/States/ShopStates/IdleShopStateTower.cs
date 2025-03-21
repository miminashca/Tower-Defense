using UnityEngine;

/// <summary>
/// Represents the tower's idle state when the shop is open. The tower remains idle 
/// until the player clicks on it to begin dragging, or until the shop closes, at which 
/// point it transitions to an attack state.
/// </summary>
public class IdleShopStateTower : IdleState
{
    public IdleShopStateTower(StateMachine pSM) : base(pSM)
    {
    }
    public override void OnEnterState()
    {
        // Debug.Log($"{SM.gameObject.name} enters idle state");
        ShopEventBus.OnShopClosed += StartAttackState;
    }
    public override void Handle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (InputReader.Instance.CheckPressOnGameObject(SM.gameObject))
            {
                StartDrag();
            }
        }
    }
    public override void OnExitState()
    {
        ShopEventBus.OnShopClosed -= StartAttackState; 
    }
    
    /// <summary>
    /// Initiates the dragging process if the tower is not currently being dragged 
    /// by another object and has already been placed in the scene.
    /// </summary>
    private void StartDrag()
    {
        if (!ServiceLocator.Get<TowerManager>().ActiveTower && SM.gameObject.GetComponent<Placeable>().WasAlreadyPlaced)
        {
            SM.TransitToState(new DraggingShopStateTower(SM));
        }
    }

    /// <summary>
    /// Called when the shop closes, transitioning the tower to the AttackStateTower 
    /// so it can participate in normal gameplay.
    /// </summary>
    private void StartAttackState()
    {
        SM.TransitToState(new AttackStateTower(SM));
    }
}
