using System;
using UnityEngine;

/// <summary>
/// Manages time scaling for both Unity's global time (Time.timeScale) 
/// and a separate scale for in-game objects. Provides functionality to pause or resume 
/// gameplay while still allowing certain UI or logical elements to continue functioning.
/// </summary>
public class TimeScaler : MonoBehaviour
{
    /// <summary>
    /// The global time scale affecting Unity's built-in time (Time.timeScale). 
    /// By default, 1 = normal speed, values > 1 speed up the game, values < 1 slow the game.
    /// Is never being set to 0.
    /// </summary>
    public float TimeScale = 1;
    
    /// <summary>
    /// A custom time scale used by game objects that move, to check if it is set to 0 - meaning the game is in pause state.
    /// </summary>
    [NonSerialized] public float TimeScaleForGameObjects = 1;
    
    /// <summary>
    /// Singleton-like reference for global access to the TimeScaler instance.
    /// </summary>
    public static TimeScaler Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance and subscribes to global pause/resume/reload events.
    /// </summary>
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        GameStateEventBus.OnPauseGame += SetZeroTimeScale;
        GameStateEventBus.OnResumeGame += ResetTimeScale;
        
        GameStateEventBus.OnReloadManagers += Reload;
    }

    /// <summary>
    /// Resets the custom time scale when managers are reloaded, ensuring the game starts unpaused.
    /// </summary>
    private void Reload()
    {
        ResetTimeScale();
    }

    /// <summary>
    /// Unsubscribes from relevant game state events upon destruction to avoid memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        GameStateEventBus.OnPauseGame -= SetZeroTimeScale;
        GameStateEventBus.OnResumeGame -= ResetTimeScale;
        
        GameStateEventBus.OnReloadManagers -= Reload;
    }

    /// <summary>
    /// Called once per frame. Sets Unity's global Time.timeScale to the configured TimeScale.
    /// If the custom time scale for game objects is nonzero, ensures it matches TimeScale.
    /// </summary>
    private void Update()
    {
        Time.timeScale = TimeScale;
        
        if (TimeScaleForGameObjects != 0f) 
        {
            ResetTimeScale();
        }
    }

    /// <summary>
    /// Sets the custom time scale to zero, effectively pausing in-game object movement 
    /// that relies on TimeScaleForGameObjects.
    /// </summary>
    private void SetZeroTimeScale()
    {
        //Debug.Log("set 0 time");
        TimeScaleForGameObjects = 0f; 
    }

    /// <summary>
    /// Resets the custom time scale to match the global time scale. 
    /// Used to resume normal movement for game objects.
    /// </summary>
    private void ResetTimeScale()
    {
        TimeScaleForGameObjects = TimeScale; 
    }
}
