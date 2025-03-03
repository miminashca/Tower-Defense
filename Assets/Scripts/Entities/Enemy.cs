using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [NonSerialized] public MoveBehaviour moveBehaviour;
    public void Start()
    {
        Init(EntityData.DataType.EnemyData);
        if (!GetComponent<MoveBehaviour>()) moveBehaviour = gameObject.AddComponent(typeof(MoveBehaviour)) as MoveBehaviour;
        else
        {
            moveBehaviour = GetComponent<MoveBehaviour>();
        }
        if (!moveBehaviour) Debug.Log("Enemy has no move behaviour!");
        
        
        if(data is EnemyData enemyData) Instantiate(enemyData.enemyModelPrefab, gameObject.transform);
        
        //Start moving
        Invoke("StartMove", 0.5f);
    }

    private void StartMove()
    {
        moveBehaviour.SetTargetPosition(GameManager.enemyManager.targetPos);
    }
    public int GetCarriedMoney()
    {
        if(data is EnemyData enemyData) return enemyData.carriedMoney;
        return -0;
    }
}
