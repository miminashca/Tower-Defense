using System;

/// <summary>
/// The EconomyEventBus class handles global events related to the player's money, 
/// allowing different parts of the game to respond whenever money is spent or earned.
/// </summary>
public static class EconomyEventBus
{
    /// <summary>
    /// Event triggered when the player spends some amount of money.
    /// The int parameter indicates how much money was spent.
    /// </summary>
    public static event Action<int> OnMoneySpent;
    
    /// <summary>
    /// Invokes the OnMoneySpent event with the specified amount 
    /// and also triggers the OnMoneyAmountChange event to notify 
    /// that the total money value may have changed.
    /// </summary>
    /// <param name="amount">The amount of money being spent.</param>
    public static void SpendMoney(int amount)
    {
        OnMoneySpent?.Invoke(amount);
        OnMoneyAmountChange?.Invoke();
    }
    
    /// <summary>
    /// Event triggered when the player earns some amount of money.
    /// The int parameter indicates how much money was earned.
    /// </summary>
    public static event Action<int> OnMoneyEarned;
    
    /// <summary>
    /// Invokes the OnMoneyEarned event with the specified amount 
    /// and also triggers the OnMoneyAmountChange event to notify 
    /// that the total money value may have changed.
    /// </summary>
    /// <param name="amount">The amount of money being earned.</param>
    public static void EarnMoney(int amount)
    {
        OnMoneyEarned?.Invoke(amount);
        OnMoneyAmountChange?.Invoke();
    }
    
    /// <summary>
    /// Event triggered whenever a change is made to the player's total money,
    /// such as after spending or earning.
    /// </summary>
    public static event Action OnMoneyAmountChange;
}