using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The TowerManager class tracks all towers in the scene, handles 
/// active tower selection, and responds to tower placement and removal events.
/// It also supports reloading manager state upon game resets.
/// </summary>
public class TowerManager : Manager<TowerManager>
{
    /// <summary>
    /// A reference to the tower prefab used when spawning or buying new towers.
    /// </summary>
    public Tower TowerPrefab;
    
    /// <summary>
    /// The parent GameObject under which all towers are organized in the scene.
    /// Typically set upon loading Level1.
    /// </summary>
    [NonSerialized] public GameObject TowerParentObject;
    
    /// <summary>
    /// The currently active tower (e.g., one that is being dragged).
    /// </summary>
    [NonSerialized] public GameObject ActiveTower = null;
    
    /// <summary>
    /// A list of all Tower instances currently in the scene.
    /// </summary>
    [NonSerialized] public List<Tower> TowersInScene;

    /// <summary>
    /// Subscribes to tower-related events, and initializes the manager's data structures.
    /// </summary>
    protected override void Awake() 
    {
        base.Awake();
        TowersInScene = new List<Tower>();
        
        TowerEventBus.OnTowerStartDrag += SetActiveTower;
        TowerEventBus.OnTowerEndDrag += UnsetActiveTower;

        TowerEventBus.OnTowerPlaced += AddTower;
        TowerEventBus.OnTowerRemoved += RemoveTower;
    }
    
    /// <summary>
    /// Resets the TowersInScene list when managers are reloaded (e.g., on a game restart).
    /// </summary>
    protected override void Reload()
    {
        if (TowersInScene == null) 
            TowersInScene = new List<Tower>();
        else 
            TowersInScene.Clear();
    }

    /// <summary>
    /// Called when a new scene is loaded.
    /// </summary>
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        base.OnSceneLoaded(scene, mode);
        
        TowerParentObject = GameObject.FindWithTag("TowerParent");
        
        // Automatically discover and add existing Tower objects in the loaded scene.
        foreach (Tower tower in FindObjectsByType<Tower>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            AddTower(tower);
        }
    }

    /// <summary>
    /// Sets the given tower GameObject as active, and invokes an event to mark the 
    /// corresponding Tower component as active as well.
    /// </summary>
    /// <param name="tower">The tower GameObject to set as active.</param>
    private void SetActiveTower(GameObject tower)
    {
        ActiveTower = tower;
        TowerEventBus.TowerBecameActive(ActiveTower.GetComponent<Tower>());
    }

    /// <summary>
    /// Unsets the active tower reference when a tower finishes being dragged.
    /// </summary>
    /// <param name="tower">The tower GameObject that ended dragging.</param>
    private void UnsetActiveTower(GameObject tower)
    {
        ActiveTower = null;
    }

    /// <summary>
    /// Registers a new Tower instance with the manager's tracking list.
    /// </summary>
    /// <param name="tower">The tower to add to the list.</param>
    private void AddTower(Tower tower)
    {
        if(TowersInScene!=null && !TowersInScene.Contains(tower)) 
            TowersInScene.Add(tower);
    }

    /// <summary>
    /// Removes a Tower instance from the manager's tracking list.
    /// </summary>
    /// <param name="tower">The tower to remove from the list.</param>
    private void RemoveTower(Tower tower)
    {
        if(TowersInScene.Contains(tower)) 
            TowersInScene.Remove(tower);
    }

    /// <summary>
    /// Unsubscribes from all tower and game state events when this manager is disabled.
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        TowerEventBus.OnTowerStartDrag -= SetActiveTower;
        TowerEventBus.OnTowerEndDrag -= UnsetActiveTower;
        
        TowerEventBus.OnTowerPlaced -= AddTower;
        TowerEventBus.OnTowerRemoved -= RemoveTower;
    }
}
