using UnityEngine;

public abstract class MoveBehaviour : MonoBehaviour
{
    [SerializeField] protected float targetRange = 0.2f;
    
    protected Vector3 targetPos;
    protected float speed; 

    protected bool targetReached = false;
    
    protected virtual void Awake()
    {
        EnemyEventBus.OnEnemiesStartMoveToPosition += SetTargetPosition;
    }

    private void OnDestroy()
    {
        EnemyEventBus.OnEnemiesStartMoveToPosition -= SetTargetPosition;
    }

    public virtual void SetTargetPosition(Vector3 position)
    {
        if(!targetReached) targetPos = position;
    }

    protected virtual void Stop()
    {
        Debug.Log("Enemy reached Target");
        targetReached = true;
        EnemyEventBus.EnemyReachedTarget();
    }

    protected virtual void FixedUpdate()
    {
        if(TimeScaler.Instance.TimeScaleForGameObjects!=0f) speed = gameObject.GetComponent<Enemy>().GetCurrentSpeed();
        else
        {
            speed = 0;
        }
        //Debug.Log(speed);
        if (Vector3.Magnitude(gameObject.transform.position - targetPos) <= targetRange && !targetReached)
        { 
            if(gameObject) Stop();
        }
    }
}