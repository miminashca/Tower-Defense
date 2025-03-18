using UnityEngine;

public class AttackStateTower : IdleState
{
    public AttackStateTower(StateMachine pSM) : base(pSM)
    {
    }
    public override void OnEnterState()
    {
        Debug.Log($"{SM.gameObject.name} enters attack state");
        EventBus.TowerStartAttack(SM.gameObject);
        EventBus.OnShopOpened += StartIdleShopState;
    }

    public override void Handle()
    {
    }

    public override void OnExitState()
    {
        EventBus.TowerEndAttack(SM.gameObject);
        EventBus.OnShopOpened -= StartIdleShopState;
    }
    
    private void StartIdleShopState()
    {
        SM.TransitToState(new IdleShopStateTower(SM));
    }
}