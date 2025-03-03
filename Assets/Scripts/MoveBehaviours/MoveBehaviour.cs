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

    protected bool targetReached = false;
    
    protected virtual void Start()
    {
        //mAnimator = GetComponent<Animator>();
        //PlayIdleAnimation();
    }

    public virtual void SetTargetPosition(Vector3 position)
    {
        targetPos = position;
        OnTargetSet?.Invoke();
        //PlayMovingAnimation();
    }

    protected virtual void Stop()
    {
        targetReached = true;
        OnGameobjectReachedTarget?.Invoke(gameObject, targetPos); 
        OnTargetReached?.Invoke();
    }

    private void Update()
    {
        if (Vector3.Magnitude(gameObject.transform.position - targetPos) <= targetRange && !targetReached)
        { 
            if(gameObject) Stop();
        }
    }

    // #region "animation related fields"
    // [SerializeField]
    // private Animator mAnimator;
    // //If idling animation is already playing, don't play it again.
    // private bool idleAnimationPlaying = false;
    // //If moving animation is already playing, don't play it again.
    // private bool moveAnimationPlaying = false;
    // //Sub-classes can use the following functions to play animations accordingly
    // protected void PlayMovingAnimation()
    // {
    //     idleAnimationPlaying = false;
    //     if (!moveAnimationPlaying)
    //     { 
    //         mAnimator.Play("Move");
    //         moveAnimationPlaying = true;
    //     }
    // }
    // protected void PlayIdleAnimation()
    // {
    //     moveAnimationPlaying = false;
    //     if (!idleAnimationPlaying)
    //     {
    //         mAnimator.Play("Idle");
    //         idleAnimationPlaying = true;
    //     }
    // }
    // #endregion
}