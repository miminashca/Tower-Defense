using System;

/// <summary>
/// The ShopEventBus class defines global events for opening/closing the shop,
/// tower upgrades, and tower purchases. Other systems can subscribe to these events
/// to react without direct references to the Shop.
/// </summary>
public static class ShopEventBus
{
    /// <summary>
    /// Triggered when the shop is opened.
    /// </summary>
    public static event Action OnShopOpened;

    /// <summary>
    /// Invokes the OnShopOpened event to indicate the shop has been opened.
    /// </summary>
    public static void OpenShop()
    {
        OnShopOpened?.Invoke();
    }
    
    /// <summary>
    /// Triggered when the shop is closed.
    /// </summary>
    public static event Action OnShopClosed;

    /// <summary>
    /// Invokes the OnShopClosed event to indicate the shop has been closed.
    /// </summary>
    public static void CloseShop()
    {
        OnShopClosed?.Invoke();
    }
    
    /// <summary>
    /// Triggered when a tower is upgraded.
    /// </summary>
    public static event Action<Tower> OnTowerUpgraded;

    /// <summary>
    /// Invokes the OnTowerUpgraded event for the specified tower.
    /// </summary>
    /// <param name="tower">The tower that was upgraded.</param>
    public static void UpgradeTower(Tower tower)
    {
        if(tower) OnTowerUpgraded?.Invoke(tower);
    }
    
    /// <summary>
    /// Triggered when a tower is purchased in the shop.
    /// </summary>
    public static event Action<TowerData> OnTowerBought;

    /// <summary>
    /// Invokes the OnTowerBought event, providing the purchased TowerData.
    /// </summary>
    /// <param name="towerData">Information about the purchased tower.</param>
    public static void BuyTower(TowerData towerData)
    {
        OnTowerBought?.Invoke(towerData);
    }
}