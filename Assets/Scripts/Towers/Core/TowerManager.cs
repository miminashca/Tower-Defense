using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public GameObject towerParentObject;
    public Tower towerPrefab;
    public GameObject activeTower = null;

    public List<Tower> towersInScene;

    private void Awake()
    {
        towersInScene = new List<Tower>();
        
        EventBus.OnTowerStartDrag += setActiveTower;
        EventBus.OnTowerEndDrag += unsetActiveTower;

        EventBus.OnTowerPlaced += AddTower;
        EventBus.OnTowerRemoved += RemoveTower;
    }

    private void setActiveTower(GameObject tower)
    {
        activeTower = tower;
        EventBus.TowerBecameActive(activeTower.GetComponent<Tower>());
    }
    private void unsetActiveTower(GameObject tower)
    {
        activeTower = null;
    }

    private void AddTower(Tower tower)
    {
        towersInScene.Add(tower);
    }
    private void RemoveTower(Tower tower)
    {
        towersInScene.Remove(tower);
    }
    private void OnDisable()
    {
        EventBus.OnTowerStartDrag -= setActiveTower;
        EventBus.OnTowerEndDrag -= unsetActiveTower;
        
        EventBus.OnTowerPlaced -= AddTower;
        EventBus.OnTowerRemoved -= RemoveTower;
    }
}