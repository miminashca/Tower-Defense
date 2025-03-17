using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int currentWave = 0;
    private SpawnPointsManager spawnPointsManagerInstance;

    public EnemyWaveData wavesData;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private SpawnPointsManager spawnPointsManagerPrefab;
    [SerializeField] private SpawnPoint pointPrefab;
    [SerializeField] private Transform enemySpawnTransform;

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
        EventBus.OnShopClosed += SpawnNewWave;
    }
    private void Start()
    {
        spawnPointsManagerInstance = Instantiate(spawnPointsManagerPrefab, enemySpawnTransform.position, enemySpawnTransform.rotation);
        spawnPointsManagerInstance.spawnPoint = pointPrefab; 
    }
    
    public void SpawnNewWave()
    {
        if (currentWave < wavesData.GetNumberOfWaves())
        {
            WaveEventBus.StartWave(currentWave+1);
            
            spawnPointsManagerInstance.Init(wavesData.waves.ElementAt(currentWave).number);

            List<Enemy> newEnemiesList = new List<Enemy>();
            SpawnEnemies(newEnemiesList, wavesData.waves.ElementAt(currentWave).type,
                spawnPointsManagerInstance);

            EnemyManager.Instance.AddEnemies(newEnemiesList);
            currentWave++;

            Invoke("EndWaveInSeconds", wavesData.tresholdBetweenWaves);
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
        WaveEventBus.EndWave(currentWave+1);
    }

    private void OnDestroy()
    {
        EventBus.OnShopClosed -= SpawnNewWave;
    }
}
