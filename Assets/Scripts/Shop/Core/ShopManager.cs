using UnityEngine;
using UnityEngine.Serialization;

public class ShopManager : MonoBehaviour
{
    public ShopData shopData;
    public bool ShopIsOpen { get; private set; } = false;
    public static ShopManager Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
   
        EventBus.OnTowerBought += BuyTower;
    }

    private void Start()
    {
        if(!shopData) Debug.Log("Shop data not set in shop manager!");
    }

    public void ActivateShop()
    { 
        ShopIsOpen = true;
        Debug.Log("open shop");
        EventBus.OpenShop();
        //Invoke("DeactivateShop", shopData.shopDuration);
    }
    public void DeactivateShop()
    {
        CancelInvoke(nameof(DeactivateShop));
        ShopIsOpen = false;
        Debug.Log("close shop");
        EventBus.CloseShop();
    }

    public void CheckGameLoop()
    {
        GameManager.Instance.GameLoop();
    }
    
    private void BuyTower(TowerData towerData)
    {
        EventBus.SpendMoney(towerData.GetStructAtLevel(TowerData.Level.Basic).BasicPrice);
        Tower newTower = Instantiate(TowerManager.Instance.TowerPrefab, Vector3.zero, Quaternion.identity, TowerManager.Instance.TowerParentObject.transform);
        newTower.TowerData = towerData;
        newTower.SM.TransitToState(new DraggingShopStateTower(newTower.SM));
    }

    private void OnDestroy()
    {
        EventBus.OnTowerBought -= BuyTower;
    }
}
