using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

/// <summary>
/// Manages the UI for the in-game shop, including displaying tower items, 
/// handling tower purchases and upgrades, and showing item details in an info panel.
/// </summary>
public class ShopUI : MonoBehaviour
{
    /// <summary>
    /// A prefab for creating a button representing each shop item available for purchase.
    /// </summary>
    [SerializeField] private GameObject buttonPrefab;
    
    /// <summary>
    /// The main panel or window for the shop UI, shown or hidden as the shop opens and closes.
    /// </summary>
    [SerializeField] private GameObject shopPanel;
    
    /// <summary>
    /// The container panel for listing all available items in the shop.
    /// </summary>
    [SerializeField] private GameObject itemsPanel;
    
    /// <summary>
    /// The panel that displays item details (price, stats, etc.) 
    /// and relevant action buttons (e.g., buy or upgrade).
    /// </summary>
    [SerializeField] private GameObject itemInfoPanel;
    
    /// <summary>
    /// A specialized material used to visually indicate that a tower is upgradeable (highlight/outline).
    /// </summary>
    [SerializeField] private Material upgradeOutlineMat;

    /// <summary>
    /// A reference to the tower currently clicked on or selected by the player.
    /// </summary>
    private Tower clickedTower = null;

    /// <summary>
    /// Subscribes to events for opening/closing the shop, updating money, placing towers,
    /// and detecting when a tower becomes active (clicked).
    /// </summary>
    private void Awake()
    {
        ShopEventBus.OnShopOpened += ActivateShopUI;
        ShopEventBus.OnShopClosed += DeactivateShopUI;
        EconomyEventBus.OnMoneyAmountChange += CheckUpgradeAvailability;
        TowerEventBus.OnTowerPlaced += CheckUpgradeAvailabilityForTower;
        TowerEventBus.OnTowerBecameActive += OnTowerClicked;
    }

    /// <summary>
    /// Validates required UI references and populates the shop items panel if present.
    /// </summary>
    private void Start()
    {
        if(!shopPanel) Debug.Log("Shop Panel not set in shop manager!");
        if(!itemsPanel) Debug.Log("Items Panel not set in shop manager!");
        else
        {
            FillTheShop();
        }
        if(!itemInfoPanel) Debug.Log("Item Info Panel not set in shop manager!");
    }

    /// <summary>
    /// Activates the main shop panel and enables tower upgrade indicators.
    /// </summary>
    private void ActivateShopUI()
    {
        shopPanel.SetActive(true);
        EnableUpgradeIndicators();
    }

    /// <summary>
    /// Deactivates the shop panel, hides the item info panel, and disables tower upgrade indicators.
    /// </summary>
    private void DeactivateShopUI()
    {
        EnableUpgradeIndicators(false);
        itemInfoPanel.SetActive(false);
        shopPanel.SetActive(false);
    }

    /// <summary>
    /// Constructs a string describing basic info about the tower, such as price, range, and impact.
    /// </summary>
    /// <param name="towerData">The tower data to display.</param>
    /// <param name="levelToPurchase">Which level of the tower to show info for (Basic or an upgrade).</param>
    /// <returns>A formatted string of tower attributes.</returns>
    private string GetItemInfo(TowerData towerData, TowerData.Level levelToPurchase = TowerData.Level.Basic)
    {
        TowerData.TowerStruct selectedTowerStruct = towerData.GetStructAtLevel(levelToPurchase);
        return $"Price: {selectedTowerStruct.BasicPrice}\n" +
               $"Type: {towerData.GetTargetSelectingType()} {towerData.GetImpactType()}\n" +
               $"Range: {selectedTowerStruct.Range}\n" +
               $"Impact: {selectedTowerStruct.Impact}\n" +
               $"Threshold: {selectedTowerStruct.Threshold}";
    }

