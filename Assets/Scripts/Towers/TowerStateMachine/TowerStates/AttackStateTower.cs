using UnityEngine;

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
    
    private void StartIdleShopState()
    {
        SM.TransitToState(new IdleShopStateTower(SM));
    }
}