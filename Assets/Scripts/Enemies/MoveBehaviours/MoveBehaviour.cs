using UnityEngine;

/// <summary>
/// An abstract base class for enemy movement behavior. It listens for a global event 
/// to set its target position, and stops the enemy when it reaches that position.
/// </summary>
public abstract class MoveBehaviour : MonoBehaviour
{
    /// <summary>
    /// The proximity threshold within which the enemy is considered to have reached its target.
    /// </summary>
    [SerializeField] protected float targetRange = 0.2f;

    /// <summary>
    /// The current target position to which this entity moves.
    /// </summary>
    protected Vector3 targetPos;

    /// <summary>
    /// The current speed at which the entity moves, typically retrieved from the Enemy component.
    /// </summary>
    protected float speed;

    /// <summary>
    /// Whether the entity has already reached the target.
    /// </summary>
    protected bool targetReached = false;
    
    /// <summary>
    /// Subscribes to the event that informs this behavior to move toward a given position.
    /// </summary>
    protected virtual void Awake()
    {
        EnemyEventBus.OnEnemiesStartMoveToPosition += SetTargetPosition;
    }

    /// <summary>
    /// Unsubscribes from the event when this object is destroyed, preventing potential memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        EnemyEventBus.OnEnemiesStartMoveToPosition -= SetTargetPosition;
    }

    /// <summary>
    /// Sets the target position for movement, provided the entity has not already reached it.
    /// </summary>
    /// <param name="position">The new target position for the entity.</param>
    public virtual void SetTargetPosition(Vector3 position)
    {
        if(!targetReached) 
            targetPos = position;
    }

    /// <summary>
    /// Called when the entity reaches its target. Marks the entity as having arrived 
    /// and raises the EnemyReachedTarget event.
    /// </summary>
    protected virtual void Stop()
    {
        Debug.Log("Enemy reached Target");
        targetReached = true;
        EnemyEventBus.EnemyReachedTarget();
    }

    /// <summary>
    /// Checks movement logic at a fixed time interval, accommodating 
    /// the current time scale and stopping if the target has been reached.
    /// </summary>
    protected virtual void FixedUpdate()
    {
        // Only move if the custom time scale is nonzero.
        if(TimeScaler.Instance.TimeScaleForGameObjects != 0f)
        {
            speed = gameObject.GetComponent<Enemy>().GetCurrentSpeed();
        }
        else
        {
            speed = 0;
        }

        // Check if we're within range of the target and haven't signaled arrival yet.
        if (Vector3.Magnitude(gameObject.transform.position - targetPos) <= targetRange && !targetReached)
        {
            if(gameObject) 
                Stop();
        }
    }
}
