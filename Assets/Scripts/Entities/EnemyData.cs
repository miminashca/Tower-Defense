using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyData")]
public class EnemyData : EntityData
{
    public int carriedMoney = 0;
    public GameObject enemyModelPrefab;
    private void Awake()
    {
        type = DataType.EnemyData;
    }
}
