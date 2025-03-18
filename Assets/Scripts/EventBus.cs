using System;
using UnityEngine;

public static class EventBus
{
    public static event Action OnShopOpened;
    public static event Action OnShopClosed;
    public static event Action<Tower> OnTowerUpgraded;
    public static void UpgradeTower(Tower tower)
    {
        if(tower) OnTowerUpgraded?.Invoke(tower);
    }
    public static event Action<TowerData> OnTowerBought;
    public static void BuyTower(TowerData towerData)
    {
        OnTowerBought?.Invoke(towerData);
    }
    
    
    public static event Action<int> OnMoneySpent;
    public static event Action<int> OnMoneyEarned;
    public static event Action OnMoneyAmountChange;
    
    
    public static event Action OnLose;
    public static event Action OnWin;

    
    
    
    
    
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
    
    
    
    public static void Lose()
    {
        OnLose?.Invoke();
    }
    
    public static void Win()
    {
        OnWin?.Invoke();
    }
    
}