using System;
using UnityEngine;

/// <summary>
/// The EconomyManager class maintains the player's current money balance, 
/// allowing external systems to spend or earn currency. It subscribes to 
/// relevant events from the EconomyEventBus and GameStateEventBus to handle 
/// updates or resets as needed.
/// </summary>
public class EconomyManager : Manager<EconomyManager>
{
    /// <summary>
    /// Initial starting money to be set whenever the manager is reloaded.
    /// </summary>
    public int StartCapital = 30;
    
    /// <summary>
    /// Tracks the current amount of money the player has.
    /// </summary>
    private int moneyEarned = 0;

    /// <summary>
    /// Subscribes to spend/earn events, 
    /// and reloads state upon manager reset events.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        
        EconomyEventBus.OnMoneySpent += SpendMoney;
        EconomyEventBus.OnMoneyEarned += EarnMoney;
    }

    /// <summary>
    /// Resets the manager's moneyEarned to the StartCapital value, 
    /// typically invoked on a game or scene reload.
    /// </summary>
    protected override void Reload()
    {
        moneyEarned = StartCapital;
    }

    /// <summary>
    /// Unsubscribes from all related events upon destruction to prevent 
    /// dangling references or memory leaks.
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        EconomyEventBus.OnMoneySpent -= SpendMoney;
        EconomyEventBus.OnMoneyEarned -= EarnMoney;
    }
    
    /// <summary>
    /// Increases the player's money by the specified amount.
    /// </summary>
    /// <param name="amount">How much money to add.</param>
    private void EarnMoney(int amount)
    {
        moneyEarned += amount;
    }
    
    /// <summary>
    /// Decreases the player's money by the specified amount, 
    /// provided the player has enough funds.
    /// </summary>
    /// <param name="amount">How much money to deduct.</param>
    private void SpendMoney(int amount)
    {
        if(amount > GetMoney()) return;
        moneyEarned -= amount;
    }
    
    /// <summary>
    /// Retrieves the current amount of money available to the player.
    /// </summary>
    /// <returns>The player's current balance.</returns>
    public int GetMoney()
    {
        return moneyEarned;
    }
}
