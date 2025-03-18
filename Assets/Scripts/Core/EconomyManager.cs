using UnityEngine;
using UnityEngine.SceneManagement;

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

        moneyEarned = StartCapital;
        
        EventBus.OnMoneySpent += SpendMoney;
        EventBus.OnMoneyEarned += EarnMoney;
    }
    private void OnDestroy()
    {
        EventBus.OnMoneySpent -= SpendMoney;
        EventBus.OnMoneyEarned -= EarnMoney;
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

