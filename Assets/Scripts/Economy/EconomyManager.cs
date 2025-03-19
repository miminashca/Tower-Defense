using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public int StartCapital = 30;
    
    private int moneyEarned = 0;
    public static EconomyManager Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        EconomyEventBus.OnMoneySpent += SpendMoney;
        EconomyEventBus.OnMoneyEarned += EarnMoney;
        
        GameStateEventBus.OnReloadManagers += Reload;
    }

    private void Reload()
    {
        moneyEarned = StartCapital;
    }

    private void OnDestroy()
    {
        EconomyEventBus.OnMoneySpent -= SpendMoney;
        EconomyEventBus.OnMoneyEarned -= EarnMoney;
        
        GameStateEventBus.OnReloadManagers -= Reload;
    }
    
    private void EarnMoney(int amount)
    {
        moneyEarned += amount;
    }
    private void SpendMoney(int amount)
    {
        if(amount>GetMoney()) return;
        moneyEarned -= amount;
    }
    public int GetMoney()
    {
        return moneyEarned;
    }
}

