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
        if (!targetTransform) Debug.Log("target not set in enemy controller, using default");
        mainEnemiesList = new List<Enemy>();
    }

    private void Start()
    {
        Enemy.OnEntityDeath += OnEnemyDie;
        MoveBehaviour.OnGameobjectReachedTarget += SmthReachedTarget;

        targetPos = targetTransform.transform.position;
    }

    private void Update()
    {
        if(currentEnemiesAtGoal==maxEnemiesAllowed) Debug.Log($"Maximum amount of enemies at target: {maxEnemiesAllowed}. Game Over!!!");
    }

    private void OnDisable()
    {
        Enemy.OnEntityDeath -= OnEnemyDie;
        MoveBehaviour.OnGameobjectReachedTarget -= SmthReachedTarget;
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
}
