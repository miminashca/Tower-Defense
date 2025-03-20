using System;
using UnityEngine;

/// <summary>
/// The Tower class represents a placed tower in the game, handling its current level, behavior, 
/// upgrades, and active state. It also subscribes to shop events to enable or disable tower 
/// functionality during shop phases.
/// </summary>
public class Tower : MonoBehaviour
{
    /// <summary>
    /// Holds the data for this tower, including model, range, and impact at different levels.
    /// </summary>
    public TowerData TowerData;
    
    /// <summary>
    /// A reference to the bullet prefab this tower may use (if needed by certain tower behaviors).
    /// </summary>
    public Bullet BulletPrefab;
    
    /// <summary>
    /// The current upgrade level of this tower (e.g., Basic, Upgrade1, etc.).
    /// </summary>
    public TowerData.Level CurrentTowerLevel { get; private set; }
    
    /// <summary>
    /// A reference to the tower's StateMachine, if present, controlling tower state transitions.
    /// </summary>
    public StateMachine SM { get; private set; }
    
    /// <summary>
    /// Stores the tower's stats (range, impact, threshold, etc.) for the current level.
    /// </summary>
    public TowerData.TowerStruct TowerStruct;
    
    /// <summary>
    /// The tower's behavior script that handles attacking or targeting logic.
    /// </summary>
    private TowerBehaviour towerBehaviour;

    /// <summary>
    /// Indicates whether the tower is currently active in gameplay (e.g., attacking) 
    /// or deactivated (e.g., during shop phase).
    /// </summary>
    [NonSerialized] public bool IsActive;

    /// <summary>
    /// Subscribes to upgrade and shop open/close events, configures the tower's state machine, 
    /// and adds a TowerBehaviour script.
    /// </summary>
    private void Awake()
    {
        ShopEventBus.OnTowerUpgraded += UpgradeTower;
        ShopEventBus.OnShopOpened += DeactivateTower;
        ShopEventBus.OnShopClosed += ActivateTower;
    
        if(GetComponent<StateMachine>()) 
            SM = GetComponent<StateMachine>();
        else
        {
            Debug.Log("Could not find State Machine in Tower!");
        }
        
        // Initialize tower behaviour
        towerBehaviour = gameObject.AddComponent<TowerBehaviour>();
        
        CurrentTowerLevel = TowerData.Level.Basic;
    }

    /// <summary>
    /// Unsubscribes from events when the tower is destroyed and 
    /// removes itself from the TowerEventBus.
    /// </summary>
    private void OnDestroy()
    {
        ShopEventBus.OnTowerUpgraded -= UpgradeTower;
        ShopEventBus.OnShopOpened -= DeactivateTower;
        ShopEventBus.OnShopClosed -= ActivateTower;
        TowerEventBus.RemoveTower(this);
    }

    /// <summary>
    /// Called after Awake. Sets up the tower at its current level 
    /// and signals that the tower has been placed.
    /// </summary>
    private void Start()
    {
        InitTowerAtCurrentLevel();
        TowerEventBus.PlaceTower(this);
    }

    /// <summary>
    /// Upgrades this tower to the next level if it matches the tower that triggered the event.
    /// After upgrading, it updates the tower's stats and deducts the upgrade cost.
    /// </summary>
    /// <param name="tower">The tower being upgraded, checked against 'this'.</param>
    private void UpgradeTower(Tower tower)
    {
        if(tower != this) return;
        CurrentTowerLevel++;
        InitTowerAtCurrentLevel();
        EconomyEventBus.SpendMoney(TowerStruct.BasicPrice);
    }

    /// <summary>
    /// Initializes the tower to match its current level data, 
    /// updating the model and configuring the TowerBehaviour accordingly.
    /// </summary>
    private void InitTowerAtCurrentLevel()
    {
        Transform oldPrefab = FindChildWithTag(this.transform, "Model");
        if(oldPrefab) Destroy(oldPrefab.gameObject);
        
        TowerStruct = TowerData.GetStructAtLevel(CurrentTowerLevel);
        
        GameObject newModelPrefab = TowerStruct.Model;
        Instantiate(newModelPrefab, transform);
        newModelPrefab.tag = "Model";
        
        towerBehaviour.Initialize(
            TowerData.towerAttackBehaviourType, 
            TowerData.towerTargetSelectingType, 
            TowerStruct.Range, 
            TowerStruct.Impact, 
            TowerStruct.Threshold
        );
    }

    /// <summary>
    /// Searches the children of a specified Transform for one with the given tag.
    /// Returns the first match or null if no match is found.
    /// </summary>
    /// <param name="parent">The Transform whose children are searched.</param>
    /// <param name="tag">The tag to look for.</param>
    /// <returns>A Transform that matches the given tag, or null if none is found.</returns>
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

    /// <summary>
    /// Called when the shop closes, marking the tower as active for gameplay.
    /// </summary>
    private void ActivateTower()
    {
        IsActive = true;
    }

    /// <summary>
    /// Called when the shop opens, marking the tower as inactive 
    /// so it does not attack or function during the shop phase.
    /// </summary>
    private void DeactivateTower()
    {
        IsActive = false;
    }
}
