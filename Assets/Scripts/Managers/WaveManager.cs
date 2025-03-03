using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int currentWave = 0;
    private EnemySpawner enemySpawner;
    private SpawnPointsManager spawnPointsManagerInstance;

    public EnemyWaveData wavesData;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private SpawnPointsManager spawnPointsManagerPrefab;
    [SerializeField] private SpawnPoint pointPrefab;
    [SerializeField] private Transform enemySpawnTransform;

    private void Awake()
    {
        if (!spawnPointsManagerPrefab) Debug.Log("spawn points manager prefab not set in wave controller");
        if (!enemyPrefab) Debug.Log("enemy prefab not set in wave controller");
        enemySpawner = ScriptableObject.CreateInstance<EnemySpawner>();
        EventBus.OnShopClosed += SpawnNewWave;
    }
    private void Start()
    {
        spawnPointsManagerInstance = Instantiate(spawnPointsManagerPrefab, enemySpawnTransform.position, enemySpawnTransform.rotation);
        spawnPointsManagerInstance.spawnPoint = pointPrefab; //
        enemySpawner.enemyPrefab = enemyPrefab;
    }
    
    public void SpawnNewWave()
    {
        if (currentWave < wavesData.GetNumberOfWaves())
        {
            EventBus.StartWave(currentWave+1);
            
            spawnPointsManagerInstance.Init(wavesData.waves.ElementAt(currentWave).number);

            List<Enemy> newEnemiesList = new List<Enemy>();
            enemySpawner.SpawnEnemies(newEnemiesList, wavesData.waves.ElementAt(currentWave).type,
                spawnPointsManagerInstance);

            GameManager.enemyManager.AddEnemies(newEnemiesList);
            currentWave++;

            Invoke("EndWaveInSeconds", wavesData.tresholdBetweenWaves);
        }
    }
    
    private void EndWaveInSeconds()
    {
        EventBus.EndWave(currentWave+1);
    }

    private void OnDestroy()
    {
        EventBus.OnShopClosed -= SpawnNewWave;
    }
}
