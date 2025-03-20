using UnityEngine;

/// <summary>
/// A generic state machine for a tower, deciding which state the tower should be in 
/// (e.g., dragging in the shop, idle in the shop, or attacking in the game).
/// </summary>
public class StateMachine : MonoBehaviour
{
    /// <summary>
    /// The current state the tower is in.
    /// </summary>
    private State currentState;

    /// <summary>
    /// Called when the object is first enabled. Determines the initial state 
    /// based on whether the shop is open and whether the tower is already placed.
    /// </summary>
    private void Start()
    {
        if (ShopManager.Instance.ShopIsOpen)
        {
            if (GetComponent<Placeable>().WasAlreadyPlaced)
                TransitToState(new IdleShopStateTower(this));
            else 
                TransitToState(new DraggingShopStateTower(this));
        }
        else
        {
            TransitToState(new AttackStateTower(this));
        }
    }

    /// <summary>
    /// Unity's Update loop calls the current state's Handle method each frame.
    /// </summary>
    private void Update()
    {
        if (currentState != null)
        {
            currentState.Handle();
        }
    }

    /// <summary>
    /// Transitions from the current state to a new one, calling the exit method 
    /// on the old state and the enter method on the new state.
    /// </summary>
    /// <param name="pState">The new state to transition to.</param>
    public void TransitToState(State pState)
    {
        if (currentState != null)
        {
            currentState.OnExitState();
        }
        currentState = pState;
        currentState.OnEnterState();
    }

    /// <summary>
    /// When this object is destroyed, ensures the current state properly exits 
    /// to avoid leaving behind event subscriptions or other references.
    /// </summary>
    private void OnDestroy()
    {
        if (currentState != null)
        {
            currentState.OnExitState();
        }
    }
}
