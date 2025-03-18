using UnityEngine;

public class IdleShopStateTower : IdleState
{
    public IdleShopStateTower(StateMachine pSM) : base(pSM)
    {
    }
    public override void OnEnterState()
    {
        //Debug.Log($"{SM.gameObject.name} enters idle state");
        EventBus.OnShopClosed += StartAttackState;
    }

    public override void Handle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(InputManager.Instance.CheckPressOnGameObject(SM.gameObject)) StartDrag();
        }
    }

    public override void OnExitState()
    {
        EventBus.OnShopClosed -= StartAttackState; 
    }
    
    private void StartDrag()
    {
        if(!TowerManager.Instance.ActiveTower && SM.gameObject.GetComponent<Placeable>().WasAlreadyPlaced) SM.TransitToState(new DraggingShopStateTower(SM));
    }
    private void StartAttackState()
    {
        SM.TransitToState(new AttackStateTower(SM));
    }
}
