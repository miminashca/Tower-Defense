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
    
    private TowerData.TowerStruct towerStruct;
    private TowerBehaviour towerBehaviour;

    [NonSerialized] public bool IsActive;

    private void Awake()
    {
        EventBus.OnTowerUpgraded += UpgradeTower;
        EventBus.OnShopOpened += DeactivateTower;
        EventBus.OnShopClosed += ActivateTower;
    
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
        EventBus.EarnMoney(towerStruct.BasicPrice);
        EventBus.OnTowerUpgraded -= UpgradeTower;
        EventBus.OnShopOpened -= DeactivateTower;
        EventBus.OnShopClosed -= ActivateTower;
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
        EventBus.SpendMoney(towerStruct.BasicPrice);
    }
    
    private void InitTowerAtCurrentLevel()
    {
        Transform oldPrefab = FindChildWithTag(this.transform, "Model");
        if(oldPrefab) Destroy(oldPrefab.gameObject);
        
        towerStruct = TowerData.GetStructAtLevel(CurrentTowerLevel);
        
        GameObject newModelPrefab = towerStruct.Model;
        Instantiate(newModelPrefab, transform);
        newModelPrefab.tag = "Model";
        
        towerBehaviour.Initialize(TowerData.towerAttackBehaviourType, TowerData.towerTargetSelectingType, towerStruct.Range, towerStruct.Impact, towerStruct.Threshold);
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