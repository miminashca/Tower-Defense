using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private int maxEnemiesAllowedAtTarget = 5;
    [SerializeField] private GameObject coinPrefab;
    public Vector3 TargetPosition { get; private set; }
    
    private List<Enemy> mainEnemiesList;
    private int currentEnemiesAtGoal;
    
    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else Destroy(gameObject);
        
        mainEnemiesList = new List<Enemy>();
        
        WaveEventBus.OnWavesCompleted += CheckGameFinalState;
        
        EnemyEventBus.OnEnemyDeath += OnEnemyDie;
        EnemyEventBus.OnEnemyReachedTarget += EnemyReachedTarget;
    }

    private void Start()
    {
        if (!targetTransform) Debug.Log("target not set in enemy controller, using default");
        TargetPosition = targetTransform.transform.position;
    }

    private void OnDestroy()
    {
        WaveEventBus.OnWavesCompleted -= CheckGameFinalState;
        EnemyEventBus.OnEnemyDeath -= OnEnemyDie;
        EnemyEventBus.OnEnemyReachedTarget -= EnemyReachedTarget;
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
        if(currentEnemiesAtGoal==maxEnemiesAllowedAtTarget) EventBus.Lose();
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
        if(currentEnemiesAtGoal<maxEnemiesAllowedAtTarget && !GameManager.Instance.gameLost) EventBus.Win();
        else if(!GameManager.Instance.gameWon)
        {
            EventBus.Lose();
        }
    }
}
