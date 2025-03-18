using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopData shopData;
    public bool shopIsOpen = false;

    public static ShopManager Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
   
        WaveEventBus.OnWaveEnd += ActivateShop;
        EventBus.OnTowerBought += InstantiateTower;
    }

    private void Start()
    {
        if(!shopData) Debug.Log("Shop data not set in shop manager!");
        ActivateShop(1);
    }

    private void ActivateShop(int currentWave)
    {
        if (currentWave <= WaveManager.Instance.wavesData.GetNumberOfWaves())
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
        Tower newTower = Instantiate(TowerManager.Instance.TowerPrefab, Vector3.zero, Quaternion.identity, TowerManager.Instance.TowerParentObject.transform);
        newTower.TowerData = towerData;
        newTower.SM.TransitToState(new DraggingShopStateTower(newTower.SM));
    }

    private void OnDestroy()
    {
        WaveEventBus.OnWaveEnd -= ActivateShop;
        EventBus.OnTowerBought -= InstantiateTower;
    }
}
