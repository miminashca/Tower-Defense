using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int maxEnemiesAllowedAtTarget = 5;
    [SerializeField] private GameObject coinPrefab;
    
    private Vector3 TargetPosition;
    private List<Enemy> mainEnemiesList;
    private int currentEnemiesAtGoal;
    
    public static EnemyManager Instance { get; private set; }

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

    private void Reload()
    {
        if (mainEnemiesList == null)  mainEnemiesList = new List<Enemy>();
        else
        {
            mainEnemiesList.Clear();
        }

        currentEnemiesAtGoal = 0;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
             TargetPosition = GameObject.FindWithTag("FinishTransform").transform.position;
        }
    }

    private void OnDestroy()
    {
        WaveEventBus.OnWavesCompleted -= CheckGameFinalState;
        EnemyEventBus.OnEnemyDeath -= OnEnemyDie;
        EnemyEventBus.OnEnemyReachedTarget -= EnemyReachedTarget;
        
        GameStateEventBus.OnReloadManagers -= Reload;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    public void AddEnemies(List<Enemy> enemiesToAdd)
    {
        foreach (Enemy enemy in enemiesToAdd)
        {
            if(!mainEnemiesList.Contains(enemy)) mainEnemiesList.Add(enemy);
        }
        EnemyEventBus.EnemiesStartMoveToPosition(TargetPosition);
    }

    private void OnEnemyDie(Enemy enemy)
    {
        EconomyEventBus.EarnMoney(enemy.GetCarriedMoney());
        
        if (coinPrefab)
        {
            GameObject coin = Instantiate(coinPrefab, enemy.transform.position, Quaternion.identity);
            coin.GetComponentInChildren<TextMeshProUGUI>().text = enemy.GetCarriedMoney().ToString();
        }

        if (mainEnemiesList.Contains(enemy)) mainEnemiesList.Remove(enemy);
    }

    private void EnemyReachedTarget()
    {
        currentEnemiesAtGoal++;
        EnemyEventBus.UpdateEnemyCountAtTarget();
        if(currentEnemiesAtGoal==maxEnemiesAllowedAtTarget) GameStateEventBus.Lose();
    }

    public int GetEnemiesAtGoal()
    {
        return currentEnemiesAtGoal;
    }
    public int GetMaxEnemiesAtGoalAllowed()
    {
        return maxEnemiesAllowedAtTarget;
    }

    private void CheckGameFinalState()
    {
        if(currentEnemiesAtGoal<maxEnemiesAllowedAtTarget && !GameManager.Instance.gameLost) GameStateEventBus.Win();
        else if(!GameManager.Instance.gameWon)
        {
            GameStateEventBus.Lose();
        }
    }
}
