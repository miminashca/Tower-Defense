using System;
using UnityEngine;

public static class TowerEventBus
{
    public static event Action<GameObject> OnTowerStartAttack;
    public static void TowerStartAttack(GameObject tower)
    {
        if(tower) OnTowerStartAttack?.Invoke(tower);
    }
    
    public static event Action<GameObject> OnTowerEndAttack;
    public static void TowerEndAttack(GameObject tower)
    {
        if(tower) OnTowerEndAttack?.Invoke(tower);
    }
    
    public static event Action<GameObject> OnTowerStartDrag;
    public static void TowerStartDrag(GameObject tower)
    {
        if(tower) OnTowerStartDrag?.Invoke(tower);
    }
    
    public static event Action<GameObject> OnTowerEndDrag;
    public static void TowerEndDrag(GameObject tower)
    {
        if(tower) OnTowerEndDrag?.Invoke(tower);
    }
    
    public static event Action<Tower> OnTowerPlaced;
    public static void PlaceTower(Tower tower)
    {
        if(tower) OnTowerPlaced?.Invoke(tower);
    }
    
    public static event Action<Tower> OnTowerRemoved;
    public static void RemoveTower(Tower tower)
    {
        if(tower) OnTowerRemoved?.Invoke(tower);
    }
    
    public static event Action<Vector3Int> OnTowerMovedToSnappedPosition;
    public static void TowerMovedToSnappedPosition(Vector3Int snappedPosition)
    {
        OnTowerMovedToSnappedPosition?.Invoke(snappedPosition);
    }
    
    public static event Action<Tower> OnTowerBecameActive;
    public static void TowerBecameActive(Tower tower)
    {
        OnTowerBecameActive?.Invoke(tower);
    }
}