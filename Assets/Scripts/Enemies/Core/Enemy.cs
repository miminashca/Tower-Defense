using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    public EnemyData Data;
    
    [NonSerialized] public MoveBehaviour MoveBehaviour;
    private float currentHP;
    private float currentSpeed;
    private bool debuffed = false;

    private void OnEnable()
    {
        EventBus.OnTowerDebuffedEntity += GetDebuff;
    }
    private void OnDisable()
    {
        EventBus.OnTowerDebuffedEntity -= GetDebuff;
    }
    void Awake()
    {
        //Initialize Data
        if (Data != null)
        {
            currentHP = Data.HealthPoints;
            currentSpeed = Data.Speed;
            //Instantiate Enemy Model from Data
            Instantiate(Data.Model, gameObject.transform);
        }
        else
        {
            Debug.LogError($"No data asset attached to {name}!!!");
        }
        
        //Initialize MoveBehaviour
        MoveBehaviour = GetComponent<MoveBehaviour>();
        if (!MoveBehaviour) Debug.Log($"No move behaviour attached to {name}!!!");
    }
    
    public void GetDamage(float amountOfDmg = 1)
    {
        currentHP -= amountOfDmg;
        EventBus.EntityReceivedDamage(this);
        if (GetCurrentHP() <= 0)
        {
            EventBus.EntityDie(this);
            Destroy(gameObject);
        }
    }
    private void GetDebuff(Enemy enemy, float debuffDuration = 1)
    {
        if(!enemy ==this || debuffed) return;
        debuffed = true;
        currentSpeed *= 0.5f;
        Invoke("RemoveDebuff", debuffDuration);
    }
    private void RemoveDebuff()
    {
        if(!debuffed) return;
        debuffed = false;
        currentSpeed = Data.Speed;
    }
    
    public float GetCurrentHP()
    {
        return currentHP;
    }
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
    public int GetCarriedMoney()
    {
        if(Data) return Data.CarriedMoney;
        return -0;
    }
    
}