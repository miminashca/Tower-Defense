using UnityEngine;

/// <summary>
/// A ScriptableObject holding configuration data for an enemy, 
/// including its base health, speed, carried money value, and a prefab model.
/// </summary>
[CreateAssetMenu(menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    /// <summary>
    /// The total health points this enemy starts with.
    /// </summary>
    public float HealthPoints = 2;
    
    /// <summary>
    /// The movement speed of the enemy.
    /// </summary>
    public float Speed = 2;
    
    /// <summary>
    /// How much money the enemy grants to the player upon death.
    /// </summary>
    public int CarriedMoney = 0;
    
    /// <summary>
    /// The 3D model prefab for the enemy, spawned when the enemy is created in the scene.
    /// </summary>
    public GameObject Model;
}