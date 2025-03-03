using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ShopManager : MonoBehaviour
{
    public ShopData shopData;
    public bool shopIsOpen = false;

    private void Awake()
    {
        EventBus.OnWaveEnd += ActivateShop;
        EventBus.OnTowerBought += InstantiateTower;
    }

    private void Start()
    {
        if(!shopData) Debug.Log("Shop data not set in shop manager!");
        ActivateShop(1);
    }

    private void ActivateShop(int currentWave)
    {
        if (currentWave <= GameManager.waveManager.wavesData.GetNumberOfWaves())
        {
            shopIsOpen = true;
            Debug.Log("open shop");
            EventBus.OpenShop();
            Invoke("DeactivateShop", shopData.shopDuration);
        }
    }
    public void DeactivateShop()
    {
        CancelInvoke(nameof(DeactivateShop));
        shopIsOpen = false;
        Debug.Log("close shop");
        EventBus.CloseShop();
    }
    
    private void InstantiateTower(TowerData towerData)
    {
        EventBus.SpendMoney(towerData.GetStructAtLevel(TowerData.Level.Basic).BasicPrice);
        Tower newTower = Instantiate(GameManager.towerManager.towerPrefab, Vector3.zero, Quaternion.identity, GameManager.towerManager.towerParentObject.transform);
        newTower.towerData = towerData;
        newTower.SM.TransitToState(newTower.GetComponent<DraggingStateTower>());
    }

    private void OnDestroy()
    {
        EventBus.OnWaveEnd -= ActivateShop;
        EventBus.OnTowerBought -= InstantiateTower;
    }
}
