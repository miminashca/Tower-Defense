using System;
using UnityEngine;

public static class EventBus
{
    
    
    public static event Action<int> OnMoneySpent;
    public static event Action<int> OnMoneyEarned;
    public static event Action OnMoneyAmountChange;
    
    
    public static event Action OnLose;
    public static event Action OnWin;

    
    
    
    
    
    
    
    
    
    public static void SpendMoney(int amount)
    {
        OnMoneySpent?.Invoke(amount);
        OnMoneyAmountChange?.Invoke();
    }
    public static void EarnMoney(int amount)
    {
        OnMoneyEarned?.Invoke(amount);
        OnMoneyAmountChange?.Invoke();
    }
    
    
    
    public static void Lose()
    {
        OnLose?.Invoke();
    }
    
    public static void Win()
    {
        OnWin?.Invoke();
    }
    
}