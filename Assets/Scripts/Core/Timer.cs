using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer = -5;
    public event Action OnTimerEnd;
    public static Timer Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
        WaveEventBus.OnWaveStart += SetWaveTimer;
        EventBus.OnShopOpened += SetShopTimer;
    }

    private void OnDisable()
    {
        WaveEventBus.OnWaveStart -= SetWaveTimer;
        EventBus.OnShopOpened -= SetShopTimer;
    }

    private void Update()
    {
        if (timer > 0f) timer -= Time.deltaTime;
        else if((int)timer == 0)
        {
            Debug.Log("Timer ends");
            OnTimerEnd?.Invoke();
        }
    }
    public int GetTimeLeft()
    {
        return Mathf.RoundToInt(timer);
    }
    private void SetTimer(int seconds)
    {
        timer = seconds;
    }
    private void SetShopTimer()
    {
        SetTimer(ShopManager.Instance.shopData.shopDuration);
    }
    private void SetWaveTimer(int currentWave)
    {
        SetTimer(WaveManager.Instance.wavesData.tresholdBetweenWaves);
    }
    private void ResetTimer()
    {
        SetTimer(0);
    }
}
