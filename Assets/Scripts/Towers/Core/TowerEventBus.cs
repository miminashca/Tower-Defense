using System;
using UnityEngine;

/// <summary>
/// The TowerEventBus class provides a set of global events related to tower actions, 
/// such as starting or ending attacks, being dragged, and being placed or removed. 
/// Other systems can subscribe to these events to react to tower state changes 
/// without needing a direct reference to the tower object.
/// </summary>
public static class TowerEventBus
{
    /// <summary>
    /// Fired when a tower begins attacking.
    /// The parameter is the GameObject representing the tower that started attacking.
    /// </summary>
    public static event Action<GameObject> OnTowerStartAttack;
    
    /// <summary>
    /// Invokes the OnTowerStartAttack event, signaling that a specific tower has started attacking.
    /// </summary>
    /// <param name="tower">The tower GameObject that initiated the attack.</param>
    public static void TowerStartAttack(GameObject tower)
    {
        if(tower) OnTowerStartAttack?.Invoke(tower);
    }
    
    /// <summary>
    /// Fired when a tower finishes attacking.
    /// The parameter is the GameObject representing the tower that stopped attacking.
    /// </summary>
    public static event Action<GameObject> OnTowerEndAttack;
    
    /// <summary>
    /// Invokes the OnTowerEndAttack event, signaling that a specific tower has stopped attacking.
    /// </summary>
    /// <param name="tower">The tower GameObject that ended the attack.</param>
    public static void TowerEndAttack(GameObject tower)
    {
        if(tower) OnTowerEndAttack?.Invoke(tower);
    }
    
    /// <summary>
    /// Fired when a tower starts being dragged.
    /// The parameter is the GameObject representing the tower that is being dragged.
    /// </summary>
    public static event Action<GameObject> OnTowerStartDrag;
    
    /// <summary>
    /// Invokes the OnTowerStartDrag event, signaling that a specific tower is now being dragged.
    /// </summary>
    /// <param name="tower">The tower GameObject that started dragging.</param>
    public static void TowerStartDrag(GameObject tower)
    {
        if(tower) OnTowerStartDrag?.Invoke(tower);
    }
    
    /// <summary>
    /// Fired when a tower finishes being dragged.
    /// The parameter is the GameObject representing the tower that finished dragging.
    /// </summary>
    public static event Action<GameObject> OnTowerEndDrag;
    
    /// <summary>
    /// Invokes the OnTowerEndDrag event, signaling that a specific tower has stopped being dragged.
    /// </summary>
    /// <param name="tower">The tower GameObject that ended dragging.</param>
    public static void TowerEndDrag(GameObject tower)
    {
        if(tower) OnTowerEndDrag?.Invoke(tower);
    }
    
    /// <summary>
    /// Fired when a tower has been successfully placed in the scene.
    /// The parameter is the Tower instance that was placed.
    /// </summary>
    public static event Action<Tower> OnTowerPlaced;
    
    /// <summary>
    /// Invokes the OnTowerPlaced event, indicating that a specific Tower has been placed.
    /// </summary>
    /// <param name="tower">The Tower instance that was placed.</param>
    public static void PlaceTower(Tower tower)
    {
        if(tower) OnTowerPlaced?.Invoke(tower);
    }
    
    /// <summary>
    /// Fired when a tower is removed from the scene.
    /// The parameter is the Tower instance that was removed.
    /// </summary>
    public static event Action<Tower> OnTowerRemoved;
    
    /// <summary>
    /// Invokes the OnTowerRemoved event, indicating that a specific Tower has been removed.
    /// </summary>
    /// <param name="tower">The Tower instance that was removed.</param>
    public static void RemoveTower(Tower tower)
    {
        if(tower) OnTowerRemoved?.Invoke(tower);
    }
    
    /// <summary>
    /// Fired when a tower is moved and snapped to a specific grid position.
    /// The parameter is the Vector3Int position where the tower was snapped.
    /// </summary>
    public static event Action<Vector3Int> OnTowerMovedToSnappedPosition;
    
    /// <summary>
    /// Invokes the OnTowerMovedToSnappedPosition event, indicating a tower has snapped 
    /// to a specified grid position.
    /// </summary>
    /// <param name="snappedPosition">The grid position to which the tower was moved.</param>
    public static void TowerMovedToSnappedPosition(Vector3Int snappedPosition)
    {
        OnTowerMovedToSnappedPosition?.Invoke(snappedPosition);
    }
    
    /// <summary>
    /// Fired when a tower becomes active in gameplay (e.g., selected or clicked).
    /// The parameter is the Tower instance that became active.
    /// </summary>
    public static event Action<Tower> OnTowerBecameActive;
    
    /// <summary>
    /// Invokes the OnTowerBecameActive event, indicating that a specific Tower is now active.
    /// </summary>
    /// <param name="tower">The Tower instance that became active.</param>
    public static void TowerBecameActive(Tower tower)
    {
        OnTowerBecameActive?.Invoke(tower);
    }
}
