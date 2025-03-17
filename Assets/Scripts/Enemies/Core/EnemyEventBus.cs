using System;
using UnityEngine;

public static class EnemyEventBus
{
    public static event Action<Vector3> OnEnemiesStartMoveToPosition;
    public static void EnemiesStartMoveToPosition(Vector3 position)
    {
        OnEnemiesStartMoveToPosition?.Invoke(position);
    }
    
    public static event Action OnEnemyReachedTarget;
    public static void EnemyReachedTarget()
    {
        OnEnemyReachedTarget?.Invoke();
    }
    
    public static event Action<Enemy> OnEnemyDeath;
    public static void EnemyDied(Enemy enemy)
    {
        OnEnemyDeath?.Invoke(enemy);
    }
    
    public static event Action<Enemy> OnUpdateEnemyHP;
    public static void UpdateEnemyHP(Enemy enemy)
    {
        OnUpdateEnemyHP?.Invoke(enemy);
    }

}
