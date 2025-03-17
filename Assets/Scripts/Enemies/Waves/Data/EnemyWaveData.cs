using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TypeNum
{
    public EnemyData type;
    public int number;
}

[CreateAssetMenu(menuName = "Data/WaveData")]
public class EnemyWaveData : ScriptableObject
{
    public List<TypeNum> waves = new List<TypeNum>();
    public int tresholdBetweenWaves = 0;

    public int GetNumberOfWaves()
    {
        return waves.Count;
    }
}