using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBehaviour : MonoBehaviour
{
    private SphereCollider trigger;
    protected List<Entity> targets;
    
    private int impact;
    private float timer;
    private float threshold;
    
    protected TargetSelectingBehaviour targetSelectingBehaviour = null;

    private bool initialized = false;
    public void Initialize(int pRange, int pImpact, float pThreshold)
    {
        if(!trigger) trigger = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
        trigger.isTrigger = true;

        trigger.radius = 0;
        trigger.radius = pRange;
        impact = pImpact;
        threshold = pThreshold;
        
        timer = threshold;
        
        initialized = true;
    }

    protected void Start(){
        targets = new List<Entity>();
    }
    
    void Update()
    {
        if (!initialized)
        {
            Debug.Log("Tower behaviour not initialized!");
            return;
        }
        
        timer += Time.deltaTime;
        if (timer >= threshold)
        {
            timer = 0;
            Attack(ChooseTarget(targets));
        }
        //Debug.Log(targets.Count);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (targets == null)
        // {
        //     Debug.LogError("Targets list not initialized.");
        //     return;
        // }
        // Check if the other GameObject has an Entity component
        Entity entity = other.GetComponent<Entity>();
        if (entity != null && !targets.Contains(entity))
        {
            targets.Add(entity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove the entity when it exits the trigger
        Entity entity = other.GetComponent<Entity>();
        if (entity != null && targets.Contains(entity))
        {
            targets.Remove(entity);
        }
    }

    //Add tower battle behaviour (similar to tower selecting behaviour)
    //Make virtual(or abstract) and rename to "GetImpact" and move to abstract TowerBattleBehaviour.
    //Override in TowerBattleBehaviour subclasses (TowerBattleBehaviourDamage/TowerBattleBehaviourDebuff).
    protected void Attack(Entity entity)
    {
        if (entity && GetComponent<Tower>().isActive)
        {
            Debug.Log("Tower attacks enemy!");
            if (entity.GetHP() <= 1)
            {
                targets.Remove(entity);
            }
            entity.GetDamage(impact);
        }
    }

    protected Entity ChooseTarget(List<Entity> pTargets)
    {
        if(targetSelectingBehaviour) return targetSelectingBehaviour.GetTarget(pTargets);
        else
        {
            Debug.Log("no target sel beh");
            return null;
        }
    }
}
