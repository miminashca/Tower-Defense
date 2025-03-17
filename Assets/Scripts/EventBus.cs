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
    public static event Action<Tower> OnTowerBecameActive;
    
    
    
    public static event Action OnShopOpened;
    public static event Action OnShopClosed;
    
    public static event Action<int> OnMoneySpent;
    public static event Action<int> OnMoneyEarned;
    public static event Action OnMoneyAmountChange;
    
    public static event Action<Enemy> OnEntityReceivedDamage;
    public static event Action<Enemy, float> OnTowerDebuffedEntity;
    public static event Action<Enemy> OnEntityDeath;
    
    public static event Action OnLose;
    public static event Action OnWin;

    
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
    
    public static void EntityReceivedDamage(Enemy enemy)
    {
        OnEntityReceivedDamage?.Invoke(enemy);
    }
    public static void TowerDebuffEntity(Enemy enemy, float debuffPower)
    {
        OnTowerDebuffedEntity?.Invoke(enemy, debuffPower);
    }
    public static void EntityDie(Enemy enemy)
    {
        OnEntityDeath?.Invoke(enemy);
    }
    
    public static void Lose()
    {
        OnLose?.Invoke();
    }
    
    public static void Win()
    {
        OnWin?.Invoke();
    }
    
}