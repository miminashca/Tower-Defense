using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer = 0;

    private void Awake()
    {
        EventBus.OnWaveStart += SetWaveTimer;
        EventBus.OnShopOpened += SetShopTimer;
    }

    private void OnDisable()
    {
        EventBus.OnWaveStart -= SetWaveTimer;
        EventBus.OnShopOpened -= SetShopTimer;
    }

    private void FixedUpdate()
    {
        if(timer>0) timer -= Time.deltaTime;
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
        SetTimer(GameManager.shopManager.shopData.shopDuration);
    }
    private void SetWaveTimer(int currentWave)
    {
        SetTimer(GameManager.waveManager.wavesData.tresholdBetweenWaves);
    }
    private void ResetTimer()
    {
        SetTimer(0);
    }
}
