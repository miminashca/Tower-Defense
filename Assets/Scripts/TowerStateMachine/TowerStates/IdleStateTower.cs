using System;
using UnityEngine;

public class IdleStateTower : IdleState
{
    private void StartDrag()
    {
        SM.TransitToState(GetComponent<DraggingStateTower>());
    }
    private void OnMouseDown()
    {
        if(!TowerManager.Instance.ActiveTower && GetComponent<Placeable>().WasAlreadyPlaced && ShopManager.Instance.shopIsOpen) StartDrag();
    }

    public override void OnEnterState()
    {
        //Debug.Log($"{gameObject.name} enters idle state");
    }

    public override void Handle() {}

    public override void OnExitState()
    {
        EventBus.TowerStartDrag(gameObject);
    }
    
}
