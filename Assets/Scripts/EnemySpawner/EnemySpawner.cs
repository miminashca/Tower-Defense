using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : ScriptableObject
{
    public Enemy enemyPrefab;
    private SpawnPointsManager spawnPointManager;
    public void SpawnEnemies(List<Enemy> enemies, EnemyData enemyData, SpawnPointsManager pSpawnPointManager)
    {
        spawnPointManager = pSpawnPointManager;
        if(!spawnPointManager || spawnPointManager.spawnPoints==null || spawnPointManager.spawnPoints.Count==0) return;
        
        //Debug.Log(spawnPointManager.spawnPoints.Count);
        for (int i = 0; i < spawnPointManager.spawnPoints.Count; i++)
        {
            if (enemyPrefab)
            {
                enemyPrefab.data = enemyData;
                Enemy enemyInstance = Instantiate(enemyPrefab, spawnPointManager.spawnPoints[i].transform.position, Quaternion.identity);
                enemies.Add(enemyInstance); // Add enemy to the list
            }
            else
            {
                Debug.Log("Enemy prefab not set!");
            }
        }
    }
}
