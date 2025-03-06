using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public EntityData data;
    protected float healthPoints;
    protected int speed;
    
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
    public virtual void GetDamage(float amountOfDmg = 1)
    {
        EventBus.EntityReceivedDamage(this);
        healthPoints -= amountOfDmg;
        Debug.Log("Entity is attacked!!");
        if (GetHP() <= 0)
        {
            EventBus.EntityDie(this);
            Destroy(gameObject);
        }
    }
    
    public float GetHP()
    {
        return healthPoints;
    }
    
    public int GetSpeed()
    {
        return speed;
    }
}