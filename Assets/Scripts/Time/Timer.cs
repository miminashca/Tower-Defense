using System;
using UnityEngine;

/// <summary>
/// The Timer class tracks a countdown that is set whenever a wave starts 
/// or the shop opens. When the timer reaches zero, it invokes an event to 
/// signal the end of the current phase (wave or shop).
/// </summary>
public class Timer : MonoBehaviour
{
    /// <summary>
    /// Internal float timer value representing the remaining time in seconds.
    /// </summary>
    private float timer;

    /// <summary>
    /// Event triggered once the timer reaches zero.
    /// </summary>
    public event Action OnTimerEnd;

    /// <summary>
    /// Provides a Singleton-like global reference to the Timer instance.
    /// </summary>
    public static Timer Instance { get; private set; }

    /// <summary>
    /// Initializes the Timer singleton, subscribes to events that set or reset the timer,
    /// and ensures only one Timer is active.
    /// </summary>
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        WaveEventBus.OnWaveStart += SetWaveTimer;
        ShopEventBus.OnShopOpened += SetShopTimer;
        
        GameStateEventBus.OnReloadManagers += Reload;
    }

    /// <summary>
    /// Resets the timer's internal value when managers are reloaded,
    /// effectively disabling the countdown until re-initialized.
    /// </summary>
    private void Reload()
    {
        timer = -1;
    }

    /// <summary>
    /// Unsubscribes from all events when this Timer is disabled,
    /// preventing potential memory leaks.
    /// </summary>
    private void OnDisable()
    {
        WaveEventBus.OnWaveStart -= SetWaveTimer;
        ShopEventBus.OnShopOpened -= SetShopTimer;
        
        GameStateEventBus.OnReloadManagers -= Reload;
    }

    /// <summary>
    /// Updates the countdown each frame. If the timer reaches 0, 
    /// it invokes the OnTimerEnd event once.
    /// </summary>
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

    /// <summary>
    /// Returns the remaining time on the timer as an integer, 
    /// clamped to 0 if time is already up.
    /// </summary>
    /// <returns>The number of seconds left on the timer.</returns>
    public int GetTimeLeft()
    {
        if(timer > 0f) 
            return Mathf.RoundToInt(timer);
        else 
            return 0;
    }

    /// <summary>
    /// Sets the internal countdown timer to a specific number of seconds.
    /// </summary>
    /// <param name="seconds">The new time (in seconds) for the timer.</param>
    private void SetTimer(int seconds)
    {
        timer = seconds;
    }

    /// <summary>
    /// Sets the timer for the duration specified in the current ShopData, 
    /// typically called when the shop opens.
    /// </summary>
    private void SetShopTimer()
    {
        SetTimer(ShopManager.Instance.shopData.ShopDuration);
    }

    /// <summary>
    /// Sets the timer to the wave threshold in the WaveManager's data, 
    /// typically called when a wave starts.
    /// </summary>
    /// <param name="currentWave">The index or number of the current wave.</param>
    private void SetWaveTimer(int currentWave)
    {
        SetTimer(WaveManager.Instance.WavesData.tresholdBetweenWaves);
    }

    /// <summary>
    /// Resets the timer to zero, effectively canceling any ongoing countdown.
    /// </summary>
    private void ResetTimer()
    {
        SetTimer(0);
    }
}
