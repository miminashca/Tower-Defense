using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public GameObject TowerParentObject;
    public Tower TowerPrefab;
    
    [NonSerialized] public GameObject ActiveTower = null;
    [NonSerialized] public List<Tower> TowersInScene;
    public static TowerManager Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    
        TowersInScene = new List<Tower>();
        
        EventBus.OnTowerStartDrag += SetActiveTower;
        EventBus.OnTowerEndDrag += UnsetActiveTower;

        EventBus.OnTowerPlaced += AddTower;
        EventBus.OnTowerRemoved += RemoveTower;
    }

    private void Start()
    {
        foreach (Tower tower in FindObjectsByType<Tower>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            AddTower(tower);
        }
    }

    private void SetActiveTower(GameObject tower)
    {
        ActiveTower = tower;
        EventBus.TowerBecameActive(ActiveTower.GetComponent<Tower>());
    }
    private void UnsetActiveTower(GameObject tower)
    {
        ActiveTower = null;
    }

    private void AddTower(Tower tower)
    {
        if(!TowersInScene.Contains(tower)) TowersInScene.Add(tower);
    }
    private void RemoveTower(Tower tower)
    {
        if(TowersInScene.Contains(tower)) TowersInScene.Remove(tower);
    }
    private void OnDisable()
    {
        EventBus.OnTowerStartDrag -= SetActiveTower;
        EventBus.OnTowerEndDrag -= UnsetActiveTower;
        
        EventBus.OnTowerPlaced -= AddTower;
        EventBus.OnTowerRemoved -= RemoveTower;
    }
}