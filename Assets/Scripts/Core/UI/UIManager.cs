using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI targetEnemyCounterText;
    [SerializeField] private TextMeshProUGUI finalGameStateMessage;
    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
        WaveEventBus.OnWaveStart += UpdateWaveText;
        EconomyEventBus.OnMoneyAmountChange += UpdateMoneyText;
        EnemyEventBus.OnUpdateEnemyCountAtTarget += UpdateEnemyCountText;
        
        GameStateEventBus.OnLose += ShowLoseGameState;
        GameStateEventBus.OnWin += ShowWinGameState;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
            UpdateWaveText(WaveManager.Instance.CurrentWave);
            UpdateMoneyText();
            UpdateEnemyCountText();
        }
    }

    private void Update()
    { 
        UpdateTimer();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = EconomyManager.Instance.GetMoney().ToString();
    }

    private void UpdateTimer()
    {
        timerText.text = Timer.Instance.GetTimeLeft().ToString();
    }
    
    private void UpdateWaveText(int waveNum)
    {
        waveText.text = waveNum + "/" + WaveManager.Instance.WavesData.GetNumberOfWaves();
    }
    
    private void UpdateEnemyCountText()
    {
        int x = EnemyManager.Instance.GetEnemiesAtGoal();
        int y = EnemyManager.Instance.GetMaxEnemiesAtGoalAllowed();
        if (x > y) x = y;
        
        targetEnemyCounterText.text = x + "/" + y;
    }

    private void ShowLoseGameState()
    {
        finalGameStateMessage.gameObject.SetActive(true);
        finalGameStateMessage.text = "YOU LOST!";
        finalGameStateMessage.color = Color.red;
        Debug.Log("GAME LOST!");
    }
    private void ShowWinGameState()
    {
        finalGameStateMessage.gameObject.SetActive(true);
        finalGameStateMessage.text = "YOU WON!";
        finalGameStateMessage.color = Color.cyan;
        Debug.Log("GAME WON!");
    }

    private void OnDisable()
    {
        EconomyEventBus.OnMoneyAmountChange -= UpdateMoneyText;
        WaveEventBus.OnWaveStart -= UpdateWaveText;
        EnemyEventBus.OnUpdateEnemyCountAtTarget -= UpdateEnemyCountText;
        GameStateEventBus.OnLose -= ShowLoseGameState;
        GameStateEventBus.OnWin -= ShowWinGameState;
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void Restart()
    {
        GameStateEventBus.Restart();
    }
}
