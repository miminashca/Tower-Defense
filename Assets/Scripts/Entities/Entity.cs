using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public EntityData data;
    protected float healthPoints;
    protected float speed;

    private bool debuffed = false;

    private void OnEnable()
    {
        EventBus.OnTowerDebuffedEntity += GetDebuff;
    }
    private void OnDisable()
    {
        EventBus.OnTowerDebuffedEntity -= GetDebuff;
    }

    protected void Init(EntityData.DataType pType)
    {
        if (data != null && data.type == pType)
        {
            healthPoints = data.healthPoints;
            speed = data.speed;
        }
        else
        {
            data = null;
            Debug.LogError($"No or wrong type data asset attached to {name}!!!");
        }
    }
    public void GetDamage(float amountOfDmg = 1)
    {
        healthPoints -= amountOfDmg;
        EventBus.EntityReceivedDamage(this);
        if (GetHP() <= 0)
        {
            EventBus.EntityDie(this);
            Destroy(gameObject);
        }
    }
    private void GetDebuff(Entity entity, float debuffDuration = 1)
    {
        if(!entity ==this || debuffed) return;
        debuffed = true;
        speed *= 0.5f;
        Invoke("RemoveDebuff", debuffDuration);
    }

    private void RemoveDebuff()
    {
        if(!debuffed) return;
        debuffed = false;
        speed = data.speed;
    }
    
    public float GetHP()
    {
        return healthPoints;
    }
    
    public float GetSpeed()
    {
        return speed;
    }
    
    
}