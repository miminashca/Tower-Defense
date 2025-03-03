using System;
using UnityEngine;

public static class EventBus
{
    public static event Action<GameObject> OnTowerStartDrag;
    public static event Action<GameObject> OnTowerEndDrag;
    public static event Action<Tower> OnTowerPlaced;
    public static event Action<Tower> OnTowerRemoved;
    public static event Action<Tower> OnTowerUpgraded;
    public static event Action<TowerData> OnTowerBought;
    public static event Action<Vector3Int> OnTowerMovedToSnappedPosition;
    public static event Action<int> OnWaveStart;
    public static event Action<int> OnWaveEnd;
    public static event Action OnShopOpened;
    public static event Action OnShopClosed;
    public static event Action<int> OnMoneySpent;
    public static event Action<int> OnMoneyEarned;
    public static event Action OnMoneyAmountChange;
    public static event Action<Tower> OnTowerBecameActive;
    
    
    public static void TowerStartDrag(GameObject tower)
    {
        if(tower) OnTowerStartDrag?.Invoke(tower);
    }
    public static void TowerEndDrag(GameObject tower)
    {
        if(tower) OnTowerEndDrag?.Invoke(tower);
    }
    public static void PlaceTower(Tower tower)
    {
        if(tower) OnTowerPlaced?.Invoke(tower);
    }
    public static void RemoveTower(Tower tower)
    {
        if(tower) OnTowerRemoved?.Invoke(tower);
    }
    public static void UpgradeTower(Tower tower)
    {
        if(tower) OnTowerUpgraded?.Invoke(tower);
    }
    public static void BuyTower(TowerData towerData)
    {
        OnTowerBought?.Invoke(towerData);
    }
    public static void TowerMovedToSnappedPosition(Vector3Int snappedPosition)
    {
        OnTowerMovedToSnappedPosition?.Invoke(snappedPosition);
    }
    
    public static void StartWave(int currentWave)
    {
        OnWaveStart?.Invoke(currentWave);
    }
    public static void EndWave(int currentWave)
    {
        OnWaveEnd?.Invoke(currentWave);
    }
    public static void OpenShop()
    {
        OnShopOpened?.Invoke();
    }
    public static void CloseShop()
    {
        OnShopClosed?.Invoke();
    }
    
    public static void SpendMoney(int amount)
    {
        OnMoneySpent?.Invoke(amount);
        OnMoneyAmountChange?.Invoke();
    }
    public static void EarnMoney(int amount)
    {
        OnMoneyEarned?.Invoke(amount);
        OnMoneyAmountChange?.Invoke();
    }
    public static void TowerBecameActive(Tower tower)
    {
        OnTowerBecameActive?.Invoke(tower);
    }
}