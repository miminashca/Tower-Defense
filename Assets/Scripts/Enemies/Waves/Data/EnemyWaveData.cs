using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple serializable class that pairs an EnemyData type with a quantity (number),
/// specifying how many enemies of a certain type should be spawned in a wave.
/// </summary>
[System.Serializable]
public class TypeNum
{
    /// <summary>
    /// The specific EnemyData defining the stats and model for a certain enemy type.
    /// </summary>
    public EnemyData type;
    
    /// <summary>
    /// The number of enemies of this type to include in a wave.
    /// </summary>
    public int number;
}

/// <summary>
/// A ScriptableObject that holds data defining multiple waves of enemies, 
/// including how many enemies of each type appear in each wave and the time threshold between waves.
/// </summary>
[CreateAssetMenu(menuName = "Data/WaveData")]
public class EnemyWaveData : ScriptableObject
{
    /// <summary>
    /// A list of TypeNum objects representing enemy types and their quantities per wave.
    /// </summary>
    public List<TypeNum> waves = new List<TypeNum>();
    
    /// <summary>
    /// The delay (in seconds or frames) before starting the next wave.
    /// </summary>
    public int tresholdBetweenWaves = 0;

    /// <summary>
    /// Returns the total number of waves defined in the waves list.
    /// </summary>
    /// <returns>An integer representing how many waves are available.</returns>
    public int GetNumberOfWaves()
    {
        return waves.Count;
    }
}