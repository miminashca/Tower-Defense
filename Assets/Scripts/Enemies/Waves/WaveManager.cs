using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class WaveManager : MonoBehaviour
{ 
    private SpawnPointsManager spawnPointsManagerInstance;
    private Transform enemySpawnTransform;

    public int CurrentWave;
    public EnemyWaveData WavesData;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private SpawnPointsManager spawnPointsManagerPrefab;
    [SerializeField] private SpawnPoint pointPrefab;

    public static WaveManager Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
        if (!spawnPointsManagerPrefab) Debug.Log("spawn points manager prefab not set in wave controller");
        if (!enemyPrefab) Debug.Log("enemy prefab not set in wave controller");
        
        GameStateEventBus.OnReloadManagers += Reload;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        GameStateEventBus.OnReloadManagers -= Reload;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Reload()
    {
        CurrentWave = 0;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
            enemySpawnTransform = GameObject.FindWithTag("EnemySpawn").transform;
            
            spawnPointsManagerInstance = Instantiate(spawnPointsManagerPrefab, enemySpawnTransform.position, enemySpawnTransform.rotation);
            spawnPointsManagerInstance.spawnPoint = pointPrefab;
        }
    }
    
    public void SpawnNewWave()
    {
        Debug.Log("Spawn wave");
        if (CurrentWave < WavesData.GetNumberOfWaves())
        {
            if(spawnPointsManagerInstance)spawnPointsManagerInstance.Init(WavesData.waves.ElementAt(CurrentWave).number);

            List<Enemy> newEnemiesList = new List<Enemy>();
            SpawnEnemies(newEnemiesList, WavesData.waves.ElementAt(CurrentWave).type,
                spawnPointsManagerInstance);

            EnemyManager.Instance.AddEnemies(newEnemiesList);
            
            CurrentWave++;
            WaveEventBus.StartWave(CurrentWave);

            Invoke("EndWaveInSeconds", WavesData.tresholdBetweenWaves);
        }
        else
        {
            WaveEventBus.WavesCompleted();
        }
    }
    private void SpawnEnemies(List<Enemy> enemies, EnemyData enemyData, SpawnPointsManager spawnPointManager)
    {
        if(spawnPointManager.spawnPoints.Count==0) return;
        
        for (int i = 0; i < spawnPointManager.spawnPoints.Count; i++)
        {
            if (enemyPrefab)
            {
                enemyPrefab.Data = enemyData;
                Enemy enemyInstance = Instantiate(enemyPrefab, spawnPointManager.spawnPoints[i].transform.position, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(enemyInstance.gameObject, SceneManager.GetSceneByName("Level1"));
                enemies.Add(enemyInstance); // Add enemy to the list
            }
            else
            {
                Debug.Log("Enemy prefab not set!");
            }
        }
    }
    
    private void EndWaveInSeconds()
    {
        WaveEventBus.EndWave(CurrentWave);
    }
}
