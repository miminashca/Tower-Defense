using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer;
    private bool gameEnd;
    public event Action OnTimerEnd;
    public static Timer Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        WaveEventBus.OnWaveStart += SetWaveTimer;
        ShopEventBus.OnShopOpened += SetShopTimer;
        GameStateEventBus.OnGameEnd += EndGame;
        
        GameStateEventBus.OnReloadManagers += Reload;
    }
    private void Reload()
    {
        timer = -1;
        gameEnd = false;
    }
    private void OnDisable()
    {
        WaveEventBus.OnWaveStart -= SetWaveTimer;
        ShopEventBus.OnShopOpened -= SetShopTimer;
        GameStateEventBus.OnGameEnd -= EndGame;
        
        GameStateEventBus.OnReloadManagers -= Reload;
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else if ((int)timer == 0)
        {
            Debug.Log("Timer ends");
            OnTimerEnd?.Invoke();
        }
    }
    public int GetTimeLeft()
    {
        if(timer > 0f) return Mathf.RoundToInt(timer);
        else return 0;
    }
    private void SetTimer(int seconds)
    {
        timer = seconds;
    }
    private void SetShopTimer()
    {
        SetTimer(ShopManager.Instance.shopData.ShopDuration);
    }
    private void SetWaveTimer(int currentWave)
    {
        SetTimer(WaveManager.Instance.WavesData.tresholdBetweenWaves);
    }
    private void ResetTimer()
    {
        SetTimer(0);
    }
    private void EndGame()
    {
        gameEnd = true;
    }
}
