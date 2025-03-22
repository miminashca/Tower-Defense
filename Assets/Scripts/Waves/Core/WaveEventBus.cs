using System;

/// <summary>
/// The WaveEventBus class handles the flow of wave-related events, allowing different 
/// parts of the game to respond when a wave starts, ends, or when all waves have completed.
/// </summary>
public static class WaveEventBus
{
    /// <summary>
    /// Triggered at the start of a wave, providing the current wave number.
    /// </summary>
    public static event Action<int> OnWaveStart;
    
    /// <summary>
    /// Triggered at the end of a wave, providing the wave number that just finished.
    /// </summary>
    public static event Action<int> OnWaveEnd;
    
    /// <summary>
    /// Triggered when all waves have been spawned and completed.
    /// </summary>
    public static event Action OnWavesCompleted;

    /// <summary>
    /// Invokes the OnWaveStart event for the specified wave number.
    /// </summary>
    /// <param name="currentWave">The wave number that is starting.</param>
    public static void StartWave(int currentWave)
    {
        OnWaveStart?.Invoke(currentWave);
    }

    /// <summary>
    /// Invokes the OnWaveEnd event for the specified wave number.
    /// </summary>
    /// <param name="currentWave">The wave number that has just ended.</param>
    public static void EndWave(int currentWave)
    {
        OnWaveEnd?.Invoke(currentWave);
    }
    
    /// <summary>
    /// Invokes the OnWavesCompleted event, signaling that all waves are finished.
    /// </summary>
    public static void WavesCompleted()
    {
        OnWavesCompleted?.Invoke();
    }
}