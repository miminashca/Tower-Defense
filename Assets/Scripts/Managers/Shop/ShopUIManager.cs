using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject itemsPanel;
    [SerializeField] private GameObject itemInfoPanel;
    [SerializeField] private Material upgradeOutlineMat;

    //private Button currentButton;
    public Tower clickedTower = null;

    private void Awake()
    {
        EventBus.OnShopOpened += ActivateShopUI;
        EventBus.OnShopClosed += DeactivateShopUI;
        EventBus.OnMoneyAmountChange += CheckUpgradeAvailability;
        EventBus.OnTowerBecameActive += OnTowerClicked;
    }

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
    private void ActivateShopUI()
    {
        shopPanel.SetActive(true);
        EnableUpgradeIndicators();
    }
    private void DeactivateShopUI()
    {
        EnableUpgradeIndicators(false);
        itemInfoPanel.SetActive(false);
        shopPanel.SetActive(false);
    }
    private string GetItemInfo(TowerData towerData, TowerData.Level levelToPurchase = TowerData.Level.Basic)
    {
        TowerData.TowerStruct selectedTowerStruct = towerData.GetStructAtLevel(levelToPurchase);
        return $"Price: {selectedTowerStruct.BasicPrice}\n" +
               $"Type: {towerData.GetTargetSelectingType().ToString()} {towerData.GetImpactType().ToString()}\n" +
               $"Range: {selectedTowerStruct.Range}\n" +
               $"Impact: {selectedTowerStruct.Impact}\n" +
               $"Threshold: {selectedTowerStruct.Threshold}";
    }
    private Sprite GetShopButtonSprite(TowerData towerData, TowerData.Level levelToPurchase = TowerData.Level.Basic)
    {
        return towerData.GetStructAtLevel(levelToPurchase).PreviewImage;
    }

    private void OnTowerClicked(Tower tower)
    {
        clickedTower = tower;
        OnShopItemClicked(tower.towerData, tower.currentTowerLevel, true);
        
        // if (tower.currentTowerLevel + 1 == TowerData.Level.Undefined)
        // {
        //     itemInfoPanel.SetActive(false);
        //     return;
        // }
        //EnableUpgradeIndicatorForTower(tower, CheckPurchaseAvailability(tower.towerData.GetStructAtLevel(tower.currentTowerLevel+1).BasicPrice));
        
    }
    private void OnShopItemClicked(TowerData towerData, TowerData.Level currentLevel = TowerData.Level.Basic, bool towerIsAlreadyPlaced = false)
    {
        TowerData.Level levelToPurchase = towerIsAlreadyPlaced ? currentLevel + 1 : currentLevel;
        itemInfoPanel.SetActive(true);
        
        itemInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = GetItemInfo(towerData, levelToPurchase);
        
        Button sellButton = itemInfoPanel.GetComponentsInChildren<Button>()[1];
        sellButton.GetComponentInChildren<TextMeshProUGUI>().text = "SELL";
        if (towerIsAlreadyPlaced) sellButton.interactable = true;
        else sellButton.interactable = false;
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(ClickSellButton);

        Button buyOrUpButton = itemInfoPanel.GetComponentsInChildren<Button>()[0];
        buyOrUpButton.GetComponentInChildren<TextMeshProUGUI>().text = towerIsAlreadyPlaced ? "UP" : "BUY";
        buyOrUpButton.GetComponentsInChildren<Image>()[1].sprite = GetShopButtonSprite(towerData, levelToPurchase);
        buyOrUpButton.interactable = CheckPurchaseAvailability(towerData.GetStructAtLevel(levelToPurchase).BasicPrice);
        if (levelToPurchase == TowerData.Level.Undefined) buyOrUpButton.interactable = false;
        buyOrUpButton.onClick.RemoveAllListeners();
        if(towerIsAlreadyPlaced) buyOrUpButton.onClick.AddListener(ClickUpButton);
        else
        {
            buyOrUpButton.onClick.AddListener(() => ClickBuyButton(towerData));
        }
    }

    private void ClickSellButton()
    {
        if(clickedTower) Destroy(clickedTower.gameObject);
        itemInfoPanel.SetActive(false);
    }
    private void ClickUpButton()
    {
        if (clickedTower) EventBus.UpgradeTower(clickedTower);
        itemInfoPanel.SetActive(false);
    }
    private void ClickBuyButton(TowerData towerData)
    {
        EventBus.BuyTower(towerData);
        itemInfoPanel.SetActive(false);
    }
    private void FillTheShop()
    {
        foreach (TowerData towerData in GameManager.shopManager.shopData.towersToBuy)
        {
            GameObject newShopItem = Instantiate(buttonPrefab, itemsPanel.transform);
            Image childImage = newShopItem.GetComponentsInChildren<Image>()[1]; // Gets the second Image (assuming 0 is parent)
            childImage.sprite = towerData.GetStructAtLevel(TowerData.Level.Basic).PreviewImage;
            Button newButton;
            if ((newButton = newShopItem.GetComponent<Button>()) != null)
            {
                newButton.onClick.AddListener(() => OnShopItemClicked(towerData));
            }
        }
    }

    private void OnDestroy()
    {
        EventBus.OnShopOpened -= ActivateShopUI;
        EventBus.OnShopClosed -= DeactivateShopUI;
        EventBus.OnMoneyAmountChange -= CheckUpgradeAvailability;
        EventBus.OnTowerBecameActive -= OnTowerClicked;
    }
    
    private void EnableUpgradeIndicators(bool enable = true)
    {
        foreach (Tower tower in GameManager.towerManager.towersInScene)
        {
            switch (enable)
            {
                case true:
                    EnableUpgradeIndicatorForTower(tower, CheckPurchaseAvailability(tower.towerData.GetStructAtLevel(tower.currentTowerLevel+1).BasicPrice));
                    break;
                case false:
                    EnableUpgradeIndicatorForTower(tower, false);
                    break;
            }
        }
    }
    private void CheckUpgradeAvailability()
    {
        EnableUpgradeIndicators();
    }
    private void EnableUpgradeIndicatorForTower(Tower tower, bool enable = true)
    {
        Renderer renderer = tower.gameObject.GetComponentInChildren<Renderer>();
        List<Material> materials = new List<Material>(renderer.materials);
        if (enable && tower.currentTowerLevel + 1 != TowerData.Level.Undefined && shopPanel.activeInHierarchy)
        {
            if(materials.Count>=2) return;
            materials.Add(upgradeOutlineMat);
        }
        else
        {
            if(materials.Count<=1) return;
            materials.Remove(materials[1]);
        }
        renderer.materials = materials.ToArray();
    }

    private bool CheckPurchaseAvailability(int price)
    {
        return GameManager.gameManager.GetMoney() >= price;
    }
}
