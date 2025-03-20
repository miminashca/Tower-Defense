using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The WaveManager class handles spawning enemy waves according to data specified
/// in EnemyWaveData, ensuring enemies appear in the correct spawn points and tracking
/// how many waves have been spawned so far.
/// </summary>
public class WaveManager : MonoBehaviour
{
    /// <summary>
    /// A reference to a spawned instance of the SpawnPointsManager, which holds a collection of spawn points.
    /// </summary>
    private SpawnPointsManager spawnPointsManagerInstance;
    
    /// <summary>
    /// A reference to the transform where enemies should be spawned within the level.
    /// </summary>
    private Transform enemySpawnTransform;

    /// <summary>
    /// The index of the current wave being spawned.
    /// </summary>
    public int CurrentWave;

    /// <summary>
    /// ScriptableObject holding data for each wave, including the total number of waves and 
    /// the composition (type and quantity of enemies) for each wave.
    /// </summary>
    public EnemyWaveData WavesData;
    
    /// <summary>
    /// A prefab for the Enemy object, which gets assigned data before instantiation.
    /// </summary>
    [SerializeField] private Enemy enemyPrefab;
    
    /// <summary>
    /// A prefab for the SpawnPointsManager, responsible for creating and maintaining spawn points in the level.
    /// </summary>
    [SerializeField] private SpawnPointsManager spawnPointsManagerPrefab;
    
    /// <summary>
    /// A prefab for a single spawn point, used by SpawnPointsManager to generate the desired number of points.
    /// </summary>
    [SerializeField] private SpawnPoint pointPrefab;

    /// <summary>
    /// A singleton-like reference for this WaveManager, ensuring it persists across scene loads.
    /// </summary>
    public static WaveManager Instance { get; private set; }

    /// <summary>
    /// Sets up the WaveManager singleton, ensures persistence with DontDestroyOnLoad,
    /// subscribes to manager reload events, and scene loading events.
    /// </summary>
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
            return;
        }

        if (!spawnPointsManagerPrefab) 
            Debug.Log("spawn points manager prefab not set in wave controller");
        if (!enemyPrefab) 
            Debug.Log("enemy prefab not set in wave controller");
        
        GameStateEventBus.OnReloadManagers += Reload;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Unsubscribes from events when this object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        GameStateEventBus.OnReloadManagers -= Reload;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Resets the wave count, typically called when managers are reloaded (e.g., on a game restart).
    /// </summary>
    private void Reload()
    {
        CurrentWave = 0;
    }

    /// <summary>
    /// When a new scene is loaded, if it's Level1, spawns the SpawnPointsManager at the designated spawn transform.
    /// </summary>
    /// <param name="scene">Information about the loaded scene.</param>
    /// <param name="mode">Specifies how the scene was loaded (e.g., Single or Additive).</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
            enemySpawnTransform = GameObject.FindWithTag("EnemySpawn").transform;
            
            spawnPointsManagerInstance = Instantiate(spawnPointsManagerPrefab, 
                enemySpawnTransform.position, 
                enemySpawnTransform.rotation);
            
            spawnPointsManagerInstance.spawnPoint = pointPrefab;
        }
    }

    /// <summary>
    /// Spawns the next wave of enemies if there are any remaining waves. 
    /// Otherwise, signals the WaveEventBus that all waves are completed.
    /// </summary>
    public void SpawnNewWave()
    {
        Debug.Log("Spawn wave");
        if (CurrentWave < WavesData.GetNumberOfWaves())
        {
            if(spawnPointsManagerInstance)
                spawnPointsManagerInstance.Init(WavesData.waves.ElementAt(CurrentWave).number);

            List<Enemy> newEnemiesList = new List<Enemy>();
            SpawnEnemies(
                newEnemiesList, 
                WavesData.waves.ElementAt(CurrentWave).type,
                spawnPointsManagerInstance
            );

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

    /// <summary>
    /// Spawns enemy objects based on the quantity and type specified by the wave data.
    /// Attaches them to the Level1 scene so they properly belong in that gameplay environment.
    /// </summary>
    /// <param name="enemies">A list to which spawned enemies are added.</param>
    /// <param name="enemyData">The data asset containing stats and model information for this wave's enemy type.</param>
    /// <param name="spawnPointManager">Manages the positions at which enemies should spawn.</param>
    private void SpawnEnemies(List<Enemy> enemies, EnemyData enemyData, SpawnPointsManager spawnPointManager)
    {
        if (spawnPointManager.spawnPoints.Count == 0) return;
        
        for (int i = 0; i < spawnPointManager.spawnPoints.Count; i++)
        {
            if (enemyPrefab)
            {
                enemyPrefab.Data = enemyData;
                Enemy enemyInstance = Instantiate(
                    enemyPrefab,
                    spawnPointManager.spawnPoints[i].transform.position,
                    Quaternion.identity
                );
                SceneManager.MoveGameObjectToScene(
                    enemyInstance.gameObject, 
                    SceneManager.GetSceneByName("Level1")
                );
                enemies.Add(enemyInstance);
            }
            else
            {
                Debug.Log("Enemy prefab not set!");
            }
        }
    }

    /// <summary>
    /// Called after a short delay following wave spawn. 
    /// Notifies the WaveEventBus that the current wave has ended 
    /// so that subsequent logic (like shop opening or next wave spawn) can occur.
    /// </summary>
    private void EndWaveInSeconds()
    {
        WaveEventBus.EndWave(CurrentWave);
    }
}
