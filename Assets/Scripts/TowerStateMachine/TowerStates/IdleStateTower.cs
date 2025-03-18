using UnityEngine;

public class IdleStateTower : IdleState
{
    public IdleStateTower(StateMachine pSM) : base(pSM)
    {
    }
    public override void OnEnterState()
    {
        //Debug.Log($"{gameObject.name} enters idle state");
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
        EventBus.TowerStartDrag(SM.gameObject);
    }
    
    private void StartDrag()
    {
        if(!TowerManager.Instance.ActiveTower && SM.gameObject.GetComponent<Placeable>().WasAlreadyPlaced && ShopManager.Instance.shopIsOpen) SM.TransitToState(new DraggingStateTower(SM));
    }
    // private void OnMouseDown()
    // {
    //     if(!TowerManager.Instance.ActiveTower && GetComponent<Placeable>().WasAlreadyPlaced && ShopManager.Instance.shopIsOpen) StartDrag();
    // }
}
