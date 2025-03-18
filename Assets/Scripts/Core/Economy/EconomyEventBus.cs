using System;

public static class EconomyEventBus
{
    public static event Action<int> OnMoneySpent;
    public static void SpendMoney(int amount)
    {
        OnMoneySpent?.Invoke(amount);
        OnMoneyAmountChange?.Invoke();
    }
    
    public static event Action<int> OnMoneyEarned;
    public static void EarnMoney(int amount)
    {
        OnMoneyEarned?.Invoke(amount);
        OnMoneyAmountChange?.Invoke();
    }
    
    public static event Action OnMoneyAmountChange;
    
}