using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Manages the in-game shop by maintaining its open/closed state, 
/// handling tower purchases, and relaying relevant events through the ShopEventBus.
/// </summary>
public class ShopManager : Manager<ShopManager>
{
    /// <summary>
    /// A reference to the ShopData ScriptableObject, containing store items and configurations.
    /// </summary>
    public ShopData ShopData;
    
    /// <summary>
    /// Indicates whether the shop is currently open. True if open, false otherwise.
    /// </summary>
    public bool ShopIsOpen { get; private set; } = false;
    

    /// <summary>
    /// Subscribes to the OnTowerBought event for tower purchase logic.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        ShopEventBus.OnTowerBought += BuyTower;
    }

    /// <summary>
    /// Verifies that ShopData is assigned, logging a warning if it's missing.
    /// </summary>
    private void Start()
    {
        if(!ShopData) Debug.Log("Shop data not set in shop manager!");
    }

    /// <summary>
    /// Opens the shop, notifies the system via ShopEventBus, and sets ShopIsOpen to true.
    /// </summary>
    public void ActivateShop()
    { 
        ShopIsOpen = true;
        //Debug.Log("open shop");
        ShopEventBus.OpenShop();
    }

    /// <summary>
    /// Closes the shop, cancels any pending invocations of itself, and sets ShopIsOpen to false,
    /// while signaling the system via ShopEventBus.
    /// </summary>
    public void DeactivateShop()
    {
        CancelInvoke(nameof(DeactivateShop));
        ShopIsOpen = false;
        //Debug.Log("close shop");
        ShopEventBus.CloseShop();
    }

    /// <summary>
    /// Used by the shop to trigger the next step in the overall game loop logic.
    /// </summary>
    public void CheckGameLoop()
    {
        ServiceLocator.Get<GameManager>().GameLoop();
    }
    
    /// <summary>
    /// Handles the creation of a new tower when purchased from the shop, deducting 
    /// its cost and spawning a tower object in a dragging state for placement.
    /// </summary>
    /// <param name="towerData">The data defining the tower to purchase.</param>
    private void BuyTower(TowerData towerData)
    {
        EconomyEventBus.SpendMoney(towerData.GetStructAtLevel(TowerData.Level.Basic).BasicPrice);

        Tower newTower = Instantiate(
            ServiceLocator.Get<TowerManager>().TowerPrefab, 
            Vector3.zero, 
            Quaternion.identity, 
            ServiceLocator.Get<TowerManager>().TowerParentObject.transform
        );

        newTower.TowerData = towerData;
        newTower.SM.TransitToState(new DraggingShopStateTower(newTower.SM));
    }

    /// <summary>
    /// Unsubscribes from the tower purchase event when this ShopManager is destroyed.
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();
        ShopEventBus.OnTowerBought -= BuyTower;
    }

    protected override void Reload() {}
}
