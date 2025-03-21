/// <summary>
/// Represents a tower state in which the tower is actively attacking. Inherits from IdleState, 
/// but transitions to an idle shop state when the shop is opened.
/// </summary>
public class AttackStateTower : IdleState
{
    public AttackStateTower(StateMachine pSM) : base(pSM)
    {
    }

    public override void OnEnterState()
    {
        //Debug.Log($"{SM.gameObject.name} enters attack state");
        TowerEventBus.TowerStartAttack(SM.gameObject);
        ShopEventBus.OnShopOpened += StartIdleShopState;
    }

    public override void Handle()
    {
    }

    public override void OnExitState()
    {
        TowerEventBus.TowerEndAttack(SM.gameObject);
        ShopEventBus.OnShopOpened -= StartIdleShopState;
    }
    
    /// <summary>
    /// Transitions the tower to an IdleShopStateTower when the shop is opened.
    /// </summary>
    private void StartIdleShopState()
    {
        SM.TransitToState(new IdleShopStateTower(SM));
    }
}