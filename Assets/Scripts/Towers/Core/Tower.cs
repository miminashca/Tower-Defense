using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerData TowerData;
    public Bullet BulletPrefab;
    public float BulletSpeed;
    
    public bool WasAlreadyPlaced = false;
    public TowerData.Level CurrentTowerLevel { get; private set; }
    public StateMachine SM { get; private set; }
    
    public TowerData.TowerStruct TowerStruct;
    private TowerBehaviour towerBehaviour;

    [NonSerialized] public bool IsActive;

    private void Awake()
    {
        ShopEventBus.OnTowerUpgraded += UpgradeTower;
        ShopEventBus.OnShopOpened += DeactivateTower;
        ShopEventBus.OnShopClosed += ActivateTower;
    
        if(GetComponent<StateMachine>()) SM = GetComponent<StateMachine>();
        else
        {
            Debug.Log("Could not find State Machine in Tower!");
        }
        
        //Initialize tower behaviour
        towerBehaviour = gameObject.AddComponent<TowerBehaviour>();
        
        CurrentTowerLevel = TowerData.Level.Basic;
    }
    private void OnDestroy()
    {
        ShopEventBus.OnTowerUpgraded -= UpgradeTower;
        ShopEventBus.OnShopOpened -= DeactivateTower;
        ShopEventBus.OnShopClosed -= ActivateTower;
        TowerEventBus.RemoveTower(this);
    }
    private void Start()
    {
        InitTowerAtCurrentLevel();
        TowerEventBus.PlaceTower(this);
    }

    private void UpgradeTower(Tower tower)
    {
        if(tower!=this) return;
        CurrentTowerLevel++;
        InitTowerAtCurrentLevel();
        EconomyEventBus.SpendMoney(TowerStruct.BasicPrice);
    }
    
    private void InitTowerAtCurrentLevel()
    {
        Transform oldPrefab = FindChildWithTag(this.transform, "Model");
        if(oldPrefab) Destroy(oldPrefab.gameObject);
        
        TowerStruct = TowerData.GetStructAtLevel(CurrentTowerLevel);
        
        GameObject newModelPrefab = TowerStruct.Model;
        Instantiate(newModelPrefab, transform);
        newModelPrefab.tag = "Model";
        
        towerBehaviour.Initialize(TowerData.towerAttackBehaviourType, TowerData.towerTargetSelectingType, TowerStruct.Range, TowerStruct.Impact, TowerStruct.Threshold);
    }
    
    private Transform FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child; // Return the first matching child
            }
        }
        return null; // No child with this tag found
    }

    private void ActivateTower()
    {
        IsActive = true;
    }
    private void DeactivateTower()
    {
        IsActive = false;
    }

}