    /// <summary>
    /// Retrieves the preview sprite for the tower at the specified level, 
    /// used to display an icon in the shop UI.
    /// </summary>
    /// <param name="towerData">The tower data containing multiple level definitions.</param>
    /// <param name="levelToPurchase">The tower level for which to fetch the preview image.</param>
    /// <returns>The sprite representing the tower at the given level.</returns>
    private Sprite GetShopButtonSprite(TowerData towerData, TowerData.Level levelToPurchase = TowerData.Level.Basic)
    {
        return towerData.GetStructAtLevel(levelToPurchase).PreviewImage;
    }

    /// <summary>
    /// Called when a tower becomes active (clicked). Sets the selected tower 
    /// and shows item info appropriate to upgrading that existing tower.
    /// </summary>
    /// <param name="tower">The tower that was clicked.</param>
    private void OnTowerClicked(Tower tower)
    {
        clickedTower = tower;
        OnShopItemClicked(tower.TowerData, tower.CurrentTowerLevel, true);
    }

    /// <summary>
    /// Displays the details about a tower item (either a new purchase or an upgrade),
    /// and provides "BUY/UP" and "SELL" buttons depending on whether the tower is placed.
    /// </summary>
    /// <param name="towerData">The data defining the tower.</param>
    /// <param name="currentLevel">Which level the tower currently has (for upgrade logic).</param>
    /// <param name="towerIsAlreadyPlaced">If true, means this tower is in the scene; otherwise, it's a fresh purchase.</param>
    private void OnShopItemClicked(TowerData towerData, TowerData.Level currentLevel = TowerData.Level.Basic, bool towerIsAlreadyPlaced = false)
    {
        TowerData.Level levelToPurchase = towerIsAlreadyPlaced ? currentLevel + 1 : currentLevel;
        itemInfoPanel.SetActive(true);
        
        itemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = GetItemInfo(towerData, levelToPurchase);
        
        // Sell button setup
        Button sellButton = itemInfoPanel.GetComponentsInChildren<Button>()[1];
        sellButton.GetComponentInChildren<TextMeshProUGUI>().text = "SELL";
        sellButton.interactable = towerIsAlreadyPlaced;
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(ClickSellButton);

        // Buy/Upgrade button setup
        Button buyOrUpButton = itemInfoPanel.GetComponentsInChildren<Button>()[0];
        buyOrUpButton.GetComponentInChildren<TextMeshProUGUI>().text = towerIsAlreadyPlaced ? "UP" : "BUY";
        buyOrUpButton.GetComponentsInChildren<Image>()[1].sprite = GetShopButtonSprite(towerData, levelToPurchase);
        buyOrUpButton.interactable = CheckPurchaseAvailability(towerData.GetStructAtLevel(levelToPurchase).BasicPrice);

        if (levelToPurchase == TowerData.Level.Undefined) 
            buyOrUpButton.interactable = false;
        
        buyOrUpButton.onClick.RemoveAllListeners();
        if (towerIsAlreadyPlaced)
        {
            buyOrUpButton.onClick.AddListener(ClickUpButton);
        }
        else
        {
            buyOrUpButton.onClick.AddListener(() => ClickBuyButton(towerData));
        }
    }

    /// <summary>
    /// Sells the currently clicked tower, refunds its cost, destroys its game object,
    /// and closes the item info panel.
    /// </summary>
    private void ClickSellButton()
    {
        if (clickedTower)
        {
            EconomyEventBus.EarnMoney(clickedTower.TowerStruct.BasicPrice);
            Destroy(clickedTower.gameObject);
        }
        itemInfoPanel.SetActive(false);
    }

    /// <summary>
    /// Sends an event to upgrade the currently clicked tower, then hides the item info panel.
    /// </summary>
    private void ClickUpButton()
    {
        if (clickedTower) 
            ShopEventBus.UpgradeTower(clickedTower);
        
        itemInfoPanel.SetActive(false);
    }

    /// <summary>
    /// Sends an event to buy a new tower of the specified data type, then closes the item info panel.
    /// </summary>
    /// <param name="towerData">Defines which tower type is being purchased.</param>
    private void ClickBuyButton(TowerData towerData)
    {
        ShopEventBus.BuyTower(towerData);
        itemInfoPanel.SetActive(false);
    }

