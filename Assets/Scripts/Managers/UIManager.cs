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

    private void Awake()
    {
        if (waveText) EventBus.OnWaveStart += UpdateWaveText;
    }

    private void Start()
    {
        if (moneyText)
        {
            EventBus.OnMoneyAmountChange += UpdateMoneyText;
            moneyText.text = GameManager.gameManager.GetMoney().ToString();
        }
        if (targetEnemyCounterText) GameManager.enemyManager.OnEnemyReachedTarget += UpdateEnemyCountText;
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
        if(GameManager.enemyManager.GetEnemiesAtGoal()<=GameManager.enemyManager.GetMaxEnemiesAtGoalAllowed()) targetEnemyCounterText.text = GameManager.enemyManager.GetEnemiesAtGoal() + "/" +  GameManager.enemyManager.GetMaxEnemiesAtGoalAllowed();
        else
        {
            targetEnemyCounterText.text = GameManager.enemyManager.GetMaxEnemiesAtGoalAllowed() + "/" +  GameManager.enemyManager.GetMaxEnemiesAtGoalAllowed();
        }
    }

    private void OnDisable()
    {
        EventBus.OnMoneyAmountChange -= UpdateMoneyText;
        EventBus.OnWaveStart -= UpdateWaveText;
        GameManager.enemyManager.OnEnemyReachedTarget -= UpdateEnemyCountText;
    }
}
