using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public static System.Action<Entity> OnEntityDeath;
    public EntityData data;
    protected int healthPoints;
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
    public virtual void GetDamage(int amountOfDmg = 1)
    {
        healthPoints -= amountOfDmg;
        Debug.Log("Entity is attacked!!");
        if (GetHP() <= 0)
        {
            OnEntityDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
    
    public int GetHP()
    {
        return healthPoints;
    }
    
    public int GetSpeed()
    {
        return speed;
    }
}