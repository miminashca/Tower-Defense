using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerData towerData;
    public Bullet bulletPrefab;
    public float bulletSpeed;
    public TowerData.TowerStruct towerStruct { get; private set; }
    public TowerData.Level currentTowerLevel { get; private set; }
    private TowerBehaviour towerBehaviour;

    public StateMachine SM { get; private set; }
    [NonSerialized] public bool isActive;

    private void Awake()
    {
        EventBus.OnTowerUpgraded += UpgradeTower;
        EventBus.OnShopOpened += DeactivateTower;
        EventBus.OnShopClosed += ActivateTower;
    }

    private void OnEnable()
    {
        if(GetComponent<StateMachine>()) SM = GetComponent<StateMachine>();
        else
        {
            Debug.Log("Could not find State Machine in Tower!");
        }
        
        towerBehaviour = gameObject.AddComponent<TowerBehaviour>();
        towerBehaviour.impactType = towerData.towerAttackBehaviourType;
        towerBehaviour.targetSelectingType = towerData.towerTargetSelectingType;

        towerBehaviour.bulletPrefab = bulletPrefab;
        towerBehaviour.bulletSpeed = bulletSpeed;
        
        currentTowerLevel = TowerData.Level.Basic;
    }

    private void Start()
    {
        InitTowerAtCurrentLevel();
        EventBus.PlaceTower(this);
    }

    private void UpgradeTower(Tower tower)
    {
        if(tower!=this) return;
        currentTowerLevel++;
        InitTowerAtCurrentLevel();
        EventBus.SpendMoney(towerStruct.BasicPrice);
    }
    private void InitTowerAtCurrentLevel()
    {
        Transform oldPrefab = FindChildWithTag(this.transform, "Model");
        if(oldPrefab) Destroy(oldPrefab.gameObject);
        
        towerStruct = towerData.GetStructAtLevel(currentTowerLevel);
        
        GameObject newModelPrefab = towerStruct.Model;
        Instantiate(newModelPrefab, transform);
        newModelPrefab.tag = "Model";
        
        towerBehaviour.Initialize(towerStruct.Range, towerStruct.Impact, towerStruct.Threshold);
    }

    private void OnDestroy()
    {
        EventBus.EarnMoney(towerStruct.BasicPrice);
        EventBus.OnTowerUpgraded -= UpgradeTower;
        EventBus.OnShopOpened -= DeactivateTower;
        EventBus.OnShopClosed -= ActivateTower;
        EventBus.RemoveTower(this);
    }
    
    public Transform FindChildWithTag(Transform parent, string tag)
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
        isActive = true;
    }
    private void DeactivateTower()
    {
        isActive = false;
    }

}