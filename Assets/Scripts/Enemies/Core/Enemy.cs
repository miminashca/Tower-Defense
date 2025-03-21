using System;
using UnityEngine;

/// <summary>
/// Represents an individual enemy in the game. This class initializes the enemy's 
/// data (health, speed, model), manages health and debuff effects, and integrates 
/// with the enemy event bus for updates and notifications.
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Holds configuration data for the enemy, such as its health, speed, and model prefab.
    /// </summary>
    public EnemyData Data;

    /// <summary>
    /// A reference to the movement behavior script attached to the enemy. 
    /// Used for controlling pathfinding or navigation.
    /// </summary>
    [NonSerialized] public MoveBehaviour MoveBehaviour;
    
    /// <summary>
    /// Tracks the current HP of this enemy.
    /// </summary>
    private float currentHP;
    
    /// <summary>
    /// Tracks the current movement speed of this enemy, which can be altered by debuffs.
    /// </summary>
    private float currentSpeed;
    
    /// <summary>
    /// Indicates whether the enemy is currently debuffed (speed reduction is active).
    /// </summary>
    private bool debuffed = false;

    /// <summary>
    /// Initializes the enemy's stats and movement behavior on object creation.
    /// </summary>
    void Awake()
    {
        // Initialize Data
        if (Data != null)
        {
            currentHP = Data.HealthPoints;
            currentSpeed = Data.Speed;
            
            // Instantiate Enemy Model from Data
            Instantiate(Data.Model, gameObject.transform);
        }
        else
        {
            Debug.LogError($"No data asset attached to {name}!!!");
        }
        
        // Initialize MoveBehaviour
        MoveBehaviour = GetComponent<MoveBehaviour>();
        if (!MoveBehaviour) 
            Debug.Log($"No move behaviour attached to {name}!!!");
    }
    
    /// <summary>
    /// Reduces the enemy's health by the specified damage amount. If HP drops to zero or below,
    /// the enemy notifies the event bus of its death and then destroys itself.
    /// </summary>
    /// <param name="amountOfDmg">Amount of damage dealt to the enemy.</param>
    public void GetDamage(float amountOfDmg = 1)
    {
        if(DebugOptions.InstantKill)
        {
            amountOfDmg = currentHP; // instantly kill enemy
        }
        
        currentHP -= amountOfDmg;
        EnemyEventBus.UpdateEnemyHP(this);
        
        if (GetCurrentHP() <= 0)
        {
            EnemyEventBus.EnemyDied(this);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Applies a debuff to the enemy that reduces its speed for the given duration. 
    /// If already debuffed, it does nothing.
    /// </summary>
    /// <param name="debuffDuration">How long the debuff lasts in seconds.</param>
    public void GetDebuff(float debuffDuration = 1)
    {
        if(debuffed) return;
        
        debuffed = true;
        currentSpeed *= 0.3f;
        Invoke("RemoveDebuff", debuffDuration);
    }

    /// <summary>
    /// Removes the debuff from the enemy, restoring the original movement speed.
    /// </summary>
    private void RemoveDebuff()
    {
        if(!debuffed) return;
        
        debuffed = false;
        currentSpeed = Data.Speed;
    }
    
    /// <summary>
    /// Returns the current HP value of the enemy.
    /// </summary>
    /// <returns>The current HP.</returns>
    public float GetCurrentHP()
    {
        return currentHP;
    }

    /// <summary>
    /// Returns the enemy's current movement speed (this may be lower than the base speed if debuffed).
    /// </summary>
    /// <returns>The current speed.</returns>
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    /// <summary>
    /// Returns the amount of money carried by the enemy, set in the associated EnemyData.
    /// </summary>
    /// <returns>Money carried by this enemy. Returns 0 if no data is present.</returns>
    public int GetCarriedMoney()
    {
        if(Data) return Data.CarriedMoney;
        return 0;
    }
}