    /// <summary>
    /// Fills the shop items panel with buttons corresponding to each TowerData in ShopManager's list.
    /// </summary>
    private void FillTheShop()
    {
        foreach (TowerData towerData in ServiceLocator.Get<ShopManager>().shopData.TowersToBuy)
        {
            GameObject newShopItem = Instantiate(buttonPrefab, itemsPanel.transform);
            Image childImage = newShopItem.GetComponentsInChildren<Image>()[1]; // Get the second Image
            childImage.sprite = towerData.GetStructAtLevel(TowerData.Level.Basic).PreviewImage;
            
            Button newButton;
            if ((newButton = newShopItem.GetComponent<Button>()) != null)
            {
                newButton.onClick.AddListener(() => OnShopItemClicked(towerData));
            }
        }
    }

    /// <summary>
    /// Unsubscribes from all events to prevent memory leaks when this object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        ShopEventBus.OnShopOpened -= ActivateShopUI;
        ShopEventBus.OnShopClosed -= DeactivateShopUI;
        EconomyEventBus.OnMoneyAmountChange -= CheckUpgradeAvailability;
        TowerEventBus.OnTowerPlaced -= CheckUpgradeAvailabilityForTower;
        TowerEventBus.OnTowerBecameActive -= OnTowerClicked;
    }
    
    /// <summary>
    /// Iterates over all towers in the scene and updates their 
    /// upgrade indicator appearance based on purchase availability.
    /// </summary>
    /// <param name="enable">If true, indicates upgrade indicators should be shown if affordable;
    /// if false, removes indicators.</param>
    private void EnableUpgradeIndicators(bool enable = true)
    {
        foreach (Tower tower in ServiceLocator.Get<TowerManager>().TowersInScene)
        {
            switch (enable)
            {
                case true:
                    EnableUpgradeIndicatorForTower(
                        tower, 
                        CheckPurchaseAvailability(tower.TowerData.GetStructAtLevel(tower.CurrentTowerLevel + 1).BasicPrice)
                    );
                    break;
                case false:
                    EnableUpgradeIndicatorForTower(tower, false);
                    break;
            }
        }
    }

    /// <summary>
    /// Refreshes upgrade indicators for all towers upon a money change event.
    /// </summary>
    private void CheckUpgradeAvailability()
    {
        EnableUpgradeIndicators();
    }

    /// <summary>
    /// Refreshes the upgrade indicator for a specific tower when it is placed.
    /// </summary>
    /// <param name="tower">The newly placed tower to check for upgrade availability.</param>
    private void CheckUpgradeAvailabilityForTower(Tower tower)
    {
        EnableUpgradeIndicatorForTower(tower);
    }

    /// <summary>
    /// Adds or removes the upgrade outline material on a tower's renderer 
    /// depending on whether upgrades are currently affordable and shop is open.
    /// </summary>
    /// <param name="tower">The tower whose appearance needs updating.</param>
    /// <param name="enable">If true, adds the upgrade material if affordable; otherwise, removes it.</param>
    private void EnableUpgradeIndicatorForTower(Tower tower, bool enable = true)
    {
        Renderer renderer = tower.gameObject.GetComponentInChildren<Renderer>();
        List<Material> materials = new List<Material>(renderer.materials);

        if (enable && tower.CurrentTowerLevel + 1 != TowerData.Level.Undefined && shopPanel.activeInHierarchy)
        {
            // Only add the material if it's not already present.
            if(materials.Count >= 2) return;
            materials.Add(upgradeOutlineMat);
        }
        else
        {
            // Remove any upgrade material if found.
            if(materials.Count <= 1) return;
            materials.Remove(materials[1]);
        }
        renderer.materials = materials.ToArray();
    }

    /// <summary>
    /// Checks if the player has enough money to afford a given price.
    /// </summary>
    /// <param name="price">Cost to check against the current money amount.</param>
    /// <returns>True if the player can afford it, false otherwise.</returns>
    private bool CheckPurchaseAvailability(int price)
    {
        return ServiceLocator.Get<EconomyManager>().GetMoney() >= price;
    }
}
