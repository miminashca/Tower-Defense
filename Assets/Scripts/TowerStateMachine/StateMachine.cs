using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State currentState;
    private void Start()
    {
        if (ShopManager.Instance.shopIsOpen)
        {
            if(GetComponent<Tower>().WasAlreadyPlaced) TransitToState(new IdleShopStateTower(this));
            else TransitToState(new DraggingShopStateTower(this));
        }
        else
        {
            TransitToState(new AttackStateTower(this));
        }
    }

    void Update()
    {
        if(currentState != null) currentState.Handle();
    }

    public void TransitToState(State pState)
    {
        if(currentState != null)
        {
            currentState.OnExitState();
        }
        currentState = pState;
        currentState.OnEnterState();
    }

    private void OnDestroy()
    {
        if(currentState!=null) currentState.OnExitState();
    }
}