using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI targetEnemyCounterText;
    [SerializeField] private TextMeshProUGUI finalGameStateMessage;

    private void Awake()
    {
        if (waveText) WaveEventBus.OnWaveStart += UpdateWaveText;
        EventBus.OnLose += ShowLoseGameState;
        EventBus.OnWin += ShowWinGameState;
    }

    private void Start()
    {
        if (moneyText)
        {
            EventBus.OnMoneyAmountChange += UpdateMoneyText;
            moneyText.text = GameManager.gameManager.GetMoney().ToString();
        }
        if (targetEnemyCounterText) EnemyEventBus.OnEnemyReachedTarget += UpdateEnemyCountText;
    }

    private void FixedUpdate()
    { 
        UpdateTimer();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = GameManager.gameManager.GetMoney().ToString();
    }

    private void UpdateTimer()
    {
        timerText.text = GameManager.timer.GetTimeLeft().ToString();
    }
    
    private void UpdateWaveText(int waveNum)
    {
        waveText.text = waveNum + "/" + GameManager.waveManager.wavesData.GetNumberOfWaves();
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
        EventBus.OnMoneyAmountChange -= UpdateMoneyText;
        WaveEventBus.OnWaveStart -= UpdateWaveText;
        EnemyEventBus.OnEnemyReachedTarget -= UpdateEnemyCountText;
        EventBus.OnLose -= ShowLoseGameState;
        EventBus.OnWin -= ShowWinGameState;
    }
}
