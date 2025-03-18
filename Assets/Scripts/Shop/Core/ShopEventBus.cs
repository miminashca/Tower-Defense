using System;

public static class ShopEventBus
{
    public static event Action OnShopOpened;
    public static void OpenShop()
    {
        OnShopOpened?.Invoke();
    }
    
    public static event Action OnShopClosed;
    public static void CloseShop()
    {
        OnShopClosed?.Invoke();
    }
    
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
}