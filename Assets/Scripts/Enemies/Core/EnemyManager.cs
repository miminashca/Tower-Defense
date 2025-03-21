using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages all enemies in the scene by tracking their state, spawning them, 
/// and monitoring how many reach the finish point. This class also handles 
/// in-game events such as enemy death, adding money when enemies die, 
/// and triggering the lose condition if too many reach the goal.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    /// <summary>
    /// The maximum number of enemies allowed to reach the goal before 
    /// triggering a loss condition.
    /// </summary>
    [SerializeField] private int maxEnemiesAllowedAtTarget = 5;
    
    /// <summary>
    /// A coin prefab to spawn at the enemy's position upon death 
    /// to visually represent earned money.
    /// </summary>
    [SerializeField] private GameObject coinPrefab;
    
    /// <summary>
    /// The finish point's position to which enemies move.
    /// </summary>
    private Vector3 TargetPosition;
    
    /// <summary>
    /// A list of all active enemy instances currently tracked by the manager.
    /// </summary>
    private List<Enemy> mainEnemiesList;
    
    /// <summary>
    /// Tracks how many enemies have reached the goal area.
    /// </summary>
    private int currentEnemiesAtGoal;
    
    /// <summary>
    /// Singleton-like reference for the EnemyManager, accessible project-wide.
    /// </summary>
    public static EnemyManager Instance { get; private set; }

    /// <summary>
    /// Sets up the EnemyManager singleton, subscribes to relevant events, 
    /// and configures the mainEnemiesList.
    /// </summary>
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        WaveEventBus.OnWavesCompleted += CheckGameFinalState;
        
        EnemyEventBus.OnEnemyDeath += OnEnemyDie;
        EnemyEventBus.OnEnemyReachedTarget += EnemyReachedTarget;

        GameStateEventBus.OnReloadManagers += Reload;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Resets or clears tracked enemies and resets the counter of enemies at the goal.
    /// Invoked when managers are reloaded.
    /// </summary>
    private void Reload()
    {
        if (mainEnemiesList == null)
        {
            mainEnemiesList = new List<Enemy>();
        }
        else
        {
            mainEnemiesList.Clear();
        }

        currentEnemiesAtGoal = 0;
    }

    /// <summary>
    /// Called when a new scene is loaded. If the loaded scene is Level1,
    /// it locates and stores the transform for the finish point used by enemies.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Bootstrap") return;

        TargetPosition = GameObject.FindWithTag("FinishTransform").transform.position;
    }

    /// <summary>
    /// Cleans up event subscriptions when the EnemyManager is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        WaveEventBus.OnWavesCompleted -= CheckGameFinalState;
        EnemyEventBus.OnEnemyDeath -= OnEnemyDie;
        EnemyEventBus.OnEnemyReachedTarget -= EnemyReachedTarget;
        
        GameStateEventBus.OnReloadManagers -= Reload;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    /// <summary>
    /// Adds newly spawned enemies to the manager's tracking list,
    /// and triggers an event so they start moving towards the target position.
    /// </summary>
    /// <param name="enemiesToAdd">A list of Enemy objects to register.</param>
    public void AddEnemies(List<Enemy> enemiesToAdd)
    {
        foreach (Enemy enemy in enemiesToAdd)
        {
            if (mainEnemiesList!=null && !mainEnemiesList.Contains(enemy)) 
                mainEnemiesList.Add(enemy);
        }
        EnemyEventBus.EnemiesStartMoveToPosition(TargetPosition);
    }

    /// <summary>
    /// Invoked when an enemy dies. Awards the player money, spawns a coin prefab,
    /// and removes the enemy from the tracking list.
    /// </summary>
    /// <param name="enemy">The enemy that died.</param>
    private void OnEnemyDie(Enemy enemy)
    {
        EconomyEventBus.EarnMoney(enemy.GetCarriedMoney());
        
        if (coinPrefab)
        {
            GameObject coin = Instantiate(coinPrefab, enemy.transform.position, Quaternion.identity);
            if(SceneManager.sceneCount>1) SceneManager.MoveGameObjectToScene(coin.gameObject, SceneManager.GetSceneAt(1));
            
            coin.GetComponentInChildren<TextMeshProUGUI>().text = enemy.GetCarriedMoney().ToString();
        }

        if (mainEnemiesList!=null && mainEnemiesList.Contains(enemy)) 
            mainEnemiesList.Remove(enemy);
    }

    /// <summary>
    /// Invoked when an enemy reaches the final target. 
    /// Increments the counter and checks if the lose condition is met.
    /// </summary>
    private void EnemyReachedTarget()
    {
        currentEnemiesAtGoal++;
        EnemyEventBus.UpdateEnemyCountAtTarget();
        if (currentEnemiesAtGoal == maxEnemiesAllowedAtTarget)
        {
            GameStateEventBus.Lose();
        }
    }

    /// <summary>
    /// Returns how many enemies have reached the goal so far.
    /// </summary>
    /// <returns>The count of enemies at the goal.</returns>
    public int GetEnemiesAtGoal()
    {
        return currentEnemiesAtGoal;
    }

    /// <summary>
    /// Returns the maximum allowed number of enemies that can reach the goal 
    /// before the player loses.
    /// </summary>
    /// <returns>The maximum number of enemies allowed at the goal.</returns>
    public int GetMaxEnemiesAtGoalAllowed()
    {
        return maxEnemiesAllowedAtTarget;
    }

    /// <summary>
    /// Checks whether the game can end with a win condition or 
    /// if it should trigger a lose, based on how many enemies made it to the goal 
    /// and the player's current game state.
    /// </summary>
    private void CheckGameFinalState()
    {
        if (currentEnemiesAtGoal < maxEnemiesAllowedAtTarget && !GameManager.Instance.gameLost)
        {
            GameStateEventBus.Win();
        }
        else if (!GameManager.Instance.gameWon)
        {
            GameStateEventBus.Lose();
        }
    }
}
