using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerManager : MonoBehaviour
{
    public Tower TowerPrefab;
    
    [NonSerialized] public GameObject TowerParentObject;
    [NonSerialized] public GameObject ActiveTower = null;
    [NonSerialized] public List<Tower> TowersInScene;
    public static TowerManager Instance { get; private set; }
    private void Awake() 
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        TowerEventBus.OnTowerStartDrag += SetActiveTower;
        TowerEventBus.OnTowerEndDrag += UnsetActiveTower;

        TowerEventBus.OnTowerPlaced += AddTower;
        TowerEventBus.OnTowerRemoved += RemoveTower;
        
        GameStateEventBus.OnReloadManagers += Reload;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void Reload()
    {
        if (TowersInScene == null) TowersInScene = new List<Tower>();
        else TowersInScene.Clear();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
            TowerParentObject = GameObject.FindWithTag("TowerParent");
            
            foreach (Tower tower in FindObjectsByType<Tower>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                AddTower(tower);
            }
        }
    }

    private void SetActiveTower(GameObject tower)
    {
        ActiveTower = tower;
        TowerEventBus.TowerBecameActive(ActiveTower.GetComponent<Tower>());
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
        TowerEventBus.OnTowerStartDrag -= SetActiveTower;
        TowerEventBus.OnTowerEndDrag -= UnsetActiveTower;
        
        TowerEventBus.OnTowerPlaced -= AddTower;
        TowerEventBus.OnTowerRemoved -= RemoveTower;
        
        GameStateEventBus.OnReloadManagers -= Reload;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}