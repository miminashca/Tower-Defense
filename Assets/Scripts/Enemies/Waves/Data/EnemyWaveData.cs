using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

//[SerializedDictionary("NumberOfEnemiesInAWave", "EnemyTypeDataInAWave")]

[System.Serializable]
public class TypeNum
{
    public EnemyData type;
    public int number;
}

[CreateAssetMenu(menuName = "Data/WaveData")]
public class EnemyWaveData : ScriptableObject
{
    //public SerializedDictionary<int ,EnemyData> waves = new SerializedDictionary<int ,EnemyData>();
    public List<TypeNum> waves = new List<TypeNum>();
    public int tresholdBetweenWaves = 0;

    public int GetNumberOfWaves()
    {
        return waves.Count;
    }
}