using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(Animator))]
public abstract class MoveBehaviour : MonoBehaviour
{
    [SerializeField] protected float targetRange = 0.2f;
    protected Vector3 targetPos;
    public static Action<GameObject, Vector3> OnGameobjectReachedTarget;
    public Action OnTargetReached;
    public Action OnTargetSet;
    protected float speed; 

    protected bool targetReached = false;
    
    protected virtual void Awake()
    {
        GetComponent<EnemyEventBus>().OnStartMoveToPosition += SetTargetPosition;
    }

    private void OnDestroy()
    {
        GetComponent<EnemyEventBus>().OnStartMoveToPosition -= SetTargetPosition;
    }

    public virtual void SetTargetPosition(Vector3 position)
    {
        targetPos = position;
        OnTargetSet?.Invoke();
    }

    protected virtual void Stop()
    {
        targetReached = true;
        OnGameobjectReachedTarget?.Invoke(gameObject, targetPos); 
        OnTargetReached?.Invoke();
    }

    protected virtual void FixedUpdate()
    {
        speed = gameObject.GetComponent<Enemy>().GetCurrentSpeed();
        Debug.Log(speed);
        if (Vector3.Magnitude(gameObject.transform.position - targetPos) <= targetRange && !targetReached)
        { 
            if(gameObject) Stop();
        }
    }
}