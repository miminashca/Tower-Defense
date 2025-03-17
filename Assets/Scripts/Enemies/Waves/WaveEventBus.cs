using System;
using UnityEngine;

public static class WaveEventBus
{
    public static event Action<int> OnWaveStart;
    public static event Action<int> OnWaveEnd;
    public static event Action OnWavesCompleted;

    public static void StartWave(int currentWave)
    {
        OnWaveStart?.Invoke(currentWave);
    }
    public static void EndWave(int currentWave)
    {
        OnWaveEnd?.Invoke(currentWave);
    }
    public static void WavesCompleted()
    {
        OnWavesCompleted?.Invoke();
    }
}