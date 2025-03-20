using System;
using UnityEngine;

/// <summary>
/// The EnemyEventBus class holds global events related to enemy actions and states.
/// It provides a central point where methods can invoke these events and where other
/// systems or components can subscribe to respond accordingly.
/// </summary>
public static class EnemyEventBus
{
    /// <summary>
    /// Triggered when enemies begin moving toward a specified position.
    /// The Vector3 parameter represents the target position.
    /// </summary>
    public static event Action<Vector3> OnEnemiesStartMoveToPosition;

    /// <summary>
    /// Invokes the OnEnemiesStartMoveToPosition event, signaling that enemies
    /// are beginning to move to a given position.
    /// </summary>
    /// <param name="position">The position enemies will move toward.</param>
    public static void EnemiesStartMoveToPosition(Vector3 position)
    {
        OnEnemiesStartMoveToPosition?.Invoke(position);
    }
    
    /// <summary>
    /// Triggered whenever an enemy reaches the final target or goal.
    /// </summary>
    public static event Action OnEnemyReachedTarget;

    /// <summary>
    /// Invokes the OnEnemyReachedTarget event, indicating an enemy has successfully
    /// reached the target.
    /// </summary>
    public static void EnemyReachedTarget()
    {
        OnEnemyReachedTarget?.Invoke();
    }
    
    /// <summary>
    /// Triggered when an enemy dies (i.e., is destroyed or HP reaches 0).
    /// The Enemy parameter indicates which enemy died.
    /// </summary>
    public static event Action<Enemy> OnEnemyDeath;

    /// <summary>
    /// Invokes the OnEnemyDeath event, indicating a specific enemy has died.
    /// </summary>
    /// <param name="enemy">The enemy that died.</param>
    public static void EnemyDied(Enemy enemy)
    {
        OnEnemyDeath?.Invoke(enemy);
    }
    
    /// <summary>
    /// Triggered whenever an enemy's HP is updated.
    /// The Enemy parameter indicates which enemy's HP changed.
    /// </summary>
    public static event Action<Enemy> OnUpdateEnemyHP;

    /// <summary>
    /// Invokes the OnUpdateEnemyHP event, indicating the specified enemy's HP
    /// has just been updated.
    /// </summary>
    /// <param name="enemy">The enemy whose HP changed.</param>
    public static void UpdateEnemyHP(Enemy enemy)
    {
        OnUpdateEnemyHP?.Invoke(enemy);
    }

    /// <summary>
    /// Triggered to indicate that the count of enemies at the target has changed.
    /// Another system can use this to refresh UI or trigger logic based on how many
    /// have reached the goal.
    /// </summary>
    public static event Action OnUpdateEnemyCountAtTarget;

    /// <summary>
    /// Invokes the OnUpdateEnemyCountAtTarget event, signaling a change in 
    /// the count of enemies currently at the target.
    /// </summary>
    public static void UpdateEnemyCountAtTarget()
    {
        OnUpdateEnemyCountAtTarget?.Invoke();
    }
}
