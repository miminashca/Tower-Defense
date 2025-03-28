using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    private bool isActive = false;
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

    private void Awake()
    {
        TowerEventBus.OnTowerStartAttack += SetTowerActive;
        TowerEventBus.OnTowerEndAttack += SetTowerUnactive;
    }
    private void OnDestroy()
    {
        TowerEventBus.OnTowerStartAttack -= SetTowerActive;
        TowerEventBus.OnTowerEndAttack -= SetTowerUnactive;
    }

    protected void Start(){
        targets = new List<Enemy>();
    }
    
    void Update()
    {
        if(isActive) Execute();
    }

    private void Execute()
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

    private void SetTowerActive(GameObject tower)
    {
        if(tower == gameObject) isActive = true;
    }
    private void SetTowerUnactive(GameObject tower)
    {
        if(tower == gameObject) isActive = false;
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

    private void Attack(Enemy enemy)
    {
        if (enemy)
        {
            //Debug.Log("attack");
            //instantiate bullet
            Bullet bullet = Instantiate(GetComponent<Tower>().BulletPrefab, gameObject.transform.position, quaternion.identity);
            bullet.Target = enemy.gameObject;
            if(SceneManager.sceneCount>1) SceneManager.MoveGameObjectToScene(bullet.gameObject, SceneManager.GetSceneAt(1));
            
            //Debug.Log("Tower attacks enemy!");
            attackBehavior.Attack(enemy, impact, bullet);
        }
    }

    private List<Enemy> ChooseTargets(List<Enemy> pTargets)
    {
        return selectingBehaviour.GetTargets(pTargets, transform.position);
    }
}
