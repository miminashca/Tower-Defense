using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    private TowerData.ImpactType impactType;
    private TowerData.TargetSelectingType targetSelectingType;
    private float impact;
    private float threshold;
    
    private SphereCollider trigger;
    private List<Enemy> targets;
    
    private TowerAttackBehaviour attackBehavior;
    private TargetSelectingBehaviour selectingBehaviour; 
    
    private float timer;
    
    private bool initialized = false;
    public void Initialize(TowerData.ImpactType pImpactType, TowerData.TargetSelectingType pTargetSelectingType, float pRange, float pImpact, float pThreshold)
    {
        impactType = pImpactType;
        targetSelectingType = pTargetSelectingType;
        InitAttackAndSelectBeh();
        
        if(!trigger) trigger = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
        trigger.isTrigger = true;

        trigger.radius = pRange;
        impact = pImpact;
        threshold = pThreshold;
        
        timer = threshold;
        
        initialized = true;
    }

    protected void Start(){
        targets = new List<Enemy>();
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
                List<Enemy> chosenTargets = ChooseTargets(targets);
                if (chosenTargets.Count != 0)
                {
                    foreach (Enemy target in chosenTargets)
                    {
                        Attack(target);
                    }
                }
            }
        }
    }

    private void InitAttackAndSelectBeh()
    {
        if (impactType == TowerData.ImpactType.Damage) attackBehavior = new TowerAttackBehaviourDamage();
        else if (impactType == TowerData.ImpactType.Debuff) attackBehavior = new TowerAttackBehaviourDebuff();
        
        if (targetSelectingType == TowerData.TargetSelectingType.Closest) selectingBehaviour = new TargetSelectingBehaviourClosest();
        else if (targetSelectingType == TowerData.TargetSelectingType.AOE) selectingBehaviour = new TargetSelectingBehaviourAOE();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && !targets.Contains(enemy))
        {
            targets.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && targets.Contains(enemy))
        {
            targets.Remove(enemy);
        }
    }

    protected void Attack(Enemy enemy)
    {
        if (enemy && GetComponent<Tower>().IsActive)
        {
            //instantiate bullet
            Bullet bullet = Instantiate(GetComponent<Tower>().BulletPrefab, gameObject.transform.position, quaternion.identity);
            bullet.GetComponent<Rigidbody>().linearVelocity =
                Vector3.Normalize(enemy.transform.position - gameObject.transform.position) * GetComponent<Tower>().BulletSpeed;
            Debug.Log("Tower attacks enemy!");
            attackBehavior.Attack(enemy, impact, bullet);
        }
    }

    protected List<Enemy> ChooseTargets(List<Enemy> pTargets)
    {
        return selectingBehaviour.GetTargets(pTargets, transform.position);
    }
}
