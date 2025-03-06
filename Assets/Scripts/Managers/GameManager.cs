using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }
    public static EnemyManager enemyManager { get; private set; }
    public static WaveManager waveManager { get; private set; }
    public static InputManager inputManager { get; private set; }
    public static UIManager uiManager { get; private set; }
    public static TowerManager towerManager { get; private set; }
    public static ShopManager shopManager { get; private set; }
    public static TileFloor tileFloor { get; private set; }
    public static Timer timer { get; private set; }
    
    private int moneyEarned = 30;
    public bool gameWon = false;
    public bool gameLost = false;
    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
        if (!inputManager) inputManager = GetComponent<InputManager>();
        if (!enemyManager) enemyManager = GetComponent<EnemyManager>();
        if (!waveManager) waveManager = GetComponent<WaveManager>();
        if (!uiManager) uiManager = GetComponentInChildren<UIManager>();
        if (!towerManager) towerManager = GetComponent<TowerManager>();
        if (!shopManager) shopManager = GetComponentInChildren<ShopManager>();
        if (!timer) timer = GetComponent<Timer>();
        
        if (!inputManager) Debug.Log("No Input manager in Game manager!!!");
        if (!enemyManager) Debug.Log("No Enemy manager in Game manager!!!");
        if (!waveManager) Debug.Log("No Wave manager in Game manager!!!");
        if (!uiManager) Debug.Log("No UI manager in Game manager!!!");
        if (!towerManager) Debug.Log("No Tower manager in Game manager!!!");
        if (!shopManager) Debug.Log("No Shop manager in Game manager!!!");
        if (!timer) Debug.Log("No Timer in Game manager!!!");

        SceneManager.sceneLoaded += OnSceneLoaded;
        EventBus.OnMoneySpent += SpendMoney;
        EventBus.OnMoneyEarned += AddMoney;
        EventBus.OnWin += Win;
        EventBus.OnLose += Lose;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventBus.OnMoneySpent -= SpendMoney;
        EventBus.OnMoneyEarned -= AddMoney;
        EventBus.OnWin -= Win;
        EventBus.OnLose -= Lose;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        tileFloor = FindObjectOfType<TileFloor>(true);
    }
    
    private void AddMoney(int amount)
    {
        moneyEarned += amount;
    }
    public int GetMoney()
    {
        return moneyEarned;
    }
    private void SpendMoney(int amount)
    {
        if(amount>GetMoney()) return;
        moneyEarned -= amount;
    }

    private void Win()
    {
        gameWon = true;
    }
    private void Lose()
    {
        gameLost = true;
    }
}

