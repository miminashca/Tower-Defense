using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public TowerData.ImpactType impactType;
    public TowerData.TargetSelectingType targetSelectingType;
    
    private SphereCollider trigger;
    private List<Entity> targets;
    
    private int impact;
    private float timer;
    private float threshold;
        
    private TowerAttackBehaviour attackBehavior;
    private TargetSelectingBehaviour selectingBehaviour; 
    
    private bool initialized = false;

    private void OnEnable()
    {
        EventBus.OnEntityDeath += RemoveEntityFromTargets;
    }

    private void OnDisable()
    {
        EventBus.OnEntityDeath -= RemoveEntityFromTargets;
    }

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

    protected void Awake(){
        targets = new List<Entity>();
        
        if (impactType == TowerData.ImpactType.Damage) attackBehavior = new TowerAttackBehaviourDamage();
        else if (impactType == TowerData.ImpactType.Debuff) attackBehavior = new TowerAttackBehaviourDebuff();
        
        if (targetSelectingType == TowerData.TargetSelectingType.Closest) selectingBehaviour = new TargetSelectingBehaviourClosest();
        //else if (targetSelectingType == TowerData.TargetSelectingType.AOE) selectingBehaviour = new TargetSelectingBehaviourAOE();
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
            
            if (targets.Count != 0)
            {
                List<Entity> chosenTargets = ChooseTargets(targets);
                if (chosenTargets.Count != 0)
                {
                    foreach (Entity target in chosenTargets)
                    {
                        Attack(target);
                    }
                }
                //Attack(ChooseTarget(targets)); 
                // List<Entity> currentTargets = ChooseTargets(targets);
                // foreach (Entity target in currentTargets)
                // {
                //     Attack(target); 
                // }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity != null && !targets.Contains(entity))
        {
            targets.Add(entity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity != null && targets.Contains(entity))
        {
            targets.Remove(entity);
        }
    }

    protected void Attack(Entity entity)
    {
        if (entity && GetComponent<Tower>().isActive)
        {
            Debug.Log("Tower attacks enemy!");
            attackBehavior.Attack(entity, impact);
        }
    }

    protected List<Entity> ChooseTargets(List<Entity> pTargets)
    {
        return selectingBehaviour.GetTargets(pTargets, transform.position);
    }

    private void RemoveEntityFromTargets(Entity entity)
    {
        if (targets.Contains(entity))
        {
            targets.Remove(entity);
        }
    }
}
