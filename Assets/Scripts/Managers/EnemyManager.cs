using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Vector3 targetPos { get; private set; }
    private List<Enemy> mainEnemiesList;
    private int currentEnemiesAtGoal;
    
    [SerializeField] private Transform targetTransform;
    [SerializeField] private int maxEnemiesAllowed = 5;
    [SerializeField] private GameObject coinPrefab;

    public Action OnEnemyReachedTarget;

    private void Awake()
    {
        EventBus.OnEntityDeath += OnEnemyDie;
        MoveBehaviour.OnGameobjectReachedTarget += SmthReachedTarget;
        EventBus.OnWavesCompleted += CheckGameFinalState;
        
        if (!targetTransform) Debug.Log("target not set in enemy controller, using default");
        mainEnemiesList = new List<Enemy>();
    }

    private void Start()
    {
        targetPos = targetTransform.transform.position;
    }

    private void OnDisable()
    {
        EventBus.OnEntityDeath -= OnEnemyDie;
        MoveBehaviour.OnGameobjectReachedTarget -= SmthReachedTarget;
        EventBus.OnWavesCompleted -= CheckGameFinalState;
    }

    private void OnEnemyDie(Entity entity)
    {
        if (entity is Enemy enemy)
        {
            EventBus.EarnMoney(enemy.GetCarriedMoney());
            
            if (coinPrefab)
            {
                GameObject coin = Instantiate(coinPrefab, enemy.transform.position, Quaternion.identity);
                coin.GetComponentInChildren<TextMeshProUGUI>().text = enemy.GetCarriedMoney().ToString();
            }

            if (mainEnemiesList.Contains(enemy)) mainEnemiesList.Remove(enemy);
        }
    }

    private void SmthReachedTarget(GameObject obj, Vector3 tar)
    {
        if (obj.GetComponent<Enemy>() && tar == targetPos)
        {
            currentEnemiesAtGoal++;
            if(currentEnemiesAtGoal==maxEnemiesAllowed) EventBus.Lose();
            OnEnemyReachedTarget?.Invoke();
        }
    }

    public void AddEnemies(List<Enemy> enemiesToAdd)
    {
        foreach (Enemy enemy in enemiesToAdd)
        {
            if(!mainEnemiesList.Contains(enemy)) mainEnemiesList.Add(enemy);
        }
    }

    public int GetEnemiesAtGoal()
    {
        return currentEnemiesAtGoal;
    }
    public int GetMaxEnemiesAtGoalAllowed()
    {
        return maxEnemiesAllowed;
    }

    private void CheckGameFinalState()
    {
        if(currentEnemiesAtGoal<maxEnemiesAllowed && !GameManager.gameManager.gameLost) EventBus.Win();
        else if(!GameManager.gameManager.gameWon)
        {
            EventBus.Lose();
        }
    }
}
