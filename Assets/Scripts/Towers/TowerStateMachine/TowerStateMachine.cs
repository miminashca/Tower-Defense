/// <summary>
/// A generic state machine for a tower, deciding which state the tower should be in 
/// (e.g., dragging in the shop, idle in the shop, or attacking in the game).
/// </summary>
public class TowerStateMachine : StateMachine
{
    /// <summary>
    /// The current state the tower is in.
    /// </summary>
    private State currentState;

    /// <summary>
    /// Called when the object is first enabled. Determines the initial state 
    /// based on whether the shop is open and whether the tower is already placed.
    /// </summary>
    protected override void Start()
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
}